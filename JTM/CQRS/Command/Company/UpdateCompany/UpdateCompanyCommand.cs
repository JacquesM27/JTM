using MediatR;

namespace JTM.CQRS.Command.Company.UpdateCompany
{
    public sealed record UpdateCompanyCommand : IRequest
    {
        public int HeaderId { get; init; }
        public int RouteId { get; init; }
        public string Name { get; init; }

        public UpdateCompanyCommand(string name, int headerId, int routeId)
        {
            Name = name;
            HeaderId = headerId;
            RouteId = routeId;
        }
    }
}
