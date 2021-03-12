using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ChartJSCore.Helpers;
using ChartJSCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Tweetinvi;
using Tweetinvi.Core.Models;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using Tweetinvi.Streaming;
using Valkyrie.Helper;
using Chart = ChartJSCore.Models.Chart;

namespace Valkyrie.Pages
{
    public class TwitterModel : PageModel
    {
        [BindProperty]
        public string UserInput { get; set; }

        public void OnGet()
        {
        }
        private List<TweetsData> GetTweets(string hashtags)
        {
            List<TweetsData> tweetList = new List<TweetsData>();
            string[] keywords = hashtags.Split(" ").Take(5).ToArray();
            foreach (string keyword in keywords)
            {
                try
                {
                    TwitterClient client = new TwitterClient(new TwitterCreds().GenerateCredentials());
                    client.RateLimits.ClearRateLimitCacheAsync();
                    var searchParam = new SearchTweetsParameters(keyword)
                    {
                        Since = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day),
                        SearchType = SearchResultType.Recent,
                        PageSize = 100,
                    };

                    var tweets = client.Search.SearchTweetsAsync(searchParam).Result.ToList();
                    tweetList.AddRange(tweets.GroupBy(d => d.CreatedAt.Date).Select(a => new TweetsData
                    { Count = a.Count(), Date = a.Key.ToString("dd/MM/yyyy"), Keyword = keyword }).ToList());
                    while (tweets.Count > 0)
                    {
                     
                        searchParam.MaxId = tweets.Select(x => x.Id).Min() - 1;
                        searchParam.Since = DateTime.Today;
                        searchParam.PageSize = 100;
                        searchParam.SearchType = SearchResultType.Recent;
                        tweets = client.Search.SearchTweetsAsync(searchParam).Result.ToList();
                        if (tweets.Count > 0)
                        {
                            tweetList.AddRange(tweets.GroupBy(d => d.CreatedAt.Date).Select(a => new TweetsData
                            { Count = a.Count(), Date = a.Key.ToString("dd/MM/yyyy"), Keyword = keyword }).ToList());
                            Debug.WriteLine($"Find : {tweets.Count}");
                            Debug.WriteLine($"All : {tweetList.Count}");
                        }
                    }
                }
                catch (Exception)
                {
                    //quick and dirty rate limit handling 
                    if (tweetList?.Count > 0)
                    {
                        return tweetList;
                    }
                }
            }
            return tweetList;


        }
        public ContentResult OnGetChartData(string hashtags)
        {
            List<string> colors = new List<string>() { "#FF6384", "#4BC0C0", "#FFCE56", "#E7E9ED", "#36A2EB" };

            List<TweetsData> tweets = GetTweets(hashtags);

             var dates = tweets.GroupBy(d => d.Date).Select(k => k.Key).ToList();
            Chart chart = new Chart
            {
                Type = Enums.ChartType.PolarArea
            };

            Data data = new Data();
            data.Labels = tweets.GroupBy(k => k.Keyword).Select(k => k.Key).ToList();
            PolarDataset dataset = new PolarDataset()
            {
                Label = "Keywords dataset",
                BackgroundColor = new List<ChartColor>(),
                Data = new List<double?>()
            };
            Random rnd = new Random();
            int i = 0;
            foreach (string label in data.Labels)
            {
                dataset.Data.Add(tweets.Where(l => l.Keyword == label).GroupBy(k => k.Count).Select(c => c.Key).Sum());
                //ChartColor.FromHexString(string.Format("#{0:X6}", rnd.Next(0x1000000))) < random colors
                dataset.BackgroundColor.Add(ChartColor.FromHexString(colors[i]));
                i++;
            }
            data.Datasets = new List<Dataset>
            {
                dataset
            };
            chart.Data = data;
            return Content(chart.CreateChartCode("lineChart"));

        }
    }
    public class TweetsData
    {
        public int Count { get; set; }
        public string Date { get; set; }
        public string Keyword { get; set; }
    }
}
