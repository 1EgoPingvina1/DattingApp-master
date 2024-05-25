using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.CQRS.AdminService.Requests.Command
{
    public class RejectPhotoCommand : IRequest<Unit>
    {
        public int PhotoId { get; set; }
    }
}
