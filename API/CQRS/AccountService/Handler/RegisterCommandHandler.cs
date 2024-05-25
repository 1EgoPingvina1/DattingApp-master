using API.CQRS.AccountService.Command;
using API.DTOs;
using API.Entities;
using API.Errors;
using API.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.CQRS.AccountService.Handler
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ActionResult<UserDTO>>
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public RegisterCommandHandler(UserManager<AppUser> userManager,
                                    ITokenService tokenService,
                                    IMapper mapper)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<ActionResult<UserDTO>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {

            if (await UserExists(request.registereDTO.Username)) throw new HttpException(500, "User already exists");
            var user = _mapper.Map<AppUser>(request.registereDTO);

            user.UserName = request.registereDTO.Username.ToLower();

            var result = await _userManager.CreateAsync(user, request.registereDTO.Password);

            if (!result.Succeeded) throw new HttpException(500, result.Errors.ToString());

            var roleResult = await _userManager.AddToRoleAsync(user, "Member");

            if (!roleResult.Succeeded) throw new HttpException(500, result.Errors.ToString());

            return new UserDTO
            {
                UserName = user.UserName,
                Token = await _tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }

        private async Task<bool> UserExists(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }

    }
}
