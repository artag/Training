using System.Threading.Tasks;

namespace Mobile.UseCases;

public interface IJob
{
    Task ExecuteAsync();
}
