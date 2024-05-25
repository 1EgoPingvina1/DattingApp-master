using MediatR;

namespace API.CQRS.AdminService.Requests.Command
{
    public class EditRolesCommand : IRequest<IEnumerable<string>>
    {
        public string Username { get; set; }

        public string UserRoles { get; set; }
    }
}
