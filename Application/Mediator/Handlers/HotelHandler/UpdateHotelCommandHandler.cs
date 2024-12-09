﻿using Application.Mediator.Commands.HotelCommands;
using AutoMapper;
using Contracts.DTOs.Hotel;
using Contracts.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Mediator.Handlers.HotelHandler
{

    public class UpdateHotelCommandHandler : IRequestHandler<UpdateHotelCommand, HotelDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateHotelCommandHandler> _logger;
        private readonly UserManager<User> _userManger;

        public UpdateHotelCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateHotelCommandHandler> logger, UserManager<User> userManger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _userManger = userManger;
        }

        public async Task<HotelDto?> Handle(UpdateHotelCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling UpdateHotelCommand for Hotel ID: {request.UpdateHotelDto.HotelID}");

            var hotel = await _unitOfWork.Hotels.GetByIdAsync(request.UpdateHotelDto.HotelID);
            if (hotel == null)
            {
                _logger.LogWarning($"Hotel with ID: {request.UpdateHotelDto.HotelID} was not found");
                return null;
            }
            var city = await _unitOfWork.Cities.GetByIdAsync(request.UpdateHotelDto.CityID);
            if (city == null)
            {
                _logger.LogWarning($"City with ID: {request.UpdateHotelDto.CityID} was not found");
                return null;
            }
            var user = await _userManger.FindByIdAsync(request.UpdateHotelDto.OwnerID);
            if (user == null)
            {
                _logger.LogWarning($"User with ID: {request.UpdateHotelDto.OwnerID} was not found");
                return null;
            }

            _mapper.Map(request.UpdateHotelDto, hotel);
            hotel.UpdatedAt = DateTime.Now;

            var updatedHotel = await _unitOfWork.Hotels.UpdateAsync(hotel);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation($"Hotel with ID: {hotel.HotelID} updated successfully.");

            return _mapper.Map<HotelDto>(hotel);
        }
    }
}