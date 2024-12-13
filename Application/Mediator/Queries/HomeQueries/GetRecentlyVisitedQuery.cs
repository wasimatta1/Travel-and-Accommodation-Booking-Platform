using Contracts.DTOs.Home;
using MediatR;

namespace Application.Mediator.Queries.HomeQueries
{
    public class GetRecentlyVisitedQuery : IRequest<IEnumerable<RecentlyVisitedDto>>
    {
        public int Take { get; set; }
    }
}
