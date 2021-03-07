using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Valkyrie.Helper;

namespace Valkyrie.Pages
{
    public class SubdomainsModel : PageModel
    {

        [BindProperty]
        public string UserInput { get; set; }
        public void OnGet()
        {
        }
    }
}
