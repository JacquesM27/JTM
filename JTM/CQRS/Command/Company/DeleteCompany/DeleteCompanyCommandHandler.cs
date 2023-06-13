using JTM.Data.UnitOfWork;
using JTM.Exceptions;
using MediatR;

namespace JTM.CQRS.Command.Company.DeleteCompany
{
    public class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteCompanyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _unitOfWork.CompanyRepository.GetByIdAsync(request.Id)
                ?? throw new NotFoundException($"Company with id:{request.Id} does not exists");

            await _unitOfWork.CompanyRepository.RemoveAsync(company);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
