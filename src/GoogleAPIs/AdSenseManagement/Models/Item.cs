using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

using Google.Apis.Adsense.v2.Data;

namespace StreamDock.Plugins.GoogleAPIs.AdSenseManagement
{
    /// <summary>
    /// API 데이터를 보관하는 클래스입니다. static은 스트림독의 모든 키에 공유됩니다.
    /// 데이터 형식은 API에서 제공하는 데이터 클래스를 사용하세요.
    /// </summary>
    public class Item
    {
        internal static IList<Account> Accounts { get; set; } = new List<Account>();
        internal static IList<Payment> Payments { get; set; } = new List<Payment>();
        internal static IList<Site> Sites { get; set; } = new List<Site>();
        internal static IDictionary<IReportKey, ReportResult> ReportResults { get; set; } = new Dictionary<IReportKey, ReportResult> { };
        internal static IDictionary<IReportKey, Image> ReportCharts { get; set; } = new Dictionary<IReportKey, Image> { };
        internal static string AccountName { get; set; }
        internal bool AccountExists => !AccountName.IsNullOrEmpty() && !Accounts.IsNullOrEmpty() && Accounts.Any();
        internal bool PaymentExists => !Payments.IsNullOrEmpty() && !Payments.IsNullOrEmpty() && Payments.Any();
        internal bool ReportExists => !ReportResults.IsNullOrEmpty() && !ReportResults.IsNullOrEmpty() && ReportResults.Any();

        internal IList<string> DisplayValues { get; set; } = new List<string>();

        internal void Init()
        {
            Accounts = new List<Account>();
            Payments = new List<Payment>();
            Sites = new List<Site>();
            ReportResults = new Dictionary<IReportKey, ReportResult> { };
            ReportCharts = new Dictionary<IReportKey, Image> { };
        }
    }
}
