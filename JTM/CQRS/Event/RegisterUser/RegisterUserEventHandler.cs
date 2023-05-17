using MediatR;

namespace JTM.CQRS.Event.RegisterUser
{
    public class RegisterUserEventHandler : INotificationHandler<RegisterUserEvent>
    {
        public Task Handle(RegisterUserEvent notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
