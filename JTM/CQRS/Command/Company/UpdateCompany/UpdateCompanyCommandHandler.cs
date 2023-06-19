using JTM.Data.UnitOfWork;
using JTM.Exceptions;
using MediatR;

namespace JTM.CQRS.Command.Company.UpdateCompany
{
    public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCompanyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            if (request.HeaderId != request.RouteId)
                throw new CompanyException($"Id from header({request.HeaderId}) does not equal id from route({request.RouteId}).");

            var company = await _unitOfWork.CompanyRepository.GetByIdAsync(request.HeaderId )
                ?? throw new CompanyException($"Company with Id:{request.HeaderId} does not exist."); 

            company.Name = request.Name;
            await _unitOfWork.CompanyRepository.UpdateAsync(request.HeaderId ,company);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
