namespace HotelApp.Services.Service_Interfaces
{
    public interface IBookingServiceManager
    {
        void StartBooking();
        void CancelBooking();
        void ViewBookings();
        void UpdateBooking();
    }
}