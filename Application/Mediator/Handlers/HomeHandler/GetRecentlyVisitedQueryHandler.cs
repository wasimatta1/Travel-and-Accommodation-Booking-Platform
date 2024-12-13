using Application.Mediator.Queries.HomeQueries;
using AutoMapper;
using Contracts.DTOs.Home;
using Contracts.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Application.Mediator.Handlers.HomeHandler
{
    public class GetRecentlyVisitedQueryHandler : IRequestHandler<GetRecentlyVisitedQuery, IEnumerable<RecentlyVisitedResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetRecentlyVisitedQueryHandler> _logger;
        private readonly UserManager<User> _userManger;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public GetRecentlyVisitedQueryHandler(IUnitOfWork unitOfWork, ILogger<GetRecentlyVisitedQueryHandler> logger,
            IMapper mapper, UserManager<User> userManger, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _userManger = userManger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<RecentlyVisitedResponse>> Handle(GetRecentlyVisitedQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetRecentBookingsQueryHandler.Handle called");

            var email = _httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;
            var user = await _userManger.FindByEmailAsync(email);

            var bookings = await _unitOfWork.Bookings.GetBookingsByUserIdAsync(user!.Id, request.Take);

            _logger.LogInformation("GetRecentBookingsQueryHandler.Handle finished");
            return _mapper.Map<IEnumerable<RecentlyVisitedResponse>>(bookings);
        }
    }
}
