using FluentValidation;

namespace JTM.DTO.WorkingTime.AddWorkingTime
{
    public class AddWorkingTimeDtoValidator : AbstractValidator<AddWorkingTimeDto>
    {
        public AddWorkingTimeDtoValidator()
        {
            RuleFor(c => c.Seconds)
                .GreaterThan(-1).WithMessage("Second field has to be greater than -1");
        }
    }
}
