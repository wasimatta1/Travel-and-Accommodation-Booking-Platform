using Application.Mediator.Commands.RoomCommands;
using AutoMapper;
using Contracts.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Mediator.Handlers.RoomHandler
{
    public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateRoomCommandHandler> _logger;

        public CreateRoomCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateRoomCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> Handle(CreateRoomCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling CreateRoomCommand for Room: {request.CreateRoomDto.RoomNumber}");

            var hotel = await _unitOfWork.Hotels.GetByIdAsync(request.CreateRoomDto.HotelID);
            if (hotel == null)
            {
                _logger.LogWarning($"Hotel with ID: {request.CreateRoomDto.HotelID} was not found");
                return -1;
            }

            var room = _mapper.Map<Room>(request.CreateRoomDto);
            await _unitOfWork.Rooms.CreateAsync(room);
            await _unitOfWork.CompleteAsync();

            var roomImages = new List<RoomImage>();

            foreach (var imageUrl in request.CreateRoomDto.ImagesUrl)
            {
                var roomImage = new RoomImage
                {
                    RoomID = room.RoomID,
                    ImageUrl = imageUrl
                };
                roomImages.Add(roomImage);
            }
            if (roomImages.Count > 0)
            {
                await _unitOfWork.RoomImages.AddRangeAsync(roomImages);
                await _unitOfWork.CompleteAsync();
            }

            _logger.LogInformation($"Room: {request.CreateRoomDto.RoomNumber} created successfully");
            return room.RoomID;
        }
    }
}
