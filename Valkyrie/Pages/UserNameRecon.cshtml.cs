using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Valkyrie.Pages
{
    public class UserNameReconModel : PageModel
    {
        public void OnGet()
        {
    //        var x =LoadSocialFileAsync("soufiane").ConfigureAwait(true);
            
        }




        public async Task<ContentResult> OnPostNickNameAsync(string nickname)
        {
            List<SocialModel> socialModels = JsonConvert.DeserializeObject<List<SocialModel>>(Resource.SocialSites);
            List<ResultModel> resultModels = new List<ResultModel>();
            foreach (SocialModel socialModel in socialModels)
            {
                try
                {
                   //fix vk.com encoding
                    Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                    Encoding.GetEncoding("windows-1254");

                    using HttpClient client = new HttpClient();
                    string url = socialModel.url.Replace("{}", nickname);
                    client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.90 Safari/537.36");
                    bool available = false;

                    using (var response = await client.GetAsync(url))
                    {
                        try
                        {
                        string responseData = await response.Content.ReadAsStringAsync();
                        if (socialModel.errorType == 0)
                        {
                            if ((int)response.StatusCode != 200)
                            {
                                available = true;
                            }
                        }
                        else if ((int)response.StatusCode == 1)
                        {
                            if (responseData.Contains(socialModel.errorMsg.ToString()))
                            {
                                available = true;
                            }
                        }
                        else if ((int)response.StatusCode == 2)
                        {
                            if ((int)response.StatusCode != 200 && responseData.Contains(socialModel.errorMsg.ToString()))
                            {
                                available = true;
                            }
                        }
                        }
                        catch (Exception ex)
                        {

                            throw;
                        }
                    }
                    resultModels.Add(new ResultModel { Available = available, Service = socialModel.service, ProfileUrl = url });

                }
                catch (Exception)
                {

                    throw;
                }
            }
            return Content(JsonConvert.SerializeObject( resultModels));
        }
    }
    public class SocialModel
    {
        public string service { get; set; }
        public string url { get; set; }
        public string urlRegister { get; set; }
        public int errorType { get; set; }
        public object errorMsg { get; set; }
    }
    public class ResultModel
    {
        public bool Available { get; set; }
        public string Service { get; set; }
        public string ProfileUrl { get; set; }
    }
}
