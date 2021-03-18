using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Valkyrie.Helper;

namespace Valkyrie.Pages
{
    [BindProperties(SupportsGet = true)]
    public class CertificateModel : PageModel
    {
        private PartialCert partialCert = new PartialCert();
        private readonly string CrtShUrl = "https://crt.sh/?q=#URL#&output=json";
        private readonly ILogger<CertificateModel> _logger;
        [BindProperty]
        public string UserInput { get; set; }
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        public CertificateModel(ILogger<CertificateModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }

        public PartialViewResult GetPaginatedResult(int currentPage, int pageSize = 10)
        {
            partialCert.CrtSHes = partialCert.CrtSHes.OrderBy(d => d.id).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            return Partial("_Certs", partialCert);
        }
        public async Task<JsonResult> OnPostAsync(string UserInput)
        {
            HttpAsync httpAsync = new HttpAsync();
            string response = await httpAsync.Get(CrtShUrl.Replace("#URL#", UserInput));
          //  var x = JArray.Parse(response).Reverse() ;
            return new JsonResult ( response);
        }
    }

    public class PartialCert:PageModel
    {
        public int CurrentPage { get; set; } = 1;
        public int Count { get; set; }
        public int PageSize { get; set; } = 10;
        public int TotalPages => (int)Math.Ceiling(decimal.Divide(Count, PageSize));
        public List<CrtSH> CrtSHes { get; set; }
    }
    public class CrtSH
    {
        public string issuer_ca_id { get; set; }
        public string issuer_name { get; set; }
        public string common_name { get; set; }
        public string name_value { get; set; }
        public string id { get; set; }
        public string entry_timestamp { get; set; }
        public string not_before { get; set; }
        public string not_after { get; set; }
        public string serial_number { get; set; }
    }
}
