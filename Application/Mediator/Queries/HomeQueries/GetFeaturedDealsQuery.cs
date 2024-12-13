using Contracts.DTOs.Home;
using MediatR;

namespace Application.Mediator.Queries.HomeQueries
{
    public class GetFeaturedDealsQuery : IRequest<IEnumerable<FeaturedDealResponse>>
    {
        public int Take { get; set; }
    }
}
