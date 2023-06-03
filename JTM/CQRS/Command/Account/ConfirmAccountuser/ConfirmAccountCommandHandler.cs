using JTM.Data;
using JTM.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JTM.CQRS.Command.Account
{
    public class ConfirmAccountCommandHandler : IRequestHandler<ConfirmAccountCommand>
    {
        private readonly DataContext _dataContext;

        public ConfirmAccountCommandHandler(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task Handle(ConfirmAccountCommand request, CancellationToken cancellationToken)
        {
            var user = await _dataContext.Users
                .SingleOrDefaultAsync(c => c.Id == request.UserId, cancellationToken);

            if (user is null)
                throw new AuthException("Invalid user.");
            else if (user.EmailConfirmed)
                throw new AuthException("User already confirmed.");
            else if (user.ActivationTokenExpires < DateTime.UtcNow)
                throw new AuthException("Token expired.");
            else if (!request.Token!.Equals(user.ActivationToken))
                throw new AuthException("Invalid token.");

            user.EmailConfirmed = true;
            user.ActivationToken = null;
            await _dataContext.SaveChangesAsync(cancellationToken);
        }
    }
}
