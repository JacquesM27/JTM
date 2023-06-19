using FluentValidation;
using FluentValidation.Results;
using JTM.Data.UnitOfWork;
using MediatR;
using System.Linq.Expressions;

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
            await CheckCompanyNameUnique(request.Name);

            Data.Model.Company company = new()
            {
                Name = request.Name
            };

            await _unitOfWork.CompanyRepository.AddAsync(company);
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task CheckCompanyNameUnique(string name)
        {
            Expression<Func<Data.Model.Company,bool>> filter = company => company.Name == name;
            var company = await _unitOfWork.CompanyRepository.QuerySingleAsync(filter);
            if(company is not null)
            {
                throw new ValidationException("Company name is busy.",
                     new List<ValidationFailure>
                     {
                        new ValidationFailure()
                        {
                            PropertyName = "Name", ErrorMessage = "Company name is busy."
                        }
                     });
            }
        }
    }
}
