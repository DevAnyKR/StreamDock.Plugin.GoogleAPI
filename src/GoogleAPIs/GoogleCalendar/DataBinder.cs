using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using BarRaider.SdTools;

namespace StreamDock.Plugins.GoogleAPIs.GoogleCalendar
{
    /// <summary>
    /// API 동작 정의 프로시저
    /// </summary>
    internal class DataBinder
    {
        PluginSettings pluginSettings { get; set; }
        Item item { get; set; }

        internal DataBinder(PluginSettings pluginsettings, Item item)
        {
            this.pluginSettings = pluginsettings;
            this.item = item;
        }
        /// <summary>
        /// 키가 눌렸을 때 동작 정의. Google API 통신.
        /// </summary>
        internal async Task<Item> ExecuteAsync()
        {
            try
            {
                // 구글 API 통신 인스턴스
                ApiService apiSevice = await ApiService.GetService();

                if (pluginSettings.CalendarSummary.IsNullOrEmpty())
                {
                    item.calendarID = apiSevice.GetPrimaryCalendar().Id;
                }
                else
                {
                    item.calendarID = apiSevice.GetCalendar(pluginSettings.CalendarSummary).Id;
                }

                // StreamDock 설정에 따른 동작 호출
                item.Events = await apiSevice.CalendarEventsToday(item.calendarID);

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
                item.DisplayValues.Clear();

                foreach (var value in item.Events) {
                    if (value.End.Date.IsDateTime())
                    {

                    }
                    item.DisplayValues.Add(value.Summary);
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
