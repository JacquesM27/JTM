﻿using MediatR;

namespace JTM.CQRS.Command.Account
{
    public sealed record BanUserCommand : IRequest<int>
    {
        public int UserId { get; init; }

        public BanUserCommand(int userId)
        {
            UserId = userId;
        }
    }
}
