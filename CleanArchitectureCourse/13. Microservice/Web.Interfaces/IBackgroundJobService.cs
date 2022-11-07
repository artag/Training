using System.Linq.Expressions;

namespace Web.Interfaces;

public interface IBackgroundJobService
{
    void Schedule<T>(Expression<Func<T, Task>> expression);
}
