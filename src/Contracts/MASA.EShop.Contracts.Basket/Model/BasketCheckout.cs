namespace Masa.EShop.Contracts.Basket.Model;

public record BasketCheckout(
    string Street,
    string City,
    string State,
    string Country,
    string ZipCode,
    string CardNumber,
    string CardHolderName,
    DateTime CardExpiration,
    string CardSecurityNumber,
    int CardTypeId,
    string Buyer,
    Guid RequestId);
