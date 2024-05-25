using API.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.CQRS.AccountService.Command
{
    public class RegisterCommand : IRequest<ActionResult<UserDTO>>
    {
        public RegistereDTO registereDTO { get; set; }
    }
}
