using System.Net.Http;
using System.Threading.Tasks;

namespace Valkyrie.Helper
{
    public class HttpAsync
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public async Task<string> Get(string url)
        {
            // The actual Get method
            using (var result = await _httpClient.GetAsync(url))
            {
                string content = await result.Content.ReadAsStringAsync();
                return content;
            }
        }
    }
}
