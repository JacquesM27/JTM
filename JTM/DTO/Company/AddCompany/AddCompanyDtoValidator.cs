using FluentValidation;

namespace JTM.DTO.Company.AddCompany
{
    public class AddCompanyDtoValidator : AbstractValidator<AddCompanyDto>
    {
        public AddCompanyDtoValidator() 
        {
            RuleFor(x => x.Name)
                .NotNull().WithMessage("Company name cannot be null");
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Company name cannot be empty");
        }
    }
}
