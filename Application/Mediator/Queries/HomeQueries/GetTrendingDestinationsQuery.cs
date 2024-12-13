using Contracts.DTOs.Home;
using MediatR;

namespace Application.Mediator.Queries.HomeQueries
{
    public class GetTrendingDestinationsQuery : IRequest<IEnumerable<TrendingDestinationDto>>
    {
        public int Take { get; set; }
    }
}
