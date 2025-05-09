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

	public static bool IsLeftBrace(this char c, out char rightChar)
	{
		switch (c)
		{
			case '{':
				rightChar = '}';
				return true;
			case '<':
				rightChar = '>';
				return true;
			case '(':
				rightChar = ')';
				return true;
			case '[':
				rightChar = ']';
				return true;
		}

		rightChar = c;
		return false;
	}

	public static bool IsSymmetricDelimiter(this char c)
	{
		return c == '-' 
		       || c == '.'
		       || c == '_' 
		       || c == '+' 
		       || c == '=' 
		       || c == '^' 
		       || c == '#'
		       || c == '|'
		       || c == '\''
		       || c == '\"'
		       || c == '`'
		       || c == '~'
			;
	}

	public static string Reverse(this string source)
	{
		char[] chars = source.ToCharArray();
		Array.Reverse(chars);
		return new string(chars);
	}
}