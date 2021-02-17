using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Tweetinvi;
using Tweetinvi.Core.Models;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Tweetinvi.Streaming;
using Valkyrie.Helper;

namespace Valkyrie.Pages
{
    public class TwitterModel : PageModel
    {
        [BindProperty]
        public string UserInput { get; set; }
        public async Task OnGetAsync()
        {
            //TwitterHub twitterHub = new TwitterHub();
            //await twitterHub.getTweetsAsync();
        }
    }

}
