using System.Collections.Generic;

using Google.Apis.Adsense.v2.Data;

namespace StreamDock.Plugin.GoogleAPIs.AdSenseManagement
{
    /// <summary>
    /// API 데이터를 보관하는 클래스입니다. static은 스트림독의 모든 키에 공유됩니다.
    /// 데이터 보관은 API에서 제공하는 데이터 클래스를 사용하세요.
    /// </summary>
    public class Item
    {
        internal IList<Account> Accounts { get; set; }
        internal IList<Payment> Payments { get; set; }
        internal IList<Site> Sites { get; set; }
        internal IList<ReportResult> ReportResults { get; set; }

        internal bool ResultAccounts { get; set; }
        internal bool ResultPayments { get; set; }
        internal string DisplayValue1 { get; set; }
        internal string DisplayValue2 { get; set; }

        internal static Item Instance { get; set; } = new Item();
        internal Item()
        {
            Init();
        }

        internal void Init()
        {
            Accounts = new List<Account>();
            Payments = new List<Payment>();
            Sites = new List<Site>();
            ReportResults = new List<ReportResult>();

            DisplayValue1 = null;
            DisplayValue2 = null;
        }

    }
}
