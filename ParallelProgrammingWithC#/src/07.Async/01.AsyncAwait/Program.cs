using System.Threading.Tasks;

namespace AsyncAwait
{
    internal static class Program
    {
        public static async Task Main()
        {
            // CalcSync.Execute();
            // CalcTask.Execute();
            await CalcAsync.Execute();
        }
    }
}
