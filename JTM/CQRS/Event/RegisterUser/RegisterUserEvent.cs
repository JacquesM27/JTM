using MediatR;

namespace JTM.CQRS.Event.RegisterUser
{
    public class RegisterUserEvent : INotification
    {
        public int UserId { get; }

        public RegisterUserEvent(int userId) => UserId = userId;
    }
}
