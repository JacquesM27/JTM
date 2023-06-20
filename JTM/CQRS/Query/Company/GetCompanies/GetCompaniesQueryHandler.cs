using JTM.Data.UnitOfWork;
using JTM.DTO.Company;
using MediatR;

namespace JTM.CQRS.Query.Company
{
    public class GetCompaniesQueryHandler : IRequestHandler<GetCompaniesQuery, IEnumerable<CompanyDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCompaniesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CompanyDto>> Handle(GetCompaniesQuery request, CancellationToken cancellationToken)
        {
            var companies = await _unitOfWork.CompanyRepository.QueryAsync();
            var companiesDto = companies.Select(c => new CompanyDto() { Id = c.Id, Name = c.Name });
            return companiesDto;
        }
    }
}
