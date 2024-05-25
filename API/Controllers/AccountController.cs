using API.CQRS.AccountService.Command;
using API.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    public class AccountController : BaseAPIController
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator) => _mediator = mediator;

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(RegistereDTO registerDto)
            => await _mediator.Send(new RegisterCommand{ registereDTO = registerDto });

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDto)
            => await _mediator.Send(new LoginCommand { loginDTO = loginDto });   
    }
}