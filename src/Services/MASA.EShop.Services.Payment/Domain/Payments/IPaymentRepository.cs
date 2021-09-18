namespace MASA.EShop.Services.Payment.Domain.Payments;
public interface IPaymentRepository
{
    Task AddAsync(Payment payment);
}
