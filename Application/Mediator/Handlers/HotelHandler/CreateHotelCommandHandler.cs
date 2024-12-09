using Application.Mediator.Commands.HotelCommands;
using AutoMapper;
using Contracts.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Mediator.Handlers.HotelHandler
{
    public class CreateHotelCommandHandler : IRequestHandler<CreateHotelCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateHotelCommandHandler> _logger;
        private readonly UserManager<User> _userManger;


        public CreateHotelCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateHotelCommandHandler> logger, UserManager<User> userManger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _userManger = userManger;
        }

        public async Task<int> Handle(CreateHotelCommand request, CancellationToken cancellationToken)
        {

            _logger.LogInformation($"Handling CreateHotelCommand for Hotel: {request.CreateHotelDto.Name}");

            var city = await _unitOfWork.Cities.GetByIdAsync(request.CreateHotelDto.CityID);
            if (city == null)
            {
                _logger.LogWarning($"City with ID: {request.CreateHotelDto.CityID} was not found");
                return -1;
            }
            var user = await _userManger.FindByIdAsync(request.CreateHotelDto.OwnerID);
            if (user == null)
            {
                _logger.LogWarning($"User with ID: {request.CreateHotelDto.OwnerID} was not found");
                return -1;
            }


            var hotel = _mapper.Map<Hotel>(request.CreateHotelDto);
            await _unitOfWork.Hotels.CreateAsync(hotel);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation($"Hotel: {request.CreateHotelDto.Name} created successfully");
            return hotel.HotelID;

        }
    }
}
