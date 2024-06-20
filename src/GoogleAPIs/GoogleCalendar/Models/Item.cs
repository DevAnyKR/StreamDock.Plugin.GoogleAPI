using System;
using System.Collections.Generic;
using System.Linq;

using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;

namespace StreamDock.Plugins.GoogleAPIs.GoogleCalendar
{
    /// <summary>
    /// API 데이터를 보관하는 클래스입니다. static은 스트림독의 모든 키에 공유됩니다.
    /// 데이터 형식은 API에서 제공하는 데이터 클래스를 사용하세요.
    /// </summary>
    public class Item
    {
        // 공유 데이터
        internal static IList<CalendarListEntry> Calendars { get; set; }

        // 개별 데이터
        internal CalendarsResource Calendar { get; set; }
        internal string calendarID { get; set; }
        internal Events Events { get; set; }
        internal bool CalendarExists => !Calendars.IsNullOrEmpty() && Calendars.Any();

        internal IList<string> DisplayValues { get; set; } = new List<string>();

        internal void Init()
        {
            Calendars?.Clear();
            calendarID = string.Empty;
            Events = null;
            DisplayValues?.Clear();
        }
    }
}
