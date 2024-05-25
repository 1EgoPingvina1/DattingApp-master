using API.CQRS.AdminService.Requests.Command;
using API.Entities;
using API.Errors;
using API.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.CQRS.AdminService.Handlers.Command
{
    public class EditRolesHandler : IRequestHandler<EditRolesCommand, IEnumerable<string>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhotoService _photoService;

        public EditRolesHandler(UserManager<AppUser> userManager, IUnitOfWork unitOfWork, IPhotoService photoService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _photoService = photoService;
        }


        public async Task<IEnumerable<string>> Handle(EditRolesCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.UserRoles)) throw new HttpException(500,"Most choose roles");

            var selectedRoles = request.UserRoles.Split(",").ToArray();

            var user = await _userManager.FindByNameAsync(request.Username);

            if (user == null) throw new HttpException(404,"User not found");

            var userRoles = await _userManager.GetRolesAsync(user);

            var result = await _userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

            if (!result.Succeeded) throw new HttpException(500, "Roles was not appoint");

            result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(userRoles));

            if (!result.Succeeded) throw new HttpException(500, "Role was not deleted");

            return await _userManager.GetRolesAsync(user);
        }
    }
}
