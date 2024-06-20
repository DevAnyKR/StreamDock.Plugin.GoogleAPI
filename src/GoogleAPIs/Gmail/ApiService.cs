using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BarRaider.SdTools;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;

namespace StreamDock.Plugins.GoogleAPIs.Gmail
{
    /// <summary>
    /// 구글 API 서비스 대리자
    /// </summary>
    internal class ApiService : GoogleAuth
    {
        readonly GmailService service;

        /// <summary>
        /// <see cref="ApiService"/> 클래스의 새 인스턴스를 초기화합니다.
        /// </summary> 
        /// <param name="service">요청을 실행할 애드센스 서비스 개체입니다.</param>
        /// <param name="maxListPageSize">검색할 최대 페이지 크기입니다.</param>
        internal ApiService(UserCredential userCredential, GmailService service)
        {
            this.service = service;
            base.userCredential = userCredential;
        }
        internal UsersResource.MessagesResource.ListRequest GetMessagesListAsync(string userId)
        {
            return service.Users.Messages.List(userId);
        }
        internal async Task<ListMessagesResponse> GetMessagesListAsync(string userId, string labelIds)
        {
            var listRequest = GetMessagesListAsync(userId);
            listRequest.LabelIds = labelIds;
            return await listRequest.ExecuteAsync();
        }
    }
}