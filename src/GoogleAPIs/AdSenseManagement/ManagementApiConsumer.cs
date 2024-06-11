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
    public class ManagementApiConsumer
    {
        private AdsenseService service;
        private int maxListPageSize;
        private Account adSenseAccount;

        /// <summary><see cref="ManagementApiConsumer"/> 클래스의 새 인스턴스를 초기화합니다.</summary> 
        /// <param name="service">요청을 실행할 애드센스 서비스 개체입니다.</param>
        /// <param name="maxListPageSize">검색할 최대 페이지 크기입니다.</param>
        public ManagementApiConsumer(AdsenseService service, int maxListPageSize)
        {
            this.service = service;
            this.maxListPageSize = maxListPageSize;
        }

        /// <summary>애드센스 관리 API에 대해 여러 게시자 요청을 실행합니다.</summary>
        public string RunCallPayment()
        {
            Logger.Instance.LogMessage(TracingLevel.INFO, "전체 계정 쿼리 시작...");
            IList<Account> accounts = GetAllAccounts();
            Logger.Instance.LogMessage(TracingLevel.INFO, "전체 계정 쿼리 완료...");

            string data = string.Empty;

            adSenseAccount = accounts.NullToEmpty().FirstOrDefault();
            if (adSenseAccount != null)
            {
                var payments = service.Accounts.Payments.List(adSenseAccount.Name).Execute();

                //Todo 코드 검증 필요.
                if (payments.Payments.Any())
                {
                    data = payments.Payments.First().Amount;
                }
                else
                {
                    data = "정보 없음";
                    Logger.Instance.LogMessage(TracingLevel.WARN, "정보 없음");
                }
            }
            else
            {
                data = "계정 없음";
                Logger.Instance.LogMessage(TracingLevel.WARN, "계정 없음");
            }

            return data;
        }

        /// <summary>로그인한 사용자의 모든 계정을 가져오고 출력합니다.</summary>
        /// <returns>검색된 계정의 마지막 페이지입니다.</returns>
        private IList<Account> GetAllAccounts()
        {
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

            // 기본 샘플에서 실행할 항목이 있도록 계정의 마지막 페이지를 반환합니다.
            return accountResponse.Accounts;
        }
    }
}
