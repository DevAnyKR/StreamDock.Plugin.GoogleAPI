using System;
using System.Drawing;

using BarRaider.SdTools;

using Newtonsoft.Json;

using System.ComponentModel;

namespace StreamDock.Plugins.GoogleAPIs.Gmail
{
    public class PluginSettings : INotifyPropertyChanged
    {
        //TODO
        [JsonProperty(PropertyName = "viewType")]
        public string PiViewType { get; set; }
        public string ViewType { get; set; }

        [JsonProperty(PropertyName = "frontColor")]
        public string PiFrontColor { get; set; }
        public Color FrontColor => GraphicsTools.ColorFromHex(PiFrontColor);

        [JsonProperty(PropertyName = "backColor")]
        public string PiBackColor { get; set; }
        public Color BackColor => GraphicsTools.ColorFromHex(PiBackColor);

        [JsonProperty(PropertyName = "circleColor")]
        public string PiCircleColor { get; set; }
        public Color CircleColor => GraphicsTools.ColorFromHex(PiCircleColor);

        [JsonProperty(PropertyName = "userTokenName")]
        public string PiUserTokenName { get; set; }
        public string UserTokenName => PiUserTokenName;

        public static PluginSettings CreateDefaultSettings()
        {
            PluginSettings instance = new PluginSettings();
            instance.PiViewType = String.Empty;
            instance.PiFrontColor = "#FFFFFF";
            instance.PiBackColor = String.Empty;
            instance.PiUserTokenName = "user";
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
