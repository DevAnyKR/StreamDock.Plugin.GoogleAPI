using System;
using System.Threading.Tasks;

using BarRaider.SdTools;

using Google.Apis.Adsense.v2;

using Google.Apis.Services;

namespace StreamDock.Plugin.GoogleAPIs.AdSenseManagement
{
    /// <summary>
    /// API 동작 정의 프로시저
    /// </summary>
    internal class ActionProc : GoogleAPI
    {
        ViewTypes ViewType { get; set; }

        internal ActionProc(ISDConnection connection, ViewTypes viewType)
        {
            ViewType = viewType;
        }

        /// <summary>
        /// 키가 눌렸을 때 동작 정의. API 통신.
        /// </summary>
        internal async Task<Item> Execute()
        {
            var item = Item.Instance;

            try
            {
                // Create the service.
                var service = new AdsenseService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = await GetClientSecretAsync(),
                    ApplicationName = "StreamDock Plugin"
                });

                // 구글 API 통신 인스턴스
                ManagementApiConsumer managementApiConsumer = new ManagementApiConsumer(service, 10);

                // StreamDock 설정에 따른 동작 호출
                switch (this.ViewType)
                {
                    case ViewTypes.Payments:
                        item = managementApiConsumer.RunCallPayment();
                        break;
                    default:
                        item.Value1 = "옵션 없음";
                        break;
                }
            }
            catch (Exception ex)
            {
                item = null;
                Logger.Instance.LogMessage(TracingLevel.ERROR, ex.Message);
                Logger.Instance.LogMessage(TracingLevel.ERROR, ex.StackTrace);
            }
            return item;
        }
    }
}
