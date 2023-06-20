using JTM.DTO.Company;
using MediatR;

namespace JTM.CQRS.Query.Company
{
    public sealed record GetCompanyQuery : IRequest<CompanyDto>
    {
        public int CompanyId { get; init; }

        public GetCompanyQuery(int companyId)
        {
            CompanyId = companyId;
        }
    }
}
