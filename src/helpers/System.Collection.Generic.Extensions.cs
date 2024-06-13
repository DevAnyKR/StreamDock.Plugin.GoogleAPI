namespace System.Collections.Generic
{
    public static class Extensions
    {
		public static IList<T> NullToEmpty<T>(this IList<T> list)
		{
			return list ?? new List<T>();
		}
		public static bool IsNullOrEmpty<T>(this IList<T> list)
		{
			return list == null || list.Count == 0;
		}
	}
}
