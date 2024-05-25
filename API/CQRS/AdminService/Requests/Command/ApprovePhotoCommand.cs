using API.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.CQRS.AdminService.Requests.Command
{
    public class ApprovePhotoCommand : IRequest<PhotoDTO>
    {
        public int Id { get; set; }
    }
}
