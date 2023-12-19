using ETradeAPI.Application.Abstractions.Storage;
using ETradeAPI.Application.Repositories;
using ETradeAPI.Application.RequestParameters;
using ETradeAPI.Application.ViewModels;
using ETradeAPI.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ETradeAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        /*
         Task<IActionResult> Get() // Task yaparak IoC'de yapilan dispose'u(AddScoped) yutturuyoruz.
         Çünkü dispose edilene kadar Task'i bekliyor ve tamamlanınca dispose ediyor.*/
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IProductReadRepository _productReadRepository;
        private readonly IFileWriteRepository _fileWriteRepository;
        private readonly IFileReadRepository _fileReadRepository;
        private readonly IInvoiceFileWriteRepository _invoiceWriteRepository;
        private readonly IInvoiceFileReadRepository _invoiceReadRepository;
        private readonly IProductImageFileReadRepository _productImageFileReadRepository;
        private readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
        private readonly IStorageService _storageService;

        public ProductsController(IProductWriteRepository productWriteRepository/*, IFileService fileService*/, IProductReadRepository productReadRepository, IFileWriteRepository fileWriteRepository, IFileReadRepository fileReadRepository, IInvoiceFileWriteRepository invoiceWriteRepository, IInvoiceFileReadRepository invoiceReadRepository, IProductImageFileReadRepository productImageFileReadRepository, IProductImageFileWriteRepository productImageFileWriteRepository, IStorageService storageService)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            //_fileService = fileService;
            _fileWriteRepository = fileWriteRepository;
            _fileReadRepository = fileReadRepository;
            _invoiceWriteRepository = invoiceWriteRepository;
            _invoiceReadRepository = invoiceReadRepository;
            _productImageFileReadRepository = productImageFileReadRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _storageService = storageService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Pagination pagination)
        {
            #region "tracking"
            /* Product p = await _productReadRepository.GetByIdAsync("4a6f8790-f798-46e5-b2b2-214131bfc961", false); // tracking:
             verileri database'den cektigimiz zaman DbContext uzerinden izleyen mekanizma. Bunu false yaparak degisiklik yapsak dahi
             verilerde bir degisiklik olmayacaktir. 'Cache yonetimine benziyor'.*/
            //p.Name = "PC-tracking-false";
            //await _productWriteRepository.SaveAsync();
            #endregion
            #region "Interceptor -> CreateDate"
            //var customerId = Guid.NewGuid();
            //await _customerWriteRepository.AddAsync(new() { Id = customerId,Name="Phoebe"});

            //await _orderWriteRepository.AddAsync(new() { Description = "chi", Address = "perk",CustomerID=customerId });
            //await _orderWriteRepository.AddAsync(new() { Description = "chi 2", Address = "peqk", CustomerID=customerId});
            //await _orderWriteRepository.SaveAsync();
            #endregion
            /*
             Skip(pagination.Page * pagination.Size) burada gelen page(index 0'la başlıyor) 2 ise size ile çarpıyoruz ve o kadar 
             itemi atlıyoruz 'Skip()' ile. Daha sonra .Take(pagination.Size) ile kalan itemleri alıyoruz ve return ediyoruz.
             */
            await Task.Delay(1500); // spinner'in calismasini icin 1.5s beklettik.
            var totalProductCount = _productReadRepository.GetAll(false).Count(); // totalCount client'te pagination'ı kaç veri geldiyse ona göre ayarlıcak. page=0, index=5 ise 0*5'ten 0. page skip geç sonra 5 tane al. 1x5'ten 1. page skip geç sonra 5 al. 
            IQueryable products = _productReadRepository.GetAll(false).Skip(pagination.Page * pagination.Size).Take(pagination.Size).Select(p => new
            {
                p.Id,
                p.Name,
                p.Stock,
                p.Price,
                p.CreatedDate,
                p.UpdatedDate
            });
            return Ok(new
            {
                products,
                totalProductCount,
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return Ok(await _productReadRepository.GetByIdAsync(id, false));
        }


        [HttpPost]
        public async Task<IActionResult> Post(VM_Create_Product model)
        {
            await _productWriteRepository.AddAsync(new()
            {
                Name = model.Name,
                Price = model.Price,
                Stock = model.Stock
            });
            await _productWriteRepository.SaveAsync();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut]
        public async Task<IActionResult> Put(VM_Update_Product model)
        {
            var product = await _productReadRepository.GetByIdAsync(model.Id);
            product.Price = model.Price;
            product.Stock = model.Stock;
            product.Name = model.Name;
            await _productWriteRepository.SaveAsync(); // track ediliyorsa saveasync kullanabilirsin.
            return StatusCode((int)HttpStatusCode.OK);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _productWriteRepository.RemoveAsync(id);
            await _productWriteRepository.SaveAsync();
            return Ok();
        }


        [HttpPost("[action]")] // endPoint'te aldıgımız fileUpluadOptions'tan aldıgımız action.
        public async Task<IActionResult> Upload()
        {
            //uploadasync icine azure entegre olaabilsei icin tek dizin yapiyoz.
            var datas = await _storageService.UploadAsync("files", Request.Form.Files); // client'te formData olarak gonderdigimiz icin burada Request.From'dan yakalıyoruz parametreden degil.
            /*
             KRITIK BILGI -> IoC'de hangi storage'i verdiysek burda ona gore islem yapacak. IStorage'den implement
            edilen hepsi ioc'de verilince otomatik ona gore islem yapacak.
             */

            //var datas = await _fileService.UploadAsync("resource/product-images", Request.Form.Files); // UploadAsync bize file'in name'i ve path'ini geri donuyor
            _productImageFileWriteRepository.AddRangeAsync(datas.Select(d => new ProductImageFile()
            {
                FileName = d.fileName,
                Path = d.pathOrContainerName,
                Storage = _storageService.StorageName // veritabanında hangi storage'de calisma yaptigimizi gormek icin.
            }).ToList());
            _productImageFileWriteRepository.SaveAsync();


            //var datas = await _fileService.UploadAsync("resource/invoices", Request.Form.Files);
            //_invoiceWriteRepository.AddRangeAsync(datas.Select(d => new InvoiceFile()
            //{
            //    FileName = d.fileName,
            //    Path = d.path,
            //    Price = new Random().Next(),
            //}).ToList());
            //_invoiceWriteRepository.SaveAsync();


            //var datas = await _fileService.UploadAsync("resource/files", Request.Form.Files);
            //_fileWriteRepository.AddRangeAsync(datas.Select(d => new ETradeAPI.Domain.Entities.File()
            //{
            //    FileName = d.fileName,
            //    Path = d.path,
            //}).ToList());
            //_fileWriteRepository.SaveAsync();

            // TPH : DbContext icinde aciklandi.
            var d1 = _productImageFileReadRepository.GetAll(); // productImage ve invoice kendilerini getirirken
            var d2 = _invoiceReadRepository.GetAll(); // productImage ve invoice kendilerini getirirken

            var d3 = _fileReadRepository.GetAll(); // file base oldugu icin hepsini getiiroyr.
            return Ok();
        }
    }
}
