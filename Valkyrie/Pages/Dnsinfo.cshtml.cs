using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Valkyrie.Pages
{
    public class DnsinfoModel : PageModel
    {


        private readonly string SectrailsApi = "https://api.securitytrails.com/v1/";
        private readonly string SectrailsApiKey = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["SecuritytrailsAPIKey"];
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string API_KEY = "apikey";
        public void OnGet()
        {

        }

        public async Task<ContentResult> OnPostDnshistoryAsync(string hostname)
        {
            if (!string.IsNullOrEmpty(hostname))
            {
                try
                {
                    _httpClient.DefaultRequestHeaders.Add(API_KEY, SectrailsApiKey);
                    using (var result = await _httpClient.GetAsync($"{SectrailsApi}history/{hostname}/dns/a"))
                    {
                        string content = await result.Content.ReadAsStringAsync();
                        return Content(JsonConvert.SerializeObject(content));
                    }
                }
                catch (Exception ex)
                {

                    return Content(ex.Message);
                }
            }
            else
            {
                return Content("No data found");
            }
        }
    }

    public class Value
    {
        public int ip_count { get; set; }
        public string ip { get; set; }
    }

    public class Record
    {
        public List<Value> values { get; set; }
        public string type { get; set; }
        public List<string> organizations { get; set; }
        public string last_seen { get; set; }
        public string first_seen { get; set; }
    }

    public class DnsHistory
    {
        public string type { get; set; }
        public List<Record> records { get; set; }
    }
}
