using JTM.Data.UnitOfWork;
using MediatR;

namespace JTM.CQRS.Command.Company.AddCompany
{
    public class AddCompanyCommandHandler : IRequestHandler<AddCompanyCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddCompanyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(AddCompanyCommand request, CancellationToken cancellationToken)
        {
            Data.Model.Company company = new()
            {
                Name = request.Name
            };

            await _unitOfWork.CompanyRepository.AddAsync(company);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
