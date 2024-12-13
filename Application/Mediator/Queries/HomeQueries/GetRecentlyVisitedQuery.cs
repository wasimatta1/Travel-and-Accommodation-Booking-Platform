using Contracts.DTOs.Home;
using MediatR;

namespace Application.Mediator.Queries.HomeQueries
{
    public class GetRecentlyVisitedQuery : IRequest<IEnumerable<RecentlyVisitedResponse>>
    {
        public int Take { get; set; }
    }
}
