using FluentValidation;
using ProductsFunctionApp.Dto;

namespace ProductsFunctionApp.Validation
{
    public class CreateProductDtoValidator: AbstractValidator<CreateProductDto>
    {
        public CreateProductDtoValidator()
        {
            RuleFor(x => x.CompanyId)
            .NotNull()
            .NotEqual(0)
            .WithMessage("Company Id must be informed");

            RuleFor(x => x.Name)
            .NotNull()
            .WithMessage("Product name must be informed");
        }
    }
}
