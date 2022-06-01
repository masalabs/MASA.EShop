namespace Masa.EShop.Services.Payment.Infrastructure.Repositories;

public class PaymentRepository : Repository<PaymentDbContext, Domain.Aggregate.Payment>, IPaymentRepository
{
    public PaymentRepository(PaymentDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
    {
    }
}
