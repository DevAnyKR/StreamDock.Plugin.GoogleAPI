using System.IO;
using System.Threading;

using BarRaider.SdTools;

using Google.Apis.Adsense.v2;
using Google.Apis.Auth.OAuth2;

using Google.Apis.Services;

namespace StreamDock.Plugin.GoogleAPIs.AdSenseManagement
{
    internal class Payment : AdSense
    {
        internal Payment(ISDConnection connection, ViewType viewType) : base(connection, viewType)
        {
        }

        internal override async void OnPress()
        {
            UserCredential credential;

            //Logger.Instance.LogMessage(TracingLevel.INFO, "OAuth2 json 읽는 중...");
            using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] { AdsenseService.Scope.AdsenseReadonly },
                    "user", CancellationToken.None);
            }
            Logger.Instance.LogMessage(TracingLevel.INFO, "OAuth2 json 읽기 완료.");


            // Create the service.
            var service = new AdsenseService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "StreamDock Plugin"
            });

            //Logger.Instance.LogMessage(TracingLevel.INFO, "API 통신 시작...");
            // API 통신 시작
            ManagementApiConsumer managementApiConsumer = new ManagementApiConsumer(service, 10);
            string value = managementApiConsumer.RunCallPayment();
            SetImageKey(value);
        }
    }
}
