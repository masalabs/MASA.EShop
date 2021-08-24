
namespace MASA.EShop.Services.Catalog.Infrastructure.Specifications;
public interface ISpecification<T>
{
    bool IsSatisfiedBy(T obj);

    Expression<Func<T, bool>> ToExpression();
}
