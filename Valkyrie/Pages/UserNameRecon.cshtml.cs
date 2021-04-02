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
              
                    resultModels.Add(new ResultModel { Available = available.ToString(), Service = socialModel.service, ProfileUrl = url });

                }
                catch (Exception)
                {
                    resultModels.Add(new ResultModel { Available ="unk", Service = socialModel.service, ProfileUrl = socialModel.url.Replace("{}", nickname) });
 
                }
  
            }
            return Content(ConstructDivs(resultModels));
        }
        public string ConstructDivs(List<ResultModel>  resultModels)
        {
            string baseDivs = " <div class=\"card mb-4 border-{0}\" style=\"min-width:12rem; max-height:12rem;\"> " +
                "  <div class=\"card-body\">" +
                " <h5 class=\"card-title\">{1}</h5>" +
                "       <p class=\"card-text\">{2}</p>" +
                "     </div><div class=\"card-footer\"> " +
                "    <small class=\"text-muted\">Check<a href=\"{3}\" target=\"_blank\"> by yourself</a></small> " +
                "    </div></div>";
            StringBuilder sb = new StringBuilder();
            foreach (ResultModel resultModel in resultModels)
            {
                if (resultModel.Available=="unk")
                {
                    sb.Append(string.Format(baseDivs, "warning", resultModel.Service, "Unable to tell, please visit the profile by yourself.", resultModel.ProfileUrl));
                }
                if (resultModel.Available== "False")
                {
                    sb.Append(string.Format(baseDivs, "success", resultModel.Service, $"Profile exists on {resultModel.Service}.", resultModel.ProfileUrl));
                }
                if (resultModel.Available == "True")
                {
                    sb.Append(string.Format(baseDivs, "danger", resultModel.Service, $"Profile not found on {resultModel.Service}.", resultModel.ProfileUrl));

                }
            }
            return sb.ToString() ;
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
        public string Available { get; set; }
        public string Service { get; set; }
        public string ProfileUrl { get; set; }
    }
}
