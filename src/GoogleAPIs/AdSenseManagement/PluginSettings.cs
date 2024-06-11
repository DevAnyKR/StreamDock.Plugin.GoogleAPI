using Newtonsoft.Json;

namespace StreamDock.Plugin.GoogleAPIs.AdSenseManagement
{
    internal class PluginSettings
    {
        [JsonProperty(PropertyName = "viewType")]
        public string SelectViewType { get; set; } = string.Empty;
        public GoogleAPIs.AdSenseManagement.ViewType ViewType { get; set; }

        [JsonProperty(PropertyName = "frontColor")]
        public string SelectFrontColor { get; set; } = string.Empty;

        [JsonProperty(PropertyName = "backColor")]
        public string SelectBackColor { get; set; } = string.Empty;

        public static PluginSettings CreateDefaultSettings()
        {
            return new PluginSettings();
        }
    }
}
