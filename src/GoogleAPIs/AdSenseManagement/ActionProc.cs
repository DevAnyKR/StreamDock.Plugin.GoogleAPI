using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BarRaider.SdTools;

using Google.Apis.Adsense.v2;
using Google.Apis.Services;

using StreamDock.Plugins.GoogleAPIs.GoogleAPIs.AdSenseManagement;

namespace StreamDock.Plugins.GoogleAPIs.AdSenseManagement
{
    /// <summary>
    /// API 동작 정의 프로시저
    /// </summary>
    internal class ActionProc : GoogleAPI
    {
        PluginSettings pluginSettings { get; set; }
        Item item { get; set; }

        internal ActionProc(PluginSettings pluginsettings, Item item)
        {
            this.pluginSettings = pluginsettings;
            this.item = item;
        }

        /// <summary>
        /// 키가 눌렸을 때 동작 정의. Google API 통신.
        /// </summary>
        internal async Task<Item> Execute()
        {
            try
            {
                // 서비스 생성
                var service = new AdsenseService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = await GetClientSecretAsync(),
                    ApplicationName = "StreamDock Plugin"
                });

                // 구글 API 통신 인스턴스
                GoogleAPIQuery managementApiConsumer = new GoogleAPIQuery(service, 50);

                string accountName = managementApiConsumer.GetAccountName();
                if (Item.AccountName.IsNullOrEmpty())
                {
                    Item.AccountName = accountName;
                }

                // StreamDock 설정에 따른 동작 호출
                switch (pluginSettings.Resource)
                {
                    case Resources.Payments:
                        Item.Payments = managementApiConsumer.RunCallPayment();
                        break;
                    case Resources.Reports:
                        var key = ReportKey.Create(pluginSettings.DateRange, pluginSettings.Metric);

                        Item.ReportResults[key] = managementApiConsumer.RunCallReport(pluginSettings.DateRange, pluginSettings.Metric);
                        break;
                    case Resources.Dimensions:

                        break;
                }

                // 디스플레이용 데이터 가공
                item = SetDisplayValue();
            }
            catch (Exception ex)
            {
                Logger.Instance.LogMessage(TracingLevel.ERROR, ex.Message);
                Logger.Instance.LogMessage(TracingLevel.ERROR, ex.StackTrace);
            }
            return item;
        }

        /// <summary>
        /// StreamDock 설정에 따라 표시할 데이터를 지정합니다.
        /// </summary>
        /// <returns></returns>
        internal Item SetDisplayValue()
        {
            try
            {
                switch (pluginSettings.Resource)
                {
                    case Resources.Payments:
                        item.DisplayValues.OnlyOne(Item.Payments.First().Amount);
                        break;
                    case Resources.Reports:
                        item.DisplayValues.OnlyOne(Item.ReportResults[ReportKey.Create(pluginSettings.DateRange, pluginSettings.Metric)].Rows.First().Cells.First().Value);
                        break;
                    case Resources.Dimensions:
                        item.DisplayValues.OnlyOne(Item.ReportResults[ReportKey.Create(pluginSettings.DateRange, pluginSettings.Metric, pluginSettings.Dimensions)].Totals.Cells[1].Value);

                        //TODO 차트 이미지로 표현하는 코드
                        //...
                        break;
                    default:
                        item.DisplayValues.OnlyOne("옵션 없음");
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.LogMessage(TracingLevel.ERROR, ex.Message);
                Logger.Instance.LogMessage(TracingLevel.ERROR, ex.StackTrace);
            }

            return item;
        }
    }
}
