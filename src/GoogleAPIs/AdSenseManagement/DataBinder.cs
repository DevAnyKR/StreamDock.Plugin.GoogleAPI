using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BarRaider.SdTools;

using Google.Apis.Adsense.v2;

namespace StreamDock.Plugins.GoogleAPIs.AdSenseManagement
{
    /// <summary>
    /// API 동작 정의 프로시저
    /// </summary>
    internal class DataBinder
    {
        PluginSettings pluginSettings;
        Item item;
        GoogleAuth googleAuth;
        internal DataBinder(PluginSettings pluginsettings, Item item)
        {
            this.pluginSettings = pluginsettings;
            this.item = item;
            this.googleAuth = new();
        }
        /// <summary>
        /// 키가 눌렸을 때 동작 정의. Google API 통신.
        /// </summary>
        internal async Task<Item> ExecuteAsync()
        {
            try
            {
                // 구글 API 통신 인스턴스
                ApiService apiSevice = new ApiService(googleAuth.userCredential, new AdsenseService(await googleAuth.GetServiceInitializerAsync(pluginSettings.UserTokenName)));

                string accountName = apiSevice.GetAccountName();
                if (Item.AccountName.IsNullOrEmpty())
                {
                    Item.AccountName = accountName;
                }

                // StreamDock 설정에 따른 동작 호출
                switch (pluginSettings.Resource)
                {
                    case Resources.Payments:
                        Item.Payments = await apiSevice.RunCallPaymentAsync();
                        break;
                    case Resources.Reports:
                        var key2 = ReportKey.Create(pluginSettings.DateRange, pluginSettings.Metrics);

                        Item.ReportResults[key2] = await apiSevice.RunCallReportAsync(pluginSettings.DateRange, pluginSettings.Metrics);
                        break;
                    case Resources.Dimensions:
                        var key3 = ReportKey.Create(pluginSettings.DateRange, pluginSettings.Metrics, pluginSettings.Dimensions);

                        Item.ReportResults[key3] = await apiSevice.RunCallReportAsync(pluginSettings.DateRange, pluginSettings.Metrics, pluginSettings.Dimensions);
                        break;
                }
#if DEBUG
                Logger.Instance.LogMessage(TracingLevel.INFO, "보고서 저장 완료.");
#endif
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
                        item.DisplayValues.OnlyOne(Item.ReportResults[ReportKey.Create(pluginSettings.DateRange, pluginSettings.Metrics)]?.Rows.First().Cells.First().Value);
                        break;
                    case Resources.Dimensions:
                        item.DisplayValues.OnlyOne(Item.ReportResults[ReportKey.Create(pluginSettings.DateRange, pluginSettings.Metrics, pluginSettings.Dimensions)]?.Totals.Cells[1].Value);
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
