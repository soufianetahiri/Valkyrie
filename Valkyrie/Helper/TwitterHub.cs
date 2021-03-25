﻿using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Core.Models;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Valkyrie.Helper
{
    public class TwitterHub : Hub
    {
        private readonly ITwitterCredentials _credentials;
        private bool IsMonitorOn = true;

        public TwitterHub()
        {
            _credentials = new TwitterCreds().GenerateCredentials();
        }
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public async Task getTweetsAsync(string hashtags)
        {
            if (!string.IsNullOrEmpty(hashtags))
            {
                foreach (string hashtag in hashtags.Split(" ").Take(5).ToArray())
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
                SearchType = SearchResultType.Mixed,
                ContinueMinMaxCursor = ContinueMinMaxCursor.UntilPageSizeIsDifferentFromRequested,
                PageSize = 10,
                Until = DateTime.Now
            };

            ITweet[] tweets = client.Search.SearchTweetsAsync(searchParameter).Result;
            if (tweets != null)
            {
                foreach (Tweet tweet in tweets)
                {
                    MiniTweet miniTweet = new MiniTweet
                    {
                        DateOfTweet = tweet.CreatedAt.ToUniversalTime().ToString(),
                        FullText = tweet.FullText,
                        User = tweet.CreatedBy.ToString(),
                        Link = tweet.Url
                    };
                    await SendMessage(JsonConvert.SerializeObject(miniTweet));
                }
            }
            // Then monitor
            IsMonitorOn = true;
            stream.AddTrack(hashtag);
            stream.MatchingTweetReceived += async (sender, args) =>
            {
                // This event will be invoked every time a tweet created is matching your criteria
                var tweet = args.Tweet;
                MiniTweet miniTweet = new MiniTweet
                {
                    DateOfTweet = tweet.CreatedAt.ToUniversalTime().ToString(),
                    FullText = tweet.FullText,
                    User = tweet.CreatedBy.ToString(),
                    Link = tweet.Url
                };
                await SendMessage(JsonConvert.SerializeObject(miniTweet));
                if (!IsMonitorOn)
                {
                    stream.Stop();
                }
            };
            await stream.StartMatchingAllConditionsAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            IsMonitorOn = false;
            await base.OnDisconnectedAsync(exception);
        }
    }
    public class MiniTweet
    {
        public string FullText { get; set; }
        public string User { get; set; }
        public string DateOfTweet { get; set; }
        public string Link { get; set; }
    }
}

