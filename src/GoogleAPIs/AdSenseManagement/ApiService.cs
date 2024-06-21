using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BarRaider.SdTools;

using Google.Apis.Adsense.v2; // https://googleapis.dev/dotnet/Google.Apis.Adsense.v2/latest/api/Google.Apis.Adsense.v2.html
using Google.Apis.Adsense.v2.Data;
using Google.Apis.Auth.OAuth2;

using DateRangeEnum = Google.Apis.Adsense.v2.AccountsResource.ReportsResource.GenerateRequest.DateRangeEnum;
using DimensionsEnum = Google.Apis.Adsense.v2.AccountsResource.ReportsResource.GenerateRequest.DimensionsEnum;
using MetricsEnum = Google.Apis.Adsense.v2.AccountsResource.ReportsResource.GenerateRequest.MetricsEnum;

namespace StreamDock.Plugins.GoogleAPIs.AdSenseManagement
{
    /// <summary>
    /// 구글 API 서비스 대리자
    /// </summary>
    internal class ApiService : GoogleAuth
    {
        readonly AdsenseService service;
        readonly int maxListPageSize = 50;
        Account adSenseAccount;

        /// <summary>
        /// <see cref="ApiService"/> 클래스의 새 인스턴스를 초기화합니다.
        /// </summary> 
        /// <param name="service">요청을 실행할 애드센스 서비스 개체입니다.</param>
        /// <param name="maxListPageSize">검색할 최대 페이지 크기입니다.</param>
        internal ApiService(UserCredential userCredential, AdsenseService service)
        {
            this.service = service;
            base.userCredential = userCredential;
        }
        /// <summary>
        /// 로그인한 사용자의 모든 계정을 가져오고 출력합니다.
        /// </summary>
        /// <returns>검색된 계정의 마지막 페이지입니다.</returns>
        internal IList<Account> GetAllAccounts()
        {
#if DEBUG
            Logger.Instance.LogMessage(TracingLevel.INFO, "전체 계정 쿼리 시작...");
#endif
            // 페이지에서 계정 목록을 검색하고 수신된 데이터를 표시합니다.
            string pageToken = null;
            ListAccountsResponse accountResponse = null;

            do
            {
                var accountRequest = service.Accounts.List();
                accountRequest.PageSize = maxListPageSize;
                accountRequest.PageToken = pageToken;
                accountResponse = accountRequest.Execute();
                pageToken = accountResponse.NextPageToken;
            } while (pageToken != null);
            Logger.Instance.LogMessage(TracingLevel.INFO, "Accounts List Request Successful");
            // 기본 샘플에서 실행할 항목이 있도록 계정의 마지막 페이지를 반환합니다.
            return accountResponse.Accounts;
        }
        /// <summary>
        /// 계정 이름을 가져옵니다.
        /// </summary>
        /// <returns></returns>
        internal string GetAccountName()
        {
            adSenseAccount = GetAllAccounts().NullToEmpty().FirstOrDefault();
            return adSenseAccount?.Name;
        }

        /// <summary>
        /// 지불 정보를 가져옵니다.
        /// </summary>
        /// <returns></returns>
        internal IList<Payment> RunCallPayment()
        {
            IList<Payment> payments = null;

            if (!Item.AccountName.IsNullOrEmpty())
            {
                var dataAsync = service.Accounts.Payments.List(Item.AccountName).Execute();
                payments = dataAsync.Payments;
            }

            return payments;
        }

        /// <summary>
        /// 날짜 범위, 측정 항목으로 보고서를 생성합니다.
        /// </summary>
        /// <param name="dateRangeEnum"></param>
        /// <param name="metricsEnum"></param>
        /// <returns></returns>
        internal ReportResult RunCallReport(DateRangeEnum dateRangeEnum, MetricsEnum metricsEnum)
        {
            ReportResult reportResult = null;

            if (!Item.AccountName.IsNullOrEmpty())
            {
                var report = service.Accounts.Reports.Generate(Item.AccountName);

                report.DateRange = dateRangeEnum;
                report.Metrics = metricsEnum;

                var result = report.Execute();

                if (result.TotalMatchedRows > 0)
                {
                    reportResult = result;
                }
            }

            return reportResult;
        }

        /// <summary>
        /// 날짜 범위, 측정 항목, 치수 단위로 보고서를 생성합니다.
        /// </summary>
        /// <param name="dateRangeEnum"></param>
        /// <param name="metricsEnum"></param>
        /// <returns></returns>
        internal ReportResult RunCallReport(DateRangeEnum dateRangeEnum, MetricsEnum metricsEnum, DimensionsEnum dimensionsEnum)
        {
            ReportResult reportResult = null;

            if (!Item.AccountName.IsNullOrEmpty())
            {
                var report = service.Accounts.Reports.Generate(Item.AccountName);

                report.DateRange = dateRangeEnum;
                report.Metrics = metricsEnum;
                report.Dimensions = dimensionsEnum;

                var result = report.Execute();

                if (result.TotalMatchedRows > 0)
                {
                    reportResult = result;
                }
            }

            return reportResult;
        }
    }
}