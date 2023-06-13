using FluentValidation;

namespace JTM.DTO.Company.UpdateCompany
{
    public class UpdateCompanyDtoValidator : AbstractValidator<UpdateCompanyDto>
    {
        public UpdateCompanyDtoValidator() 
        {
            RuleFor(x => x.Name)
                .NotNull().WithMessage("Company name cannot be null");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Company name cannot be empty");

            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Company id has to be greater than 0");
        }
    }
}
