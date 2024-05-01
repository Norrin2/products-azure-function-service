using FluentValidation;
using ProductsFunctionApp.Models;

namespace ProductsFunctionApp.Validation
{
    public class ProductValidator: AbstractValidator<Product>
    {
        public ProductValidator() 
        {
            RuleFor(x => x.CompanyId)
            .NotNull()
            .NotEqual(0)
            .WithMessage("Company Id must be informed");

            RuleFor(x => x.Id)
            .NotNull()
            .NotEqual(0)
            .WithMessage("Id must be informed");

            RuleFor(x => x.Name)
            .NotNull()
            .WithMessage("Product name must be informed");
        }
    }
}
