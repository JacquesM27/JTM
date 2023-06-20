using JTM.Data.UnitOfWork;
using JTM.DTO.Company;
using JTM.Exceptions;
using MediatR;
using System.Linq.Expressions;

namespace JTM.CQRS.Query.Company
{
    public class GetCompanyQueryHandler : IRequestHandler<GetCompanyQuery, CompanyDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCompanyQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CompanyDto> Handle(GetCompanyQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Data.Model.Company, bool>> filter = c => c.Id == request.CompanyId;

            var company = await _unitOfWork.CompanyRepository.QuerySingleAsync(filter)
                ?? throw new CompanyException($"Company with id: {request.CompanyId} does not exist.");

            return new CompanyDto()
            {
                Id = company.Id,
                Name = company.Name
            };
        }
    }
}
