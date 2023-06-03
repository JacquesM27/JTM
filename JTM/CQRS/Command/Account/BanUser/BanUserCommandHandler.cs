using JTM.Data;
using JTM.DTO.Account;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlTypes;

namespace JTM.CQRS.Command.Account.BanUser
{
    public class BanUserCommandHandler : IRequestHandler<BanUserCommand, AuthResponseDto>
    {
        private readonly DataContext _dataContext;

        public BanUserCommandHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<AuthResponseDto> Handle(BanUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _dataContext.Users
                 .SingleOrDefaultAsync(u => u.Id == request.UserId, cancellationToken: cancellationToken);

            if (user is null)
            {
                return new AuthResponseDto { Message = "User not found." };
            }

            user.Banned = true;
            user.TokenExpires = (DateTime)SqlDateTime.MinValue;
            await _dataContext.SaveChangesAsync(cancellationToken);
            return new AuthResponseDto { Success = true };
        }
    }
}
