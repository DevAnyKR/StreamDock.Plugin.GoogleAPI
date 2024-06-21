using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

using Google.Apis.Adsense.v2;

namespace StreamDock.Plugins.GoogleAPIs.AdSenseManagement
{
    /// <summary>
    /// API 동작 정의 프로시저
    /// </summary>
    internal class DataBinder : DataModel<PluginSettings, Item>
    {
        internal DataBinder(PluginSettings pluginsettings)
        {
            this.pluginSettings = pluginsettings;
            this.item = new Item();
            this.googleAuth = new();
        }
        /// <summary>
        /// 키가 눌렸을 때 동작 정의. Google API 통신.
        /// </summary>
        internal async Task<Item> ServiceExecuteAsync()
        {
            // 구글 API 통신 인스턴스
            ApiService apiSevice = new ApiService(googleAuth.userCredential, new AdsenseService(await googleAuth.GetServiceInitializerAsync(pluginSettings.UserTokenName)));

            if (Item.AccountName.IsNullOrEmpty())
            {
                Item.AccountName = apiSevice.GetAccountName();
            }

            // StreamDock 설정에 따른 동작 호출
            switch (pluginSettings.Resource)
            {
                case Resources.Payments:
                    Item.Payments = apiSevice.RunCallPayment();
                    break;
                case Resources.Reports:
                    var key2 = ReportKey.Create(pluginSettings.DateRange, pluginSettings.Metrics);

                    Item.ReportResults[key2] = apiSevice.RunCallReport(pluginSettings.DateRange, pluginSettings.Metrics);
                    break;
                case Resources.Dimensions:
                    var key3 = ReportKey.Create(pluginSettings.DateRange, pluginSettings.Metrics, pluginSettings.Dimensions);

                    Item.ReportResults[key3] = apiSevice.RunCallReport(pluginSettings.DateRange, pluginSettings.Metrics, pluginSettings.Dimensions);
                    break;
            }
            // 디스플레이용 데이터 가공
            item = SetDisplayValue();
            return item;
        }
        internal void SetInitialValue()
        {
            item.DisplayValues.OnlyOne("Press Key...");
        }
        /// <summary>
        /// StreamDock 설정에 따라 표시할 데이터를 지정합니다.
        /// </summary>
        /// <returns></returns>
        internal Item SetDisplayValue()
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

            return item;
        }
        /// <summary>
        /// 제목에 표시할 문자열을 지정합니다.
        /// </summary>
        /// <returns></returns>
        internal string GetDisplayTitle()
        {
            return string.Empty;
        }
        /// <summary>
        /// 기존 데이터가 있는지 검사합니다.
        /// </summary>
        /// <returns></returns>
        internal bool CheckExistData()
        {
            return pluginSettings.Resource switch
            {
                Resources.Payments => item.PaymentExists,
                Resources.Reports => item.ReportExists && Item.ReportResults.ContainsKey(ReportKey.Create(pluginSettings.DateRange, pluginSettings.Metrics)),
                Resources.Dimensions => item.ReportExists && Item.ReportResults.ContainsKey(ReportKey.Create(pluginSettings.DateRange, pluginSettings.Metrics, pluginSettings.Dimensions)),
                _ => false
            };
        }
        /// <summary>
        /// 키 이미지를 변경합니다. 출력할 정보를 이미지로 변환합니다.
        /// </summary>
        internal Bitmap GetUpdateKeyImage(bool initial = false)
        {
            Bitmap bmp = new Bitmap(ImageHelper.GetImage(pluginSettings.BackColor));

            switch (pluginSettings.Resource)
            {
                case Resources.Payments:
                case Resources.Reports:
                    for (int i = 0; i < item.DisplayValues.Count; i++)
                    {
                        bmp = new Bitmap(ImageHelper.SetImageText(bmp, item.DisplayValues[i].DefaultIfEmpty("No Data!"), new SolidBrush(pluginSettings.FrontColor), 72, (144 / (item.DisplayValues.Count + 1)) * (i + 1)));
                    }
                    break;
                case Resources.Dimensions:
                    if (initial)
                    {
                        bmp = new Bitmap(ImageHelper.SetImageText(bmp, item.DisplayValues[0].DefaultIfEmpty("No Data!"), new SolidBrush(pluginSettings.FrontColor), 72, 72));
                    }
                    else
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            new ChartReport(pluginSettings).CreateChart()?.SaveImage(ms, ChartImageFormat.Bmp);

                            ms.Position = 0;
                            using (var bitmap = new Bitmap(ms))
                            {
                                bmp = new Bitmap(bitmap);
                            }
                        }
                    }
                    break;
            }

            return bmp;
        }
    }
}
