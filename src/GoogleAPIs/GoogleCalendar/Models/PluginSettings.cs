using System;
using System.Drawing;

using BarRaider.SdTools;

using Newtonsoft.Json;

using System.ComponentModel;

namespace StreamDock.Plugins.GoogleAPIs.GoogleCalendar
{
    public class PluginSettings : INotifyPropertyChanged
    {
        //TODO
        [JsonProperty(PropertyName = "CalendarList")]
        public string PiCalendarList { get; set; }
        public string CalendarSummary { get; set; } // 캘린더 이름
        public string CalendarListJSON { get; set; }

        [JsonProperty(PropertyName = "frontColor")]
        public string PiFrontColor { get; set; }
        public Color FrontColor => GraphicsTools.ColorFromHex(PiFrontColor);

        [JsonProperty(PropertyName = "backColor")]
        public string PiBackColor { get; set; }
        public Color BackColor => GraphicsTools.ColorFromHex(PiBackColor);

        public static PluginSettings CreateDefaultSettings()
        {
            PluginSettings instance = new PluginSettings();
            instance.PiCalendarList = String.Empty;
            instance.PiFrontColor = "#FFFFFF";
            instance.PiBackColor = String.Empty;
            return instance;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
        protected void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
    }
}
