using MediatR;

namespace JTM.CQRS.Command.Company.DeleteCompany
{
    public sealed record DeleteCompanyCommand : IRequest
    {
        public int Id { get; init; }

        public DeleteCompanyCommand(int id)
        {
            Id = id;    
        }
    }
}
