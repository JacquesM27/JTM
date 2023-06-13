using MediatR;

namespace JTM.CQRS.Command.Company.AddCompany
{
    public sealed record AddCompanyCommand : IRequest
    {
        public string Name { get; init; }

        public AddCompanyCommand(string name)
        {
            Name = name;
        }
    }
}
