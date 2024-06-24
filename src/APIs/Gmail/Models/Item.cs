namespace StreamDock.Plugin.GoogleAPI.Gmail
{
    /// <summary>
    /// API 데이터를 보관하는 클래스입니다. static은 스트림독의 모든 키에 공유됩니다.
    /// 데이터 형식은 API에서 제공하는 데이터 클래스를 사용하세요.
    /// </summary>
    public class Item
    {
        internal string UserId { get; set; }
        internal IList<Google.Apis.Gmail.v1.Data.Message> Messages { get; set; }
        internal long? MessageUnReadCount { get; set; }
        internal bool MessageExists => !Messages.IsNullOrEmpty() && Messages.Any();

        internal IList<string> DisplayValues { get; set; } = new List<string>();

        internal void Init()
        {
            UserId = string.Empty;
            Messages?.Clear();
            MessageUnReadCount = null;
            DisplayValues?.Clear();
        }
    }
}
