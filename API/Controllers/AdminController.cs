using API.CQRS.AdminService.Requests.Command;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AdminController : BaseAPIController
    {
        private readonly IMediator _mediator;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhotoService _photoService;

        public AdminController(IMediator mediator, UserManager<AppUser> userManager, IUnitOfWork unitOfWork, IPhotoService photoService)
        {
            _mediator = mediator;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _photoService = photoService;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult> GetUserWithRoles()
        {
            var users = await _userManager.Users
                .OrderBy(u => u.UserName)
                .Select(u => new
                {
                    u.Id,
                    u.UserName,
                    Roles = u.UserRoles.Select(r => r.Role.Name).ToList()
                }).ToListAsync();

            return Ok(users);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("edit-roles/{username}")]
        public async Task<IEnumerable<string>> EditRoles(string username, [FromQuery] string roles)
        {
            return await _mediator.Send(new EditRolesCommand { Username = username, UserRoles = roles });
        }


        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("photos-to-moderate")]
        public async Task<ActionResult> GetPhotosForModeration()
        {
            return Ok(await _unitOfWork.PhotoRepository.GetUnapprovedPhotos());
        }


        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("approve-photo/{photoId}")]
        public async Task<PhotoDTO> ApprovePhoto(int photoId)
        {
            return await _mediator.Send(new ApprovePhotoCommand { Id = photoId });

        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("reject-photo/{photoId}")]
        public async Task<ActionResult> RejectPhoto(int photoId)
        {
            var photo = await _unitOfWork.PhotoRepository.GetPhotoById(photoId);
            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Result == "ok")
                    _unitOfWork.PhotoRepository.RemovePhoto(photo);
            }
            else
                _unitOfWork.PhotoRepository.RemovePhoto(photo);

            await _unitOfWork.Complete();
            return Ok();
        }
    }
}
