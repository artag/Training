using System.Linq.Expressions;
using Hangfire;
using Web.Interfaces;

namespace WebApp.Services;

public class BackgroundJobService : IBackgroundJobService
{
    public void Schedule<T>(Expression<Func<T, Task>> expression)
    {
        BackgroundJob.Schedule(expression, delay: TimeSpan.Zero);
    }
}
