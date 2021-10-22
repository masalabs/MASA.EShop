namespace MASA.EShop.Services.Ordering.Actors;

public interface IOrderingProcessActor : IActor
{
    Task Submit(string userId, string userName, string street, string city,
        string zipCode, string state, string country, CustomerBasket basket);

    Task<bool> Cancel();

    Task<bool> Ship();

    Task NotifyPaymentSucceeded();

    Task NotifyPaymentFailed();

    Task NotifyStockConfirmed();

    Task NotifyStockRejected(List<int> rejectedProductIds);
}

