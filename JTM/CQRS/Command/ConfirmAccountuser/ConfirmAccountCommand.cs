﻿using JTM.DTO.Account;
using MediatR;

namespace JTM.CQRS.Command.ConfirmAccountuser
{
    public class ConfirmAccountCommand : IRequest<AuthResponseDto>
    {
        public int UserId { get; set; }
        public string? Token { get; set; }
    }
}
