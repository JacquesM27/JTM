using JTM.Data;
using JTM.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlTypes;

namespace JTM.CQRS.Command.Account
{
    public class BanUserCommandHandler : IRequestHandler<BanUserCommand>
    {
        private readonly DataContext _dataContext;

        public BanUserCommandHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task Handle(BanUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _dataContext.Users
                 .SingleOrDefaultAsync(u => u.Id == request.UserId, cancellationToken: cancellationToken)
                 ?? throw new AuthException("User not found.");

            user.Banned = true;
            user.TokenExpires = (DateTime)SqlDateTime.MinValue;
            await _dataContext.SaveChangesAsync(cancellationToken);
        }
    }
}
