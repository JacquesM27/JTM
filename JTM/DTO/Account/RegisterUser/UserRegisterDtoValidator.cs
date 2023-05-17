using Dapper;
using FluentValidation;
using JTM.Data.DapperConnection;

namespace JTM.DTO.Account.RegisterUser
{
    public class RegisterDtoValidator : AbstractValidator<RegisterUserDto>
    {
        private readonly IDapperConnectionFactory _connectionFactory;
        public RegisterDtoValidator(IDapperConnectionFactory connectionFactory) 
        {
            _connectionFactory = connectionFactory;

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
            .Custom(async (value, context) =>
            {
                var emailInUse = await IsEmailInUse(value);
                if (emailInUse)
                {
                    context.AddFailure("Email", "Email is already taken");
                }
            });

            RuleFor(x => x.EmailConfirmation)
                .Equal(x => x.Email).WithMessage("Confirmation email must be the same as email");

            RuleFor(x => x.PasswordConfirmation)
                .Equal(x => x.Email).WithMessage("Confirmation password must be the same as password");
        }

        private async Task<bool> IsEmailInUse(string email)
        {
            using var connection = _connectionFactory.DbConnection;
            string sql = "SELECT COUNT(1) FROM dbo.Users WHERE Email = @email";
            int count = await connection.QuerySingleAsync<int>(sql, new { email });
            return count > 0;
        }
    }
}
