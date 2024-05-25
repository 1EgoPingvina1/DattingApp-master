using API.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.CQRS.AccountService.Command
{
    public class LoginCommand : IRequest<ActionResult<UserDTO>>
    {
        public LoginDTO loginDTO {  get; set; }
    }
}
