namespace AsciiSlides.Parser;

public enum TokenType
{
	ASCII,
	PropertyKey,
	PropertyValue,
	Delimiter,
	StartSlide,
	EndFrontmatter,
	
}
public struct Token(TokenType tokenType, string value, int line, int column)
{
	TokenType type;
	string value;
	int line;
	int column;
}