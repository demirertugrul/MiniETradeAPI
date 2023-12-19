using ETradeAPI.Application.ViewModels;
using FluentValidation;

namespace ETradeAPI.Application.Validators.Products
{
    public class CreateProductValidator : AbstractValidator<VM_Create_Product>
    {
        public CreateProductValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Lütfen ürün adını boş geçmeyiniz.")
                .MaximumLength(100)
                .MinimumLength(3)
                    .WithMessage("Lütfen ürün adını 3 ila 100 karakter arasında giriniz.");

            RuleFor(p => p.Stock)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Lütfen stok bilgisini boş geçmeyiniz.")
                .Must(NotNegative)
                    //.Must(s => s>=0)
                    .WithMessage("Stock bilgisi negatif olamaz.");

            RuleFor(p => p.Price)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Lütfen fiyat bilgisini boş geçmeyiniz.")
                .Must(p => p >= 0)
                    .WithMessage("Fiyat bilgisi negatif olamaz.");




        }

        private bool NotNegative(int s)
        {
            return s >= 0; //.Must(s => s>=0)
        }
    }
}
