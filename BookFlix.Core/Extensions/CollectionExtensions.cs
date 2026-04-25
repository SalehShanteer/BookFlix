namespace BookFlix.Core.Extensions
{
    public static class CollectionExtensions
    {
        public static bool HasAny<T>(this ICollection<T> collection)
        {
            if (collection == null) return false;

            return collection.Count != 0;
        }
        public static bool IsEmpty<T>(this ICollection<T> collection)
        {
            return collection == null || collection.Count == 0;
        }

        public static bool IsEmpty<T>(this IReadOnlyCollection<T> collection)
        {
            return collection == null || collection.Count == 0;
        }
    }
}
