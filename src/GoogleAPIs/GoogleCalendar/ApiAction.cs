using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BarRaider.SdTools;

using Google.Apis.Calendar.v3;
using Google.Apis.Services;

using Newtonsoft.Json.Linq;

namespace StreamDock.Plugins.GoogleAPIs.GoogleCalendar
{
    /// <summary>
    /// API 동작 정의 프로시저
    /// </summary>
    internal class ApiAction : GoogleAPI
    {
        PluginSettings pluginSettings { get; set; }
        Item item { get; set; }

        internal ApiAction(PluginSettings pluginsettings, Item item)
        {
            this.pluginSettings = pluginsettings;
            this.item = item;
        }
        internal async Task<CalendarService> GetService()
        {
            // 서비스 생성
            var service = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = await GetClientSecretAsync(),
                ApplicationName = "StreamDock Plugin"
            });

            return service;
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
                var service = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = await GetClientSecretAsync(),
                    ApplicationName = "StreamDock Plugin"
                });

                // 구글 API 통신 인스턴스
                GoogleAPIQuery googleAPIQuery = new GoogleAPIQuery(service);

                if (pluginSettings.CalendarSummary.IsNullOrEmpty())
                {
                    item.calendarID = googleAPIQuery.GetPrimaryCalendar().Id;
                }
                else
                {
                    item.calendarID = googleAPIQuery.GetCalendar(pluginSettings.CalendarSummary).Id;
                }

                Logger.Instance.LogMessage(TracingLevel.INFO, item.calendarID);

                // StreamDock 설정에 따른 동작 호출
                item.Events = await googleAPIQuery.CalendarEventsToday(item.calendarID);

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
