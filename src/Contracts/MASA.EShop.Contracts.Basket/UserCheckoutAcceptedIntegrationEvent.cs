using MASA.EShop.Contracts.Basket.Model.BFF;

namespace MASA.EShop.Contracts.Basket
{
    public record UserCheckoutAcceptedIntegrationEvent(
            string UserId,
            string UserName,
            string City,
            string Street,
            string State,
            string Country,
            string ZipCode,
            string CardNumber,
            string CardHolderName,
            DateTime CardExpiration,
            string CardSecurityNumber,
            int CardTypeId,
            string Buyer,
            Guid RequestId,
            CustomerBasket Basket)
    {
        public int OrderNumber { get; set; }
    }
}
