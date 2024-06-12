/*
Copyright 2021 Google Inc

Licensed under the Apache License, Version 2.0(the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

   http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System.Collections.Generic;
using System.Linq;

using BarRaider.SdTools;

using Google.Apis.Adsense.v2;
using Google.Apis.Adsense.v2.Data;

namespace StreamDock.Plugin.GoogleAPIs.AdSenseManagement
{
    internal class ManagementApiConsumer
    {
        private AdsenseService service;
        private int maxListPageSize;
        private Account adSenseAccount;

        /// <summary>
        /// <see cref="ManagementApiConsumer"/> 클래스의 새 인스턴스를 초기화합니다.
        /// </summary> 
        /// <param name="service">요청을 실행할 애드센스 서비스 개체입니다.</param>
        /// <param name="maxListPageSize">검색할 최대 페이지 크기입니다.</param>
        internal ManagementApiConsumer(AdsenseService service, int maxListPageSize)
        {
            this.service = service;
            this.maxListPageSize = maxListPageSize;
        }

        //TODO 계정이 여러개일 경우 PI로 전달 후 설정에 따라 쿼리할 것.
        /// <summary>
        /// 애드센스 관리 API에 대해 여러 게시자 요청을 실행합니다.
        /// </summary>
        internal Item RunCallPayment()
        {
            IList<Account> accounts = GetAllAccounts();

            Item.Instance.Init();

            adSenseAccount = accounts.NullToEmpty().FirstOrDefault();
            if (adSenseAccount != null)
            {
                //Item.Instance.Value2 = adSenseAccount.Name;
                var payments = service.Accounts.Payments.List(adSenseAccount.Name).Execute();

                if (payments.Payments.Any())
                {
                    Item.Instance.Value1 = payments.Payments.First().Amount;
                    Item.Instance.DataValid = true;
                }
                else
                {
                    Item.Instance.Value1 = "정보 없음";
                    Logger.Instance.LogMessage(TracingLevel.WARN, "정보 없음");
                }
            }
            else
            {
                Item.Instance.Value1 = "계정 없음";
                Logger.Instance.LogMessage(TracingLevel.WARN, "계정 없음");
            }
            Item.Instance.DataReceived = true;

            return Item.Instance;
        }

        internal Item RunCallIncomeToday()
        {
            Item.Instance.Init();
            return Item.Instance;
        }
        internal Item RunCallIncomeWeekly()
        {
            Item.Instance.Init();
            return Item.Instance;
        }

        internal Item RunCallIncomeMonthly()
        {
            Item.Instance.Init();
            return Item.Instance;
        }
        internal Item RunCallIncomeLast30Days()
        {
            Item.Instance.Init();
            return Item.Instance;
        }

        /// <summary>
        /// 로그인한 사용자의 모든 계정을 가져오고 출력합니다.
        /// </summary>
        /// <returns>검색된 계정의 마지막 페이지입니다.</returns>
        private IList<Account> GetAllAccounts()
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, "전체 계정 쿼리 시작...");

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

            Logger.Instance.LogMessage(TracingLevel.INFO, "전체 계정 쿼리 완료...");

            // 기본 샘플에서 실행할 항목이 있도록 계정의 마지막 페이지를 반환합니다.
            return accountResponse.Accounts;
        }
    }
}
