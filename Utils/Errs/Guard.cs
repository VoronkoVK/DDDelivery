namespace Errs;

public static class Guard
{
    private static readonly Guid EmptyGuid = Guid.Empty;

    // Object
    public static bool IsNull(object obj)
    {
        return obj == null;
    }

    // String
    public static bool IsNullOrEmpty(string value)
    {
        return string.IsNullOrWhiteSpace(value);
    }

    // Guid
    public static bool IsNullOrEmpty(Guid? guid)
    {
        return !guid.HasValue || guid == EmptyGuid;
    }

    // Collection
    public static bool IsNullOrEmpty<T>(ICollection<T> collection)
    {
        return collection == null || collection.Count == 0;
    }
}