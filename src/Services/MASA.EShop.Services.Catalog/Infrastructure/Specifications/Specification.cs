
using System.Linq.Expressions;

namespace MASA.EShop.Services.Catalog.Infrastructure.Specifications;
public abstract class Specification<T> : ISpecification<T>
{
    public bool IsSatisfiedBy(T obj)
    {
        return ToExpression().Compile()(obj);
    }

    public abstract Expression<Func<T, bool>> ToExpression();

    public static implicit operator Expression<Func<T, bool>>(Specification<T> specification)
    {
        return specification.ToExpression();
    }
}
