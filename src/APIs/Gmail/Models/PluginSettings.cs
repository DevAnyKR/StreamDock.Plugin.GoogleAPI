using System;
using System.ComponentModel;
using System.Drawing;

using BarRaider.SdTools;

using Newtonsoft.Json;

namespace StreamDock.Plugin.GoogleAPI.Gmail
{
    internal class PluginSettings : PluginSettingsBase, IPluginSettings, INotifyPropertyChanged
    {
        //TODO
        [JsonProperty(PropertyName = "viewType")]
        public string PiViewType { get; set; } = String.Empty;
        public string ViewType { get; set; }

        [JsonProperty(PropertyName = "circleColor")]
        public string PiCircleColor { get; set; }
        public Color CircleColor => GraphicsTools.ColorFromHex(PiCircleColor);

        internal PluginSettings()
        {
            PiCircleColor = "#FF0000";
        }
    }
}
