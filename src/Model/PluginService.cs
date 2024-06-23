using System;

namespace StreamDock.Plugins
{
    /// <summary>
    /// 플러그인의 동작을 보조하는 클래스
    /// </summary>
    internal class PluginService
    {
        /// <summary>
        /// 최근 갱신 시각
        /// </summary>
        internal DateTime LatestRefreshTime { get; private set; }
        /// <summary>
        /// 처음 실행되었
        /// </summary>
        internal bool HasExecuteOnce { get; private set; }
        /// <summary>
        /// 갱신 시각을 업데이트 합니다.
        /// </summary>
        internal void UpdateRefreshTime()
        {
            LatestRefreshTime = DateTime.Now;
        }
        /// <summary>
        /// 처음 실행되었음을 설정합니다.
        /// </summary>
        internal void SetExecuted()
        {
            HasExecuteOnce = true;
        }
        internal bool IsRefreshable(TimeSpan timespan)
        {
            if (timespan.TotalSeconds > 0 && DateTime.Now.Subtract(LatestRefreshTime).CompareTo(timespan) >= 0)
            {
                UpdateRefreshTime();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
