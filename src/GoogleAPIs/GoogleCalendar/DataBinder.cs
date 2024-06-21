using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

using Google.Apis.Calendar.v3;

namespace StreamDock.Plugins.GoogleAPIs.GoogleCalendar
{
    /// <summary>
    /// API 동작 정의 프로시저
    /// </summary>
    internal class DataBinder
    {
        internal PluginSettings pluginSettings;
        internal Item item;
        GoogleAuth googleAuth;

        internal DataBinder(PluginSettings pluginsettings)
        {
            this.pluginSettings = pluginsettings;
            this.item = new();
            this.googleAuth = new();
        }
        /// <summary>
        /// 키가 눌렸을 때 동작 정의. Google API 통신.
        /// </summary>
        internal async Task<Item> ServiceExecuteAsync()
        {
            // 구글 API 통신 인스턴스
            ApiService apiSevice = new ApiService(googleAuth.userCredential, new CalendarService(await googleAuth.GetServiceInitializerAsync(pluginSettings.UserTokenName)));

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
            item.DisplayValues.Clear();

            foreach (var value in item.Events.Items)
            {
                if (value.End.Date.IsDateTime())
                {
                    //TODO 종일 또는 종료시각 설정에 따라 처리
                }
                item.DisplayValues.Add(value.Summary);
            }

            return item;
        }
        /// <summary>
        /// 제목에 표시할 문자열을 지정합니다.
        /// </summary>
        /// <returns></returns>
        internal string GetDisplayTitle()
        {
            return item.Events?.Summary;
        }
        /// <summary>
        /// 기존 데이터가 있는지 검사합니다.
        /// </summary>
        /// <returns></returns>
        internal bool CheckExistData()
        {
            if (item.Events is null) return false;
            return item.Events.Items.Any();
        }
        /// <summary>
        /// 키 이미지를 변경합니다. 출력할 정보를 이미지로 변환합니다.
        /// </summary>
        internal Bitmap GetUpdateKeyImage(bool autoSize = false)
        {
            Bitmap bmp = new Bitmap(ImageHelper.GetImage(pluginSettings.BackColor));

            for (int i = 0; i < item.DisplayValues.Count; i++)
            {
                var font = new Font("Arial", 32, FontStyle.Bold, GraphicsUnit.Pixel);
                var stringFormat = new StringFormat
                {
                    Alignment = StringAlignment.Near,
                    LineAlignment = StringAlignment.Center
                };
                var isRGB = stringFormat.Alignment == StringAlignment.Near;

                using (var graphics = Graphics.FromImage(bmp))
                {
                    font = autoSize ? ImageHelper.ResizeFont(graphics, item.DisplayValues[i], font) : font;
                    graphics.DrawString(item.DisplayValues[i], font, new SolidBrush(pluginSettings.FrontColor), !isRGB ? 72 : 5, (144 / (item.DisplayValues.Count + 1)) * (i + 1), stringFormat);
                }
            }

            return bmp;
        }
    }
}
