using System;
using System.ComponentModel;

using Newtonsoft.Json;

using DateRangeEnum = Google.Apis.Adsense.v2.AccountsResource.ReportsResource.GenerateRequest.DateRangeEnum;
using DimensionsEnum = Google.Apis.Adsense.v2.AccountsResource.ReportsResource.GenerateRequest.DimensionsEnum;
using MetricsEnum = Google.Apis.Adsense.v2.AccountsResource.ReportsResource.GenerateRequest.MetricsEnum;

namespace StreamDock.Plugin.GoogleAPI.AdSenseManagement
{
    internal class PluginSettings : PluginSettingsBase, IPluginSettings, INotifyPropertyChanged
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

        public ValueTypes ValueType { get; set; }

        internal PluginSettings()
        {
            PiResource = String.Empty;
            PiDateRange = String.Empty;
            PiMetric = String.Empty;
            PiDimensions = String.Empty;
            ValueType = ValueTypes.String;
        }
    }
}
