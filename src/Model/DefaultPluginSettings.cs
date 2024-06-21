using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

using BarRaider.SdTools;

using Newtonsoft.Json;

namespace StreamDock.Plugins
{
    abstract public class DefaultPluginSettings
    {
        [JsonProperty(PropertyName = "frontColor")]
        public string PiFrontColor { get; set; }
        public Color FrontColor => GraphicsTools.ColorFromHex(PiFrontColor);

        [JsonProperty(PropertyName = "backColor")]
        public string PiBackColor { get; set; }
        public Color BackColor => GraphicsTools.ColorFromHex(PiBackColor);

        [JsonProperty(PropertyName = "userTokenName")]
        public string PiUserTokenName { get; set; }
        public string UserTokenName => PiUserTokenName;

        [JsonProperty(PropertyName = "refreshIntervalMin")]
        public string PiRefreshIntervalMin { get; set; }
        public TimeSpan RefreshIntervalMin => TimeSpan.FromMinutes(PiRefreshIntervalMin.DefaultIfEmpty("0").ToDouble());

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
        protected void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
        public void InitializeSettings()
        {
            PiFrontColor = "#FFFFFF";
            PiBackColor = String.Empty;
            PiUserTokenName = "user";
            PiRefreshIntervalMin = String.Empty;
        }
    }
}
