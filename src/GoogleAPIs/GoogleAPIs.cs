using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Google.Apis.Adsense.v2;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;

namespace StreamDock.Plugins.GoogleAPIs
{
    /// <summary>
    /// API 최상위 클래스
    /// </summary>
    internal abstract class GoogleAPI
    {
        public static readonly string clientSecretsFile = "client_secrets.json";
        public static readonly string tokenPath = @"DevAny\StreamDock.Plugins\GoogleAPI";
        public static string tokenUser = "user";
        public static readonly string tokenFile = $"Google.Apis.Auth.OAuth2.Responses.TokenResponse-{tokenUser}";

        internal GoogleAPI()
        {
            GoogleWebAuthorizationBroker.Folder = tokenPath;
        }

        /// <summary>
        /// Client Secret 정보를 불러옵니다.
        /// </summary>
        /// <returns></returns>
        protected async Task<UserCredential> GetClientSecretAsync()
        {
            UserCredential credential;

            using (var stream = new FileStream(clientSecretsFile, FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    new[] {
                        AdsenseService.Scope.AdsenseReadonly,
                        CalendarService.Scope.Calendar
                    },
                    tokenUser, CancellationToken.None);
            }
#if DEBUG
            Logger.Instance.LogMessage(TracingLevel.INFO, $"Read {clientSecretsFile}");
#endif
            return credential;
        }
    }
}
