namespace MASA.EShop.Services.Payment.Domain.Repositories;
public interface IPaymentRepository
{
    Task CreateAsync(Entities.Payment payment);
}
