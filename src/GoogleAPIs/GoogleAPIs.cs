using System.IO;
using System.Threading;
using System.Threading.Tasks;

using BarRaider.SdTools;

using Google.Apis.Adsense.v2;
using Google.Apis.Auth.OAuth2;

namespace StreamDock.Plugins.GoogleAPIs
{
    /// <summary>
    /// API 최상위 클래스
    /// </summary>
    internal abstract class GoogleAPI
    {
        internal GoogleAPI()
        {
            GoogleWebAuthorizationBroker.Folder = @"DevAny\StreamDock.Plugins\GoogleAPI";
        }

        /// <summary>
        /// Client Secret 정보를 불러옵니다.
        /// </summary>
        /// <returns></returns>
        protected async Task<UserCredential> GetClientSecretAsync()
        {
            UserCredential credential;

            using (var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    new[] { AdsenseService.Scope.AdsenseReadonly },
                    "user", CancellationToken.None);
            }
            Logger.Instance.LogMessage(TracingLevel.INFO, "OAuth2 json 읽기 완료.");

            return credential;
        }
    }
}
