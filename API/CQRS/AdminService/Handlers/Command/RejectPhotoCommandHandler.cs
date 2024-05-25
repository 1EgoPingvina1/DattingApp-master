using API.CQRS.AdminService.Requests.Command;
using API.Entities;
using API.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.CQRS.AdminService.Handlers.Command
{
    public class RejectPhotoCommandHandler : IRequestHandler<RejectPhotoCommand, Unit>
    {
        private readonly IPhotoService _photoService;
        private readonly IUnitOfWork _unit;
        public RejectPhotoCommandHandler(IUnitOfWork unit,IPhotoService photoService) 
        {
            _unit = unit;
            _photoService = photoService;
        }

        public async Task<Unit> Handle(RejectPhotoCommand request, CancellationToken cancellationToken)
        {
            var photo = await _unit.PhotoRepository.GetPhotoById(request.PhotoId);
            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Result == "ok")
                    _unit.PhotoRepository.RemovePhoto(photo);
            }
            else
                _unit.PhotoRepository.RemovePhoto(photo);

            await _unit.Complete();

            return Unit.Value;

        }
    }
}
