namespace AsciiSlidesCore.Parser;

public class BadDelimiterException : Exception
{
    public BadDelimiterException(string delim, string message) : base($"Bad delimiter: {delim}. {message}")
    {
    }
}