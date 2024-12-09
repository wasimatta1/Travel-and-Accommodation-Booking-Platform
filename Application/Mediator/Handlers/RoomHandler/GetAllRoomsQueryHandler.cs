using Application.Mediator.Queries.RoomQueries;
using AutoMapper;
using Contracts.DTOs.Room;
using Contracts.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Application.Mediator.Handlers.RoomHandler
{
    public class GetAllRoomsQueryHandler : IRequestHandler<GetAllRoomsQuery, IEnumerable<RoomDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllRoomsQueryHandler> _logger;

        public GetAllRoomsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAllRoomsQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<RoomDto>> Handle(GetAllRoomsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetAllRoomsQuery");

            var includes = new[]
            {
                "Hotel",
                "RoomImages"
            };

            IEnumerable<Expression<Func<Room, bool>>> criteria = new List<Expression<Func<Room, bool>>>();
            if (!string.IsNullOrWhiteSpace(request.RoomNumber))
                criteria = criteria.Append(r => r.RoomNumber.Contains(request.RoomNumber));
            if (!string.IsNullOrWhiteSpace(request.Type))
                criteria = criteria.Append(r => r.RoomType == Enum.Parse<RoomType>(request.Type));
            if (request.AdultCapacity.HasValue)
                criteria = criteria.Append(r => r.AdultsCapacity == request.AdultCapacity);
            if (request.ChildrenCapacity.HasValue)
                criteria = criteria.Append(r => r.ChildrenCapacity == request.ChildrenCapacity);
            if (request.Availability.HasValue)
                criteria = criteria.Append(r => r.Availability == request.Availability);

            var rooms = await _unitOfWork.Rooms.FindAllAsync(criteria, request.PagNumber, request.PageSize, includes);

            var roomDtos = _mapper.Map<IEnumerable<RoomDto>>(rooms);

            return roomDtos;

        }
    }
}
