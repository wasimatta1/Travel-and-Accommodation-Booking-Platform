using Contracts.DTOs.HotelPage;
using MediatR;

namespace Application.Mediator.Queries.HotelPageQueries
{
    public class GetCartItemsQuery : IRequest<IEnumerable<CartItemDto>?>
    {
    }
}
