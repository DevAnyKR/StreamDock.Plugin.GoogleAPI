using System;
using System.Drawing;

using BarRaider.SdTools;

using Newtonsoft.Json;

using System.ComponentModel;

namespace StreamDock.Plugins.GoogleAPIs.Gmail
{
    public class PluginSettings : DefaultPluginSettings, INotifyPropertyChanged
    {
        //TODO
        [JsonProperty(PropertyName = "viewType")]
        public string PiViewType { get; set; }
        public string ViewType { get; set; }

        [JsonProperty(PropertyName = "circleColor")]
        public string PiCircleColor { get; set; }
        public Color CircleColor => GraphicsTools.ColorFromHex(PiCircleColor);

        public static PluginSettings CreateDefaultSettings()
        {
            PluginSettings instance = new PluginSettings();
            instance.InitializeSettings();
            instance.PiViewType = String.Empty;
            instance.PiCircleColor = "#FF0000";
            return instance;
        }
    }
}
