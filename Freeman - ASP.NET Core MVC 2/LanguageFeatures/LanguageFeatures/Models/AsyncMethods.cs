using System.Net.Http;
using System.Threading.Tasks;

namespace LanguageFeatures.Models
{
    public class AsyncMethods
    {
        public static Task<long?> GetPageLength()
        {
            var client = new HttpClient();
            var httpTask = client.GetAsync("http://apress.com");

            return httpTask.ContinueWith(antecedent =>
            {
                return antecedent.Result.Content.Headers.ContentLength;
            });
        }

        public async static Task<long?> GetPageLengthAsync()
        {
            var client = new HttpClient();
            var httpMessage = await client.GetAsync("http://apress.com");

            return httpMessage.Content.Headers.ContentLength;
        }
    }
}
