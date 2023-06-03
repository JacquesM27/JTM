﻿using MediatR;

namespace JTM.CQRS.Command.Account
{
    public record BanUserCommand : IRequest
    {
        public int UserId { get; init; }

        public BanUserCommand(int userId)
        {
            UserId = userId;
        }
    }
}
