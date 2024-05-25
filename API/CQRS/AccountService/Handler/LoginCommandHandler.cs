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
    public class LoginCommandHandler : IRequestHandler<LoginCommand, ActionResult<UserDTO>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        public LoginCommandHandler(UserManager<AppUser> userManager,ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<ActionResult<UserDTO>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users
                .Include(x => x.Photos)
                .SingleOrDefaultAsync(x => x.UserName == request.loginDTO.UserName);

            if (user == null) throw new HttpException(401, "Invalid login or password");

            var result = await _userManager.CheckPasswordAsync(user, request.loginDTO.Password);

            if (!result) throw new HttpException(401, "Invalid password");

            return new UserDTO
            {
                UserName = user.UserName,
                Token = await _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain).Url,
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }
    }
}