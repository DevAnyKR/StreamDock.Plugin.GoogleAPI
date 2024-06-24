using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;

using BarRaider.SdTools;

using Newtonsoft.Json;

using StreamDock.Plugin.GoogleAPI;

namespace StreamDock.Plugin
{
    abstract internal class PluginSettingsBase
    {
        [JsonProperty(PropertyName = "frontColor")]
        public string PiFrontColor { get; set; } = "#FFFFFF";
        internal Color FrontColor => GraphicsTools.ColorFromHex(PiFrontColor);

        [JsonProperty(PropertyName = "backColor")]
        public string PiBackColor { get; set; } = "#000000";
        internal Color BackColor => GraphicsTools.ColorFromHex(PiBackColor);

        [JsonProperty(PropertyName = "userTokenName")]
        public string PiUserTokenName { get; set; } = "user";
        private string userTokenName;
        internal string UserTokenName
        {
            get
            {
                if (userTokenName != PiUserTokenName)
                {
                    userTokenName = PiUserTokenName;
                    OnPropertyChanged("UserTokenName");
                }
                return userTokenName;
            }
        }

        [JsonProperty(PropertyName = "refreshIntervalMin")]
        public string PiRefreshIntervalMin { get; set; } = "0";
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
    }
}
