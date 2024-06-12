using System.Drawing;

using Newtonsoft.Json;

namespace StreamDock.Plugin.GoogleAPIs.AdSenseManagement
{
    internal class PluginSettings
    {
        [JsonProperty(PropertyName = "viewType")]
        public string PiViewType { get; set; } = string.Empty;
        public ViewTypes ViewType { get; set; }

        [JsonProperty(PropertyName = "frontColor")]
        public string PiFrontColor { get; set; } = string.Empty;
        public Color FrontColor { get; set; }

        [JsonProperty(PropertyName = "backColor")]
        public string PiBackColor { get; set; } = string.Empty;
        public Color BackColor { get; set; }

        public static PluginSettings CreateDefaultSettings()
        {
            return new PluginSettings();
        }
    }
}
