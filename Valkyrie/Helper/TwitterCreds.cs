using Tweetinvi.Models;

namespace Valkyrie.Helper
{
    public class TwitterCreds
    {
        private readonly ITwitterCredentials _credentials;
        public TwitterCreds() => _credentials = GenerateCredentials();



        public ITwitterCredentials GenerateCredentials()
        {
            return new TwitterCredentials("Prb9wTN0vSxfXNhCZrhYNfnki", "EstlO8eG14fE4XVwFt5VntsrcaJgUDCjybvyYxlty4x8odZ2c7",
                "353549736-fp9IBMvtYlkeCRmTqkkk1sHfvbouyYz5rleE9FrI", "sCUCjF2qIswPj7oNEDPk7C92ItcOFXshq8YyieMg7rEum");
        }

        public ITwitterCredentials GenerateAppCreds()
        {
            var userCreds = GenerateCredentials();
            return new TwitterCredentials(userCreds.ConsumerKey, userCreds.ConsumerSecret);
        }
    }
}
