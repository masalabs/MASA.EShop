namespace MASA.EShop.Services.Payment.Infrastructure.Repositories;
public class PaymentRepository : IPaymentRepository
{
    private readonly PaymentDbContext _dbContext = default!;

    public PaymentRepository(PaymentDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateAsync(Domain.Entities.Payment payment)
    {
        await _dbContext.AddAsync(payment);

        await _dbContext.SaveChangesAsync();
    }
}
