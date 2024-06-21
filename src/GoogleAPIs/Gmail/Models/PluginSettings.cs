using System;
using System.Drawing;

using BarRaider.SdTools;

using Newtonsoft.Json;

using System.ComponentModel;
using System.Management.Instrumentation;

namespace StreamDock.Plugins.GoogleAPIs.Gmail
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
