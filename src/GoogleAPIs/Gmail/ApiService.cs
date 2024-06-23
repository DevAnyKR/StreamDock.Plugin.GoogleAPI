using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Oauth2.v2;

namespace StreamDock.Plugins.GoogleAPIs.Gmail
{
    internal class ApiService : GoogleAuth
    {
        readonly GmailService gmailService;
        readonly Oauth2Service oauth2Service;

        internal ApiService(UserCredential userCredential, string user)
        {
            base.userCredential = userCredential;
            oauth2Service = new(GetServiceInitializerAsync(user).Result);
            gmailService = new GmailService(GetServiceInitializerAsync(user).Result);
        }
        internal  string GetUserId()
        {
            return oauth2Service.Userinfo.Get().Execute().Email;
        }
        internal long? GetResultSizeEstimate(string userId)
        {
            var request = gmailService.Users.Messages.List(userId);
            request.LabelIds = "UNREAD";
            return request.Execute().ResultSizeEstimate;
        }
    }
}