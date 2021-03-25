using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
