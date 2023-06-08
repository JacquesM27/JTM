using JTM.Data;
using JTM.Data.Model;
using JTM.Data.Repository;
using JTM.Data.UnitOfWork;
using JTM.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlTypes;

namespace JTM.CQRS.Command.Account
{
    public sealed class BanUserCommandHandler : IRequestHandler<BanUserCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        //private readonly DataContext _dataContext;

        public BanUserCommandHandler(IUnitOfWork unitOfWork)
        {
            //_dataContext = dataContext;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(BanUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId)
                 ?? throw new AuthException("User not found.");
                //.Users
                // .SingleOrDefaultAsync(u => u.Id == request.UserId, cancellationToken: cancellationToken)

            user.Banned = true;
            user.TokenExpires = (DateTime)SqlDateTime.MinValue;
            //await _dataContext.SaveChangesAsync(cancellationToken);
            await _unitOfWork.UserRepository.UpdateAsync(user.Id, user);
            await _unitOfWork.SaveChanges();
        }
    }
}
