using Application.Mediator.Queries.HotelQueries;
using AutoMapper;
using Contracts.DTOs.Hotel;
using Contracts.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Application.Mediator.Handlers.HotelHandler
{
    public class GetAllHotelsQueryHandler : IRequestHandler<GetAllHotelsQuery, IEnumerable<HotelDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllHotelsQueryHandler> _logger;

        public GetAllHotelsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAllHotelsQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<HotelDto>> Handle(GetAllHotelsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetAllHotelsQuery");

            // include owner table and city table
            var includes = new[]
            {
                "Owner",
                "City",
                "Rooms"
            };

            IEnumerable<Expression<Func<Hotel, bool>>> criteria = new List<Expression<Func<Hotel, bool>>>();
            if (!string.IsNullOrWhiteSpace(request.HotelName))
                criteria = criteria.Append(h => h.Name.Contains(request.HotelName));

            if (!string.IsNullOrWhiteSpace(request.City))
                criteria = criteria.Append(h => h.City.Name.Contains(request.City));

            if (!string.IsNullOrWhiteSpace(request.Owner))
                criteria = criteria.Append(h => h.Owner.FirstName.Contains(request.Owner) || h.Owner.LastName.Contains(request.Owner));

            if (request.StarRating.HasValue)
                criteria = criteria.Append(h => h.StarRating == request.StarRating);

            var hotels = await _unitOfWork.Hotels.FindAllAsync(criteria, request.PagNumber, request.PageSize, includes);


            var hotelDtos = _mapper.Map<IEnumerable<HotelDto>>(hotels);


            return hotelDtos;

        }
    }
}