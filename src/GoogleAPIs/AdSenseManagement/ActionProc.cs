using System.Drawing;
using System.IO;
using System.Threading;

using BarRaider.SdTools;

using Google.Apis.Adsense.v2;
using Google.Apis.Auth.OAuth2;

using Google.Apis.Services;

namespace StreamDock.Plugin.GoogleAPIs.AdSenseManagement
{
    /// <summary>
    /// API 동작 정의 프로시저
    /// </summary>
    internal class ActionProc : GoogleAPIs
    {
        protected Color bgColor;
        protected readonly ISDConnection sDConnection;
        ViewTypes ViewType { get; set; }

        internal ActionProc(ISDConnection connection, ViewTypes viewType)
        {
            this.sDConnection = connection;
            this.ViewType = viewType;
            GoogleWebAuthorizationBroker.Folder = "StreamDock Plugin";
        }

        /// <summary>
        /// 키가 눌렸을 때 동작 정의
        /// </summary>
        internal async void OnPress()
        {
            UserCredential credential;

            using (var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
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

            // 구글 API 통신 인스턴스
            ManagementApiConsumer managementApiConsumer = new ManagementApiConsumer(service, 10);

            // StreamDock 설정에 따른 동작 호출
            string value = null;
            switch (this.ViewType)
            {
                case ViewTypes.Payments:
                    value = managementApiConsumer.RunCallPayment();
                    break;
                default:
                    value = "옵션 없음";
                    break;
            }
            SetImageKey(value);
        }

        /// <summary>
        /// 키 이미지 변경
        /// </summary>
        /// <param name="value"></param>
        protected void SetImageKey(string value)
        {
            bgColor = Color.Black;
            var keyImage = ImageHelper.GetImage(bgColor);
            keyImage = ImageHelper.SetImageText(keyImage, value);
            sDConnection.SetImageAsync(keyImage);
        }
    }
}
