using PayPal.Api;
namespace RestaurantRaterBooking.Services
{
    public interface IPaypalServices
    {
        Task<Payment> CreateOrderAsync(decimal amount, string returnUrl, string cancelUrl);
    }
}
