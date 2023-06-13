using JTM.DTO.Company;
using MediatR;

namespace JTM.CQRS.Query.Company
{
    public sealed record GetCompaniesQuery : IRequest<IEnumerable<CompanyDto>>
    {
    }
}
