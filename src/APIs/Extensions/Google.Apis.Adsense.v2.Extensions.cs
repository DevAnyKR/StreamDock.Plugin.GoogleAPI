using System;

using Google.Apis.Util;

using GenerateRequest = Google.Apis.Adsense.v2.AccountsResource.ReportsResource.GenerateRequest;

namespace Google.Apis.Adsense.v2
{
    public static class Extensions
    {
        public static void AddDimension(this GenerateRequest request,
                                        GenerateRequest.DimensionsEnum dimension)
        {
            string dimensionText = Utilities.ConvertToString(dimension);

            request.ModifyRequest += message => {
                var uriBuilder = new UriBuilder(message.RequestUri);
                string separator = uriBuilder.Query == "" ? "" : "&";
                uriBuilder.Query += $"{separator}dimensions={dimensionText}";
                message.RequestUri = uriBuilder.Uri;
            };
        }

        public static void AddMetric(this GenerateRequest request, GenerateRequest.MetricsEnum metric)
        {
            string metricText = Utilities.ConvertToString(metric);

            request.ModifyRequest += message => {
                var uriBuilder = new UriBuilder(message.RequestUri);
                string separator = uriBuilder.Query == "" ? "" : "&";
                uriBuilder.Query += $"{separator}metrics={metricText}";
                message.RequestUri = uriBuilder.Uri;
            };
        }
    }
}
