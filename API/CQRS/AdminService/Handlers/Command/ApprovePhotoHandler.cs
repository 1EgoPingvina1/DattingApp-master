using API.CQRS.AdminService.Requests.Command;
using API.DTOs;
using API.Errors;
using API.Interfaces;
using AutoMapper;
using MediatR;

namespace API.CQRS.AdminService.Handlers.Command
{
    public class ApprovePhotoHandler : IRequestHandler<ApprovePhotoCommand, PhotoDTO>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ApprovePhotoHandler( IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<PhotoDTO> Handle(ApprovePhotoCommand request, CancellationToken cancellationToken)
        {
            var photo = await _unitOfWork.PhotoRepository.GetPhotoById(request.Id);
            if (photo == null)  throw new HttpException(404, "Could not find photo");

            photo.IsApproved = true;
            var user = await _unitOfWork.UserRepository.GetUserByPhotoId(request.Id);

            if (!user.Photos.Any(x => x.IsMain))
                photo.IsMain = true;

            if(!await _unitOfWork.Complete()) throw new HttpException(400, "Fail complete"); ;
            return _mapper.Map<PhotoDTO>(photo);
        }
    }
}
