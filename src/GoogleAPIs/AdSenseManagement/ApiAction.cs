using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using BarRaider.SdTools;

using Google.Apis.Adsense.v2;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;

namespace StreamDock.Plugins.GoogleAPIs.AdSenseManagement
{
    /// <summary>
    /// API 동작 정의 프로시저
    /// </summary>
    internal class ApiAction : GoogleAPI
    {
        PluginSettings pluginSettings { get; set; }
        Item item { get; set; }

        /// <summary>
        /// Client Secret 정보를 불러옵니다.
        /// </summary>
        /// <returns></returns>
        protected async Task<UserCredential> GetClientSecretAsync()
        {
            UserCredential credential;

            using (var stream = new FileStream(googleClientSecretsJson, FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    new[] { AdsenseService.Scope.AdsenseReadonly },
                    "user", CancellationToken.None);
            }
            Logger.Instance.LogMessage(TracingLevel.INFO, "Read client_secrets.json");

            return credential;
        }

        internal ApiAction(PluginSettings pluginsettings, Item item)
        {
            this.pluginSettings = pluginsettings;
            this.item = item;
        }

        /// <summary>
        /// 키가 눌렸을 때 동작 정의. Google API 통신.
        /// </summary>
        internal async Task<Item> ExecuteAsync()
        {
            Item _item = this.item;

            try
            {
                // 서비스 생성
                var service = new AdsenseService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = await GetClientSecretAsync(),
                    ApplicationName = "StreamDock Plugin"
                });

                // 구글 API 통신 인스턴스
                GoogleAPIQuery googleAPIQuery = new GoogleAPIQuery(service, 50);

                string accountName = googleAPIQuery.GetAccountName();
                if (Item.AccountName.IsNullOrEmpty())
                {
                    Item.AccountName = accountName;
                }

                // StreamDock 설정에 따른 동작 호출
                switch (pluginSettings.Resource)
                {
                    case Resources.Payments:
                        Item.Payments = await googleAPIQuery.RunCallPaymentAsync();
                        break;
                    case Resources.Reports:
                        var key2 = ReportKey.Create(pluginSettings.DateRange, pluginSettings.Metrics);

                        Item.ReportResults[key2] = await googleAPIQuery.RunCallReportAsync(pluginSettings.DateRange, pluginSettings.Metrics);
                        break;
                    case Resources.Dimensions:
                        var key3 = ReportKey.Create(pluginSettings.DateRange, pluginSettings.Metrics, pluginSettings.Dimensions);

                        Item.ReportResults[key3] = await googleAPIQuery.RunCallReportAsync(pluginSettings.DateRange, pluginSettings.Metrics, pluginSettings.Dimensions);
                        break;
                }
#if DEBUG
                Logger.Instance.LogMessage(TracingLevel.INFO, "보고서 저장 완료.");
#endif
                // 디스플레이용 데이터 가공
                _item = SetDisplayValue();
            }
            catch (Exception ex)
            {
                Logger.Instance.LogMessage(TracingLevel.ERROR, ex.Message);
                Logger.Instance.LogMessage(TracingLevel.ERROR, ex.StackTrace);
            }
            return _item;
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
                        item.DisplayValues.OnlyOne(Item.ReportResults[ReportKey.Create(pluginSettings.DateRange, pluginSettings.Metrics)].Rows.First().Cells.First().Value);
                        break;
                    case Resources.Dimensions:
                        item.DisplayValues.OnlyOne(Item.ReportResults[ReportKey.Create(pluginSettings.DateRange, pluginSettings.Metrics, pluginSettings.Dimensions)].Totals.Cells[1].Value);
                        break;
                    default:
                        item.DisplayValues.OnlyOne("옵션 없음");
                        break;
                }
#if DEBUG
                Logger.Instance.LogMessage(TracingLevel.INFO, "표시 값 저장 완료.");
#endif
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
