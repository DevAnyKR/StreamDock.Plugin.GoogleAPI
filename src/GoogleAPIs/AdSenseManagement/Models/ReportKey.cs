namespace StreamDock.Plugins.GoogleAPIs.GoogleAPIs.AdSenseManagement
{
    interface IReportKey
    {
    }

    internal struct ReportKey<T1, T2> : IReportKey
    {
        internal readonly T1 Item1;
        internal readonly T2 Item2;
        internal ReportKey(T1 item1, T2 item2)
        {
            this.Item1 = item1;
            this.Item2 = item2;
        }
    }
    internal struct ReportKey<T1, T2, T3> : IReportKey
    {
        internal readonly T1 Item1;
        internal readonly T2 Item2;
        internal readonly T3 Item3;

        internal ReportKey(T1 item1, T2 item2, T3 item3)
        {
            this.Item1 = item1;
            this.Item2 = item2;
            this.Item3 = item3;
        }
    }
    internal static class ReportKey
    {
        internal static ReportKey<T1, T2> Create<T1,T2>(T1 item1, T2 item2)
        {
            return new ReportKey<T1,T2>(item1, item2);
        }
        internal static ReportKey<T1, T2,T3> Create<T1, T2,T3>(T1 item1, T2 item2, T3 item3)
        {
            return new ReportKey<T1, T2, T3>(item1, item2, item3);
        }
    }
}