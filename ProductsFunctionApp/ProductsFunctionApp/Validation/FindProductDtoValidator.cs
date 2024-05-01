using FluentValidation;
using ProductsFunctionApp.Dto;

namespace ProductsFunctionApp.Validation
{
    public class FindProductDtoValidator: AbstractValidator<FindProductDto>
    {
        public FindProductDtoValidator() 
        {
            RuleFor(x => x.CompanyId)
            .NotNull()
            .NotEqual(0)
            .WithMessage("Company Id must be informed");

            RuleFor(x => x.Id)
            .NotNull()
            .NotEqual(0)
            .WithMessage("Id must be informed");
        }
    }
}
