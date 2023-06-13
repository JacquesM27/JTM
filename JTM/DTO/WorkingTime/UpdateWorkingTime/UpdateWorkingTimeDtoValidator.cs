using FluentValidation;

namespace JTM.DTO.WorkingTime.UpdateWorkingTime
{
    public class UpdateWorkingTimeDtoValidator : AbstractValidator<UpdateWorkingTimeDto>
    {
        public UpdateWorkingTimeDtoValidator() 
        {
            RuleFor(x => x.Seconds)
                .GreaterThan(-1).WithMessage("Second field has to be greater than -1");
        }
    }
}
