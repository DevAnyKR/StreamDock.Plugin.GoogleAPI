using System;
using System.Drawing;

using BarRaider.SdTools;

using Newtonsoft.Json;

using DateRangeEnum = Google.Apis.Adsense.v2.AccountsResource.ReportsResource.GenerateRequest.DateRangeEnum;
using MetricsEnum = Google.Apis.Adsense.v2.AccountsResource.ReportsResource.GenerateRequest.MetricsEnum;
using DimensionsEnum = Google.Apis.Adsense.v2.AccountsResource.ReportsResource.GenerateRequest.DimensionsEnum;
using System.ComponentModel;

namespace StreamDock.Plugins.GoogleAPIs.AdSenseManagement
{
    public class PluginSettings : INotifyPropertyChanged
    {
        //? Json 속성 이름과 CS 속성 이름이 중복되면 안 됨. 대소문자 구분.
        [JsonProperty(PropertyName = "resource")]
        public string PiResource { get; set; } = "Payments";

        private Resources resources;
        public Resources Resource
        {
            get
            {
                var _ = PiResource.TryParse<Resources>();
                if (resources != _)
                {
                    resources = _;
                    OnPropertyChanged("Resources");
                }
                return resources;
            }
        }

        [JsonProperty(PropertyName = "dateRange")]
        public string PiDateRange { get; set; }
        private DateRangeEnum dateRange;
        public DateRangeEnum DateRange
        {
            get
            {
                var _ = PiDateRange.TryParse<DateRangeEnum>();
                if (dateRange != _)
                {
                    dateRange = _;
                    OnPropertyChanged("DateRange");
                }
                return dateRange;
            }
        }

        [JsonProperty(PropertyName = "metrics")]
        public string PiMetric { get; set; }
        private MetricsEnum metrics;
        public MetricsEnum Metrics
        {
            get
            {
                var _ = PiMetric.TryParse<MetricsEnum>();
                if (metrics != _)
                {
                    metrics = _;
                    OnPropertyChanged("Metrics");
                }
                return metrics;
            }
        }

        [JsonProperty(PropertyName = "dimensions")]
        public string PiDimensions { get; set; }
        private DimensionsEnum dimensions;
        public DimensionsEnum Dimensions
        {
            get
            {
                var _ = PiDimensions.TryParse<DimensionsEnum>();
                if (dimensions != _)
                {
                    dimensions = _;
                    OnPropertyChanged("Dimensions");
                }
                return dimensions;
            }
        }

        [JsonProperty(PropertyName = "frontColor")]
        public string PiFrontColor { get; set; }
        public Color FrontColor => GraphicsTools.ColorFromHex(PiFrontColor);

        [JsonProperty(PropertyName = "backColor")]
        public string PiBackColor { get; set; }
        public Color BackColor => GraphicsTools.ColorFromHex(PiBackColor);

        [JsonProperty(PropertyName = "userTokenName")]
        public string PiUserTokenName { get; set; }
        public string UserTokenName => PiUserTokenName;

        public ValueTypes ValueType { get; set; }

        public static PluginSettings CreateDefaultSettings()
        {
            PluginSettings instance = new PluginSettings();
            instance.PiResource = String.Empty;
            instance.PiDateRange = String.Empty;
            instance.PiMetric = String.Empty;
            instance.PiDimensions = String.Empty;
            instance.PiFrontColor = "#FFFFFF";
            instance.PiBackColor = String.Empty;
            instance.PiUserTokenName = "user";
            instance.ValueType = ValueTypes.String;
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
