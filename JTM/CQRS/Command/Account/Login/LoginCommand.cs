﻿using JTM.DTO.Account;
using MediatR;

namespace JTM.CQRS.Command.Account
{
    public sealed record LoginCommand : IRequest<AuthResponseDto>
    {
        public string Email { get; init; }
        public string Password { get; init; }

        public LoginCommand(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
