namespace RestaurantRaterBooking.Services
{
    public interface IUnitOfWork
    {
        IPaypalServices PaypalServices { get; }
    }
}
