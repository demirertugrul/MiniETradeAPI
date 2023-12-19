using ETradeAPI.Application.Abstractions.Storage.Local;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace ETradeAPI.Infrastructure.Services.Storage.Local
{
    public class LocalStorage : Storage, ILocalStorage
    {
        readonly IWebHostEnvironment _webHostEnvironment;
        public LocalStorage(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task DeleteAsync(string path, string fileName)
            => File.Delete($"{path}\\{fileName}");

        public List<string> GetFiles(string path)
        {
            DirectoryInfo directory = new(path);
            return directory.GetFiles().Select(f => f.Name).ToList();
        }

        public bool HasFile(string path, string fileName)
            => File.Exists($"{path}\\{fileName}");

        public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string path, IFormFileCollection files)
        {
            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, path); /*webrootpath wwwroot 
                                                                                      klasorunun pathi*/
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            List<(string fileName, string path)> datas = new(); /* database'e isleem sokmak icin tuple ile() 
                                                                 dosyanin adini ve dosya yolunu aldik*/
            //List<bool> results = new(); // dosya wwwroot'a yuklendi mi ufacik bi control
            foreach (IFormFile file in files) /* Request.Form.Files client'te post ile gonderilen formData'yi
            yakaliyor. */
            {
                //dosyanin adi ayni ise onu isleme sokup numaralandirdik.
                //string fileNewName = await FileRenameOperation.FileRenameAsync(uploadPath, file.FileName); bu base hale gelecek.
                string fileNewName = await FileRenameAsync(path, file.Name, HasFile);

                await CopyFileAsync($"{uploadPath}\\{fileNewName}", file);
                datas.Add((fileNewName, $"{path}\\{fileNewName}"));
            }

            //if (results.TrueForAll(r => r.Equals(true)))
            //    return datas;

            return datas;

            //todo Eğer ki yukarıdaki if geçerli değilse burada dosyaların sunucuda yüklenirken hata alındığına dair uyarıcı bir exception oluşturulup fırlatılması gerekyior!
        }

        async private Task<bool> CopyFileAsync(string path, IFormFile file) /* client'ten upload edilen dosya 
                                                                            wwwroot'a copy ediliyor.*/
        {
            try
            {
                await using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);

                await file.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
                return true;
            }
            catch (Exception ex)
            {
                //todo log!
                throw ex;
            }
        }
    }
}
