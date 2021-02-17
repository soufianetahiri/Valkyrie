using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Core.Models;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Valkyrie.Helper
{
    public class TwitterHub:Hub
    {
        private readonly ITwitterCredentials _credentials;
        public TwitterHub()
        {
            _credentials = new TwitterCreds().GenerateCredentials();
        }
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
 
        public async Task getTweetsAsync(string hashtags)
        {
            if (!string.IsNullOrEmpty(hashtags))
            {
                foreach (string hashtag in hashtags.Split(";"))
                {
                    await StartMonitoring(hashtag);
                }
            }
          
        }

        private async Task StartMonitoring(string hashtag)
        {
            TwitterClient client = new TwitterClient(_credentials);
            var stream = client.Streams.CreateFilteredStream();

            //Start by searching

            var searchParameter = new SearchTweetsParameters(hashtag)
            {
             //   Lang = LanguageFilter.French,
                SearchType = SearchResultType.Mixed,
                ContinueMinMaxCursor = ContinueMinMaxCursor.UntilPageSizeIsDifferentFromRequested,
                PageSize = 20,
                Until = DateTime.Now
            };

            ITweet[] tweets = client.Search.SearchTweetsAsync(searchParameter).Result;
            if (tweets != null)
            {
                foreach (Tweet tweet in tweets)
                {
                    await SendMessage(tweet.CreatedBy.Name, tweet.FullText);
                }
            }

            // Then monitor
            stream.AddTrack(hashtag);
       

            stream.MatchingTweetReceived += async (sender, args) =>
            {
                // This event will be invoked every time a tweet created is matching your criteria
                var tweet = args.Tweet;
                await SendMessage(tweet.CreatedBy.Name, tweet.FullText);

            };
            await stream.StartMatchingAllConditionsAsync();
        }
    }
   
}
