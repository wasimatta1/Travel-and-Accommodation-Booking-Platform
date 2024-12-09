using Application.Mediator.Commands.RoomCommands;
using AutoMapper;
using Contracts.DTOs.Room;
using Contracts.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Mediator.Handlers.RoomHandler
{
    public class UpdateRoomCommandHnadler : IRequestHandler<UpdateRoomCommand, RoomDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateRoomCommandHnadler> _logger;

        public UpdateRoomCommandHnadler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateRoomCommandHnadler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<RoomDto> Handle(UpdateRoomCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling UpdateRoomCommand for Room ID: {request.UpdateRoomDto.RoomID}");
            var includes = new[] {

                "RoomImages"
            };

            var room = await _unitOfWork.Rooms.FindAsync(c => c.RoomID == request.UpdateRoomDto.RoomID, includes);

            if (room == null)
            {
                _logger.LogWarning($"Room with ID: {request.UpdateRoomDto.RoomID} was not found");
                return null;
            }
            _mapper.Map(request.UpdateRoomDto, room);
            room.UpdatedAt = DateTime.Now;

            await _unitOfWork.Rooms.UpdateAsync(room);

            await _unitOfWork.CompleteAsync();

            await ManageRoomImagesAsync(room, request.UpdateRoomDto.ImagesUrl);

            _logger.LogInformation($"Room with ID: {room.RoomID} updated successfully.");

            return _mapper.Map<RoomDto>(room);
        }



        private async Task ManageRoomImagesAsync(Room room, ICollection<string> imagesUrl)
        {
            _logger.LogInformation($"Updating Room Images for Room ID: {room.RoomID}");

            // Easy Way Remove All Images and Add New Ones
            await _unitOfWork.RoomImages.DeleteRangeAsync(room.RoomImages);

            if (imagesUrl == null || imagesUrl.Count == 0)
            {
                await _unitOfWork.CompleteAsync();

                _logger.LogInformation($"Room Images for Room ID: {room.RoomID} updated successfully.");

                return;
            }

            var roomImages = new List<RoomImage>();

            foreach (var imageUrl in imagesUrl)
            {
                var roomImage = new RoomImage
                {
                    RoomID = room.RoomID,
                    ImageUrl = imageUrl
                };
                roomImages.Add(roomImage);
            }

            await _unitOfWork.RoomImages.AddRangeAsync(roomImages);

            await _unitOfWork.CompleteAsync();

            _logger.LogInformation($"Room Images for Room ID: {room.RoomID} updated successfully.");
        }
    }
}
