
namespace MASA.EShop.Services.Catalog.Infrastructure.Specifications;
public interface ISpecification<TEntity>
{
    bool IsSatisfiedBy(TEntity entity);

    Expression<Func<TEntity, bool>> ToExpression();
}
