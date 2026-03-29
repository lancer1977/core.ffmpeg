namespace Core.FFmpeg.Tests;

public static class Assert
{
    public static void NotNull(object? value)
    {
        if (value is null) throw new Exception("Expected value to be non-null.");
    }

    public static void True(bool condition, string? message = null)
    {
        if (!condition) throw new Exception(message ?? "Expected condition to be true.");
    }

    public static void Equal<T>(T expected, T actual)
    {
        if (!EqualityComparer<T>.Default.Equals(expected, actual))
            throw new Exception($"Expected '{expected}' but got '{actual}'.");
    }

    public static void Contains(string expected, IReadOnlyCollection<string> values)
    {
        if (!values.Contains(expected)) throw new Exception($"Expected collection to contain '{expected}'.");
    }

    public static void Contains(string expected, string value)
    {
        if (!value.Contains(expected, StringComparison.Ordinal)) throw new Exception($"Expected string to contain '{expected}'.");
    }

    public static void DoesNotContain(string unexpected, IReadOnlyCollection<string> values)
    {
        if (values.Contains(unexpected)) throw new Exception($"Expected collection not to contain '{unexpected}'.");
    }

    public static async Task ThrowsAsync<T>(Func<Task> action) where T : Exception
    {
        try
        {
            await action();
        }
        catch (Exception ex)
        {
            if (ex is T) return;
            throw new Exception($"Expected exception of type {typeof(T).Name}, got {ex.GetType().Name}.");
        }

        throw new Exception($"Expected exception of type {typeof(T).Name}, but no exception was thrown.");
    }
}
