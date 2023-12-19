using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ETradeAPI.Infrastructure.Filters
{
    public class ValidationFilter : IAsyncActionFilter // Filter yapılanmaları middleware'lerde oldugu gıbı sıralı bır sekılde calısırlar. 
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) /* bu sebeple 
            next fonksiyonu her filterin yani başka başka filterlarin kenidi icindeki OnActionExecutionAsync şu fonksiyonu delegate
            etmek ıcın şu görevle ActionExecutionDelegate next next() diyerek başka filterlardaki fonksiyonu delegate ediyoruz */
        {
            if (!context.ModelState.IsValid) /* context belirtilen Validator sınıfı (tek bitane belirtsek bile Program.cs'de 
            açıklaması var 'RegisterValidatorsFromAssemblyContaining bununla dierlerini de okuyor' ve belirtilen Validator'larda 
            geçersiz işlem varsa aşağıda hataları yakalıyoruz ve manuel olarak client'e gönderiyoruz.)*/
            {
                var errors = context.ModelState
                      .Where(x => x.Value.Errors.Any())
                      .ToDictionary(e => e.Key, e => e.Value.Errors.Select(e => e.ErrorMessage))
                      .ToArray();

                context.Result = new BadRequestObjectResult(errors);
                return; /* burada eğer Herhangi bir Validator'da hata çıktıysa ufacık bir yerinde bile direkt return ediyor aşağıdaki
                        next fonksiyonuna gelmiyor. Hata yoksa Bu Filter nesnesinde next ile diier Filter'lara geçiyoruz.*/
            }
            await next(); // dier filtre sınıfı varsa onun bununla 'ActionExecutionDelegate next' aynı methoduna geç diyoruz.
        }
    }
}
