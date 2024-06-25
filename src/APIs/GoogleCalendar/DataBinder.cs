using System.Drawing;

namespace StreamDock.Plugin.GoogleAPI.GoogleCalendar
{
    internal class DataBinder : DataModel<PluginSettings, Item>
    {
        internal bool ExistsUserCredential => GoogleAuth.CredentialExist(pluginSettings.UserTokenName);
        internal DataBinder(PluginSettings pluginsettings)
        {
            this.pluginSettings = pluginsettings;
            this.item = new Item();
            this.googleAuth = new();
        }
        internal async Task ServiceExecuteAsync()
        {
            ApiService apiSevice = await ApiService.GetInstance(googleAuth.userCredential, pluginSettings.UserTokenName);

            if (pluginSettings.CalendarSummary.IsNullOrEmpty())
            {
                item.calendarID = apiSevice.GetPrimaryCalendar().Id;
            }
            else
            {
                item.calendarID = apiSevice.GetCalendar(pluginSettings.CalendarSummary).Id;
            }

            item.Events = await apiSevice.CalendarEventsToday(item.calendarID);
            SetDisplayValue();
        }
        internal void SetInitValue()
        {
            item.DisplayValues.OnlyOne(WaitingMessage);
        }
        internal void SetDisplayValue()
        {
            item.DisplayValues.Clear();

            if (!item.Events.Items.Any())
            {
                item.DisplayValues.Add("No Data!");
            }
            else
            {
                foreach (var value in item.Events.Items)
                {
                    if (value.End.Date.IsDateTime())
                    {
                        //TODO 종일 또는 종료시각 설정에 따라 처리
                    }
                    item.DisplayValues.Add(value.Summary);
                }
            }
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

        int[] strOffset = [0, 0, 0, 0, 0];
        internal Bitmap GetUpdateKeyImage(bool autoSize = false)
        {
            Bitmap bmp = new Bitmap(ImageHelper.GetImage(pluginSettings.BackColor));

            for (int i = 0; i < item.DisplayValues.Count; i++)
            {
                if (i >= 4) continue;

                var font = new Font("Arial", 28, FontStyle.Bold, GraphicsUnit.Pixel);
                var stringFormat = new StringFormat
                {
                    Alignment = StringAlignment.Near,
                    LineAlignment = StringAlignment.Center
                };
                var isNear = stringFormat.Alignment == StringAlignment.Near;
                int fontX = 5;
                using (var graphics = Graphics.FromImage(bmp))
                {
                    var newSize = graphics.MeasureString(item.DisplayValues[i], font);
                    if (newSize.Width > 144)
                    {
                        fontX = 144;
                        fontX -= strOffset[i];
                    }

                    font = autoSize ? ImageHelper.ResizeFont(graphics, item.DisplayValues[i], font) : font;
                    graphics.DrawString(item.DisplayValues[i], font, new SolidBrush(pluginSettings.FrontColor), !isNear ? 72 : fontX, (120 / (item.DisplayValues.Count + 1)) * (i + 1), stringFormat);


                    strOffset[i] += 20;
                    if (strOffset[i] > newSize.Width + 144)
                    {
                        strOffset[i] = 0;
                    }
                }
            }

            return bmp;
        }
    }
}
