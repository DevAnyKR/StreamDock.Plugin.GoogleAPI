using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

using Google.Apis.Calendar.v3;

namespace StreamDock.Plugins.GoogleAPIs.GoogleCalendar
{
    internal class DataBinder : DataModel<PluginSettings, Item>
    {
        internal DataBinder(PluginSettings pluginsettings)
        {
            this.pluginSettings = pluginsettings;
            this.item = new Item();
            this.googleAuth = new();
        }
        internal async Task<Item> ServiceExecuteAsync()
        {
            ApiService apiSevice = new ApiService(googleAuth.userCredential, new CalendarService(await googleAuth.GetServiceInitializerAsync(pluginSettings.UserTokenName)));

            if (pluginSettings.CalendarSummary.IsNullOrEmpty())
            {
                item.calendarID = apiSevice.GetPrimaryCalendar().Id;
            }
            else
            {
                item.calendarID = apiSevice.GetCalendar(pluginSettings.CalendarSummary).Id;
            }

            item.Events = await apiSevice.CalendarEventsToday(item.calendarID);
            item = SetDisplayValue();
            return item;
        }
        internal void SetInitialValue()
        {
            item.DisplayValues.OnlyOne(WaitingMessage);
        }
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
        internal string GetDisplayTitle()
        {
            return item.Events?.Summary;
        }
        internal bool CheckExistData()
        {
            if (item.Events is null) return false;
            return item.Events.Items.Any();
        }
        internal Bitmap GetUpdateKeyImage(bool autoSize = false)
        {
            Bitmap bmp = new Bitmap(ImageHelper.GetImage(pluginSettings.BackColor));

            for (int i = 0; i < item.DisplayValues.Count; i++)
            {
                if (i >= 5) continue;

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
