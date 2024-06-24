using System;
using System.ComponentModel;
using System.Drawing;

using BarRaider.SdTools;

using Newtonsoft.Json;

namespace StreamDock.Plugin.GoogleAPI.GoogleCalendar
{
    internal class PluginSettings : PluginSettingsBase, IPluginSettings, INotifyPropertyChanged
    {
        //TODO
        [JsonProperty(PropertyName = "CalendarList")]
        public string PiCalendarList { get; set; }
        internal string CalendarSummary { get; set; } // 캘린더 이름
        internal string CalendarListJSON { get; set; }

        internal PluginSettings()
        {
            PiCalendarList = String.Empty;
        }
    }
}
