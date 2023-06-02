using JTM.Data;
using JTM.DTO.Account;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JTM.CQRS.Command.ConfirmAccountuser
{
    public class ConfirmAccountCommandHandler : IRequestHandler<ConfirmAccountCommand, AuthResponseDto>
    {
        private readonly DataContext _dataContext;

        public ConfirmAccountCommandHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<AuthResponseDto> Handle(ConfirmAccountCommand request, CancellationToken cancellationToken)
        {
            var user = await _dataContext.Users
                .SingleOrDefaultAsync(c => c.Id == request.UserId);
            if (user is null)
            {
                return new AuthResponseDto { Message = "Invalid user." };
            }
            else if (user.EmailConfirmed)
            {
                return new AuthResponseDto { Message = "User already confirmed." };
            }
            else if (user.ActivationTokenExpires < DateTime.UtcNow)
            {
                return new AuthResponseDto { Message = "Token expired." };
            }
            else if (!request.Token!.Equals(user.ActivationToken))
            {
                return new AuthResponseDto { Message = "Invalid token." };
            }

            user.EmailConfirmed = true;
            user.ActivationToken = null;
            await _dataContext.SaveChangesAsync();
            return new AuthResponseDto()
            {
                Success = true,
            };
        }
    }
}
