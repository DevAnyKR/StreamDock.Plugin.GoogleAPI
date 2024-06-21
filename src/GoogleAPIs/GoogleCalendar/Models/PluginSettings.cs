using System;
using System.ComponentModel;
using System.Drawing;

using BarRaider.SdTools;

using Newtonsoft.Json;

namespace StreamDock.Plugins.GoogleAPIs.GoogleCalendar
{
    public class PluginSettings : DefaultPluginSettings, INotifyPropertyChanged
    {
        //TODO
        [JsonProperty(PropertyName = "CalendarList")]
        public string PiCalendarList { get; set; }
        public string CalendarSummary { get; set; } // 캘린더 이름
        public string CalendarListJSON { get; set; }

        public static PluginSettings CreateDefaultSettings()
        {
            PluginSettings instance = new PluginSettings();
            instance.InitializeSettings();
            instance.PiCalendarList = String.Empty;
            return instance;
        }
    }
}
