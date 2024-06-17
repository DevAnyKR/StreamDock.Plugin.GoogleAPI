using Google.Apis.Auth.OAuth2;

namespace StreamDock.Plugins.GoogleAPIs
{
    /// <summary>
    /// API 최상위 클래스
    /// </summary>
    internal abstract class GoogleAPI
    {
        protected const string googleClientSecretsJson = "client_secrets.json";

        internal GoogleAPI()
        {
            GoogleWebAuthorizationBroker.Folder = @"DevAny\StreamDock.Plugins\GoogleAPI";
        }
    }
}
