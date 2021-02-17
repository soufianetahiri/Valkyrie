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
            _credentials = TwitterCreds.GenerateCredentials();
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
    public static class TwitterCreds
    {
        public static ITwitterCredentials GenerateCredentials()
        {
            return new TwitterCredentials("Prb9wTN0vSxfXNhCZrhYNfnki", "EstlO8eG14fE4XVwFt5VntsrcaJgUDCjybvyYxlty4x8odZ2c7",
                "353549736-fp9IBMvtYlkeCRmTqkkk1sHfvbouyYz5rleE9FrI", "sCUCjF2qIswPj7oNEDPk7C92ItcOFXshq8YyieMg7rEum");
        }

        public static ITwitterCredentials GenerateAppCreds()
        {
            var userCreds = GenerateCredentials();
            return new TwitterCredentials(userCreds.ConsumerKey, userCreds.ConsumerSecret);
        }
    }
}
