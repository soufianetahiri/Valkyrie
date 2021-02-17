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
        public ActionResult OnGetChartData()
        {


            TwitterClient client = new TwitterClient(new TwitterCreds().GenerateCredentials());
            var searchParameter = new SearchTweetsParameters("#cdiscount")
            {
                //   Lang = LanguageFilter.French,
                SearchType = SearchResultType.Mixed,
                ContinueMinMaxCursor = ContinueMinMaxCursor.UntilPageSizeIsDifferentFromRequested,
                PageSize = 500,
                Until = DateTime.Now
            };

            ITweet[] tweets = client.Search.SearchTweetsAsync(searchParameter).Result;
            var x = tweets.GroupBy(d => d.CreatedAt.Date).Select(a => new { size = a.Count(), date = a.Key });

            Chart chart = new Chart
            {
                cols = new object[]
               {
                new { id = "dateofTweets", type = "string", label = "Date" },
                new { id = "ntweet", type = "number", label = "Number Of Tweets/Day" }
               }
            };
            var y = createChart().ToArray();

            chart.rows = createChart().ToArray();
            return new JsonResult(chart);


            IEnumerable<object> createChart()
            {

                foreach (var item in x)
                {
                    yield return new { c = new object[] { new { v = item.date.ToString("dd/MM/yyyy") }, new { v = item.size } } };
                }

            }
        }
    }

}
