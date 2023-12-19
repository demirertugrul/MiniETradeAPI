
using ETradeAPI.Application.Validators.Products;
using ETradeAPI.Infrastructure;
using ETradeAPI.Infrastructure.Enums;
using ETradeAPI.Infrastructure.Filters;
using ETradeAPI.Infrastructure.Services.Storage.Azure;
using ETradeAPI.Infrastructure.Services.Storage.Local;
using ETradeAPI.Persistence;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPersistenceService();
builder.Services.AddInfrastructureServices();

//builder.Services.AddStorage(StorageType.Azure);
//builder.Services.AddStorage(StorageType.Local);
//builder.Services.AddStorage(StorageType.AWS);
//builder.Services.AddStorage(StorageType.Local); //2. kullanim -> PEK TERC�H ED�LM�YOR.

//builder.Services.AddStorage<LocalStorage>(); //1. kullanim localde calisiyoruz
builder.Services.AddStorage<AzureStorage>();

builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
    policy.WithOrigins("http://localhost:4200", "https://localhost:4200").AllowAnyHeader().AllowAnyMethod()
));

builder.Services.AddControllers(options => options.Filters.Add<ValidationFilter>())
    .AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>()) // burada tek bir tane validator'� eklesek bile dierlerini de i�leme alacakt�r �unun vas�tas�yla; RegisterValidatorsFromAssemblyContaining.
    .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true); // Asp .net core'un kendisine g�re filtreleme yap�lanmas� var default olarak devrede olan. Biz yani bizler SuppressModelStateInvalidFilter �unu true yaparak buunu kendimize g�re ayarl�yoruz. Controller'a gelmeden Validator'lerine bak�yor do�rulam�yorsa direkt olarak client'a g�nderiyor default olan. Manuel olan yani bizim elimizde olan ise manuel olarak ayarlad���m�z ValidationFilter nesnesi ile client'e error'lari filtreliyoruz.

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    //app.UseDeveloperExceptionPage(); // .NET 5'te manuel ekliyorduk bunu. ama burada eklenmesine yok default geliyor
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
