using System.Threading.Tasks;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Oauth2.v2;

namespace StreamDock.Plugin.GoogleAPI.Gmail
{
    internal class ApiService : GoogleAuth
    {
        GmailService service;
        Oauth2Service oauth2Service;

        ApiService()
        {
        }
        internal static async Task<ApiService> GetInstance(UserCredential userCredential, string user)
        {
            var instance = new ApiService();
            instance.userCredential = userCredential;
            var initializer = await instance.GetServiceInitializerAsync(user);
            instance.oauth2Service = new Oauth2Service(initializer);
            instance.service = new GmailService(initializer);
            return instance;
        }
        internal string GetUserId()
        {
            return oauth2Service.Userinfo.Get().Execute().Email;
        }
        internal long? GetResultSizeEstimate(string userId)
        {
            var request = service.Users.Messages.List(userId);
            request.LabelIds = "UNREAD";
            return request.Execute().ResultSizeEstimate;
        }
    }
}