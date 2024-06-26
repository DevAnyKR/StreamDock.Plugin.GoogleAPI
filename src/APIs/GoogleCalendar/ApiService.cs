﻿using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;

namespace StreamDock.Plugin.GoogleAPI.GoogleCalendar
{
    internal class ApiService : GoogleAuth
    {
        CalendarService service;
        /// <summary>
        /// <see cref="ApiService"/> 클래스의 새 인스턴스를 초기화합니다.
        /// </summary> 
        /// <param name="service">요청을 실행할 애드센스 서비스 개체입니다.</param>
        /// <param name="maxListPageSize">검색할 최대 페이지 크기입니다.</param>
        ApiService()
        {
        }
        internal static async Task<ApiService> GetInstance(UserCredential userCredential, string user)
        {
            var instance = new ApiService();
            instance.userCredential = userCredential;
            instance.service = new CalendarService(await instance.GetServiceInitializerAsync(user));
            return instance;
        }

        /// <summary>
        /// 계정의 모든 캘린더를 가져옵니다.
        /// </summary>
        /// <returns>검색된 계정의 마지막 페이지입니다.</returns>
        internal IList<CalendarListEntry> GetAllCalendars()
        {
            string pageToken = null;
            CalendarList calendarList = null;

            do
            {
                var calendarListRequest = service.CalendarList.List();
                calendarListRequest.PageToken = pageToken;
                calendarList = calendarListRequest.Execute();
                pageToken = calendarList.NextPageToken;
            } while (pageToken != null);
            return calendarList.Items;
        }
        /// <summary>
        /// 단일 캘린더를 가져옵니다.
        /// </summary>
        /// <returns></returns>
        internal CalendarListEntry GetCalendar(string summary)
        {
            return GetAllCalendars().First(s => s.Summary == summary);
        }
        /// <summary>
        /// 단일 캘린더를 가져옵니다.
        /// </summary>
        /// <returns></returns>
        internal CalendarListEntry GetPrimaryCalendar()
        {
            return GetAllCalendars().First(s => s.Primary == true);
        }
        /// <summary>
        /// 오늘 이벤트를 가져옵니다.
        /// </summary>
        /// <returns></returns>
        internal async Task<Events> CalendarEventsToday(string id)
        {
            var requeust = service.Events.List(id);

            requeust.MaxResults = 5;
            requeust.TimeMinDateTimeOffset = DateTime.Today;
            requeust.TimeMaxDateTimeOffset = DateTime.Today.AddDays(1).AddSeconds(-1);

            var result = await requeust.ExecuteAsync();

            return result;
        }
    }
}