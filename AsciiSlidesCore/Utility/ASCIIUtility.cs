namespace AsciiSlidesCore;

public static class ASCIIUtility
{
	public static (int, int) GetRowColCount(this string source)
	{
		string[] split = source.Split('\n');
		int row = split.Length - 1;
		int col = split.Max(x=>x.Length);
		return (row, col);
	}
}