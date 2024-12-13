using Application.Mediator.Queries.HomeQueries;
using Contracts.DTOs.Home;
using Contracts.Interfaces.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Mediator.Handlers.HomeHandler
{
    public class SearchRequestQureyHandler : IRequestHandler<SearchRequestQurey, IEnumerable<SearchResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SearchRequestQureyHandler> _logger;

        public SearchRequestQureyHandler(IUnitOfWork unitOfWork, ILogger<SearchRequestQureyHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<IEnumerable<SearchResponse>> Handle(SearchRequestQurey request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("SearchRequestQureyHandler.Handle called");

            var hotels = await _unitOfWork.Hotels.SearchHotels(request.Query, request.StarRating,
                request.PageNumber, request.PageSize, request.Amenities);

            var rooms = hotels.SelectMany(h => h.Rooms).Where(r => r.Availability == true
            && r.AdultsCapacity >= request.Adults && r.ChildrenCapacity >= request.Children
            && r.bookings.OrderBy(b => b.CheckInDate).All(b =>
                // Check if booking ends before requested check-in or starts after requested check-out
                (b.CheckOutDate <= request.CheckInDate || b.CheckInDate >= request.CheckOutDate) ||

                // Check for gaps between consecutive bookings
                r.bookings.Zip(r.bookings.Skip(1), (current, next) =>
                    next.CheckInDate >= current.CheckOutDate && next.CheckInDate - current.CheckOutDate >= request.CheckOutDate - request.CheckInDate
                ).All(gap => gap)));

            if (request.PriceMin != null)
                rooms = rooms.Where(r => r.PricePerNight >= request.PriceMin);
            if (request.PriceMax != null)
                rooms = rooms.Where(r => r.PricePerNight <= request.PriceMax);

            if (request.RoomType != null)
                rooms = rooms.Where(r => r.RoomType.ToString() == request.RoomType);

            var groupedRooms = rooms.GroupBy(r => r.HotelID).Where(g => g.Count() >= request.Rooms);

            rooms = groupedRooms.SelectMany(g => g.Select(room => room));

            var discounteds = await _unitOfWork.Discounts.GetDiscountsByRoomIdAsync(rooms.Select(r => r.RoomID).ToList());

            hotels = hotels.Where(h => groupedRooms.Select(g => g.Key).Contains(h.HotelID));

            var discountedRoomIds = new HashSet<int>(discounteds.Select(d => d.RoomId));

            var hotelSearchResponses = new Dictionary<int, SearchResponse>();

            foreach (var hotel in hotels)
            {
                var firstRoomWithDiscount = rooms.FirstOrDefault(r => r.HotelID == hotel.HotelID && discountedRoomIds.Contains(r.RoomID));
                var firstRoomWithOutDiscount = rooms.FirstOrDefault(r => r.HotelID == hotel.HotelID);

                decimal? pricePerNight = null;
                decimal? pricePerNightDiscounted = null;
                string roomType = "";

                if (firstRoomWithDiscount != null)
                {
                    var discount = discounteds.FirstOrDefault(d => d.RoomId == firstRoomWithDiscount.RoomID);
                    pricePerNight = firstRoomWithDiscount.PricePerNight;
                    pricePerNightDiscounted = pricePerNight * (1 - discount!.DiscountPercentage / 100);
                    roomType = firstRoomWithDiscount.RoomType.ToString();
                }
                else if (firstRoomWithOutDiscount != null)
                {
                    pricePerNight = firstRoomWithOutDiscount.PricePerNight;
                    roomType = firstRoomWithOutDiscount.RoomType.ToString();
                }

                hotelSearchResponses.Add(hotel.HotelID, new SearchResponse
                {
                    HotelName = hotel.Name,
                    ThumbnailUrl = hotel.ThumbnailURL,
                    StarRating = hotel.StarRating,
                    HotelDescription = hotel.Description,
                    PricePerNight = (decimal)pricePerNight!,
                    PricePerNightDiscounted = pricePerNightDiscounted,
                    RoomType = roomType
                });
            }

            return hotelSearchResponses.Values;
        }
    }
}
