namespace MASA.EShop.Services.Payment.Domain.Repositories;
public interface IPaymentRepository
{
    Task AddAsync(Entities.Payment payment);
}
