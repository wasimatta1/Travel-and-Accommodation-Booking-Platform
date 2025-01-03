# Travel and Accommodation Booking Platform

## Project Overview

This project is a comprehensive solution designed to streamline the online booking process for hotels and other accommodations. The platform allows users to search for hotels, view detailed information, select rooms, and securely complete checkouts. Additionally, administrators have an easy-to-use interface for managing hotels, rooms, and bookings. The system is built using the **ASP.NET Core** framework and follows modern architectural principles such as **Clean Architecture** and **Domain-Driven Design (DDD)**.

---

## Key Features

### 1. **Login Page**
- **Login** functionality using secure JWT tokens for user authentication.
- Users can register, log in, and log out of the platform.
- Role-based access control (RBAC) for different user permissions.

### 2. **Home Page**
- **Robust Search Functionality**:
  - Search bar with the placeholder: "Search for hotels, cities..."
  - Interactive calendar for selecting check-in and check-out dates.
  - Adjustable controls for selecting the number of adults and children.
  - Room selection options with a default setting of one room.

- **Featured Deals Section**:
  - Highlighting special offers and discounts.
  - Displays 3-5 hotels with thumbnails, hotel names, locations, and original/discounted prices.
  - Star ratings for each featured hotel.

- **User's Recently Visited Hotels**:
  - Displays the last 3-5 hotels the user visited.
  - Information like hotel name, city, star rating, and pricing details.

- **Trending Destination Highlights**:
  - Top 5 cities that have been visited the most on the platform.
  - Each city is displayed with a thumbnail and name.

### 3. **Search Results Page**
- **Comprehensive Search Filters**:
  - Sidebar filters including price range, star rating, and amenities.
  - Filters for different room types: luxury, budget, boutique hotels.
  
- **Hotel Listings**:
  - Displays a list of hotels matching the search criteria with an infinite scroll feature.
  - Each hotel listing includes a thumbnail, name, star rating, price per night, and brief description.

### 4. **Hotel Page**
- **Visual Gallery**:
  - High-quality images of the hotel, with the ability to view them in fullscreen mode.

- **Detailed Hotel Information**:
  - Hotel name, star rating, description/history, and guest reviews.
  - Interactive map showing the hotel’s location with nearby attractions.

- **Room Availability and Selection**:
  - List of available rooms with images, descriptions, and prices.
  - “Add to cart” option for booking.

### 5. **Secure Checkout and Confirmation**
- **User Information and Payment**:
  - Form for entering user details and payment information.
  - Option for special requests or remarks during booking.
  - Optional third-party payment gateway integration.

- **Confirmation Page**:
  - Displays booking details including confirmation number, hotel address, room details, dates, and total price.
  - Options to print or save the booking confirmation as a PDF.
  - Email notifications with booking confirmation and invoice details.

### 6. **Admin Page for Easy Management**
- **Functional Left Navigation**:
  - Collapsible navigation with links to Cities, Hotels, and Rooms.

- **Admin Search Bar**:
  - Search functionality for entities in grids.

- **Detailed Grids**:
  - **Cities**: Displays city name, country, postal code, number of hotels, creation/modification dates, and delete option.
  - **Hotels**: Displays hotel name, star rating, owner, number of rooms, creation/modification dates, and delete option.
  - **Rooms**: Displays room number, availability, adult/child capacity, creation/modification dates, and delete option.

- **Create and Update Entities**:
  - **Create Button**: Opens a form for creating new Cities, Hotels, or Rooms.
  - **Entity Update Form**: Accessible by clicking on any grid row for editing existing cities, hotels, or rooms.



