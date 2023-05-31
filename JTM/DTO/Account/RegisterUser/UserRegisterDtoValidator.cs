using FluentValidation;
using JTM.Data;
using Microsoft.EntityFrameworkCore;

namespace JTM.DTO.Account.RegisterUser
{
    public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
    {
        public RegisterUserDtoValidator() 
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Your name cannot be empty.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password cannot be empty.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
                .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("It is not an email address");

            RuleFor(x => x.EmailConfirmation)
                .Equal(x => x.Email).WithMessage("Confirmation email must be the same as email")
                .EmailAddress().WithMessage("It is not an email address");

            RuleFor(x => x.PasswordConfirmation)
                .Equal(x => x.Password).WithMessage("Confirmation password must be the same as password");
        }
    }
}
