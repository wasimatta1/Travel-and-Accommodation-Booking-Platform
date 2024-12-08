using Application.Mediator.Queries.CityQueries;
using AutoMapper;
using Contracts.DTOs.City;
using Contracts.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Application.Mediator.Handlers.CityHandler
{
    public class GetAllCitiesQueryHandler : IRequestHandler<GetAllCitiesQuery, IEnumerable<CityDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllCitiesQueryHandler> _logger;

        public GetAllCitiesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAllCitiesQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<CityDto>> Handle(GetAllCitiesQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetAllCitiesQuery");

            IEnumerable<Expression<Func<City, bool>>> criteria = new List<Expression<Func<City, bool>>>();

            if (!string.IsNullOrEmpty(request.CityName))
            {
                criteria = criteria.Append(c => c.Name == request.CityName);
            }
            if (!string.IsNullOrEmpty(request.Country))
            {
                criteria = criteria.Append(c => c.Country == request.Country);
            }
            if (!string.IsNullOrEmpty(request.PostOffice))
            {
                criteria = criteria.Append(c => c.PostOffice == request.PostOffice);
            }

            var cities = await _unitOfWork.Cities.FindAllAsync(criteria, request.PagNumber, request.PageSize);

            return _mapper.Map<IEnumerable<CityDto>>(cities);
        }
    }
}
