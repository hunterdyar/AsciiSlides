using System.Text;

namespace AsciiSlides.Parser;


public class Parser
{
	public enum ParserState
	{
		Initial,//any state
		ASCIICustomDelim,
		ASCII,
		Frontmatter,
	}
	
	private ParserState _state = ParserState.Initial;
	private string _current = "";
	private IEnumerator<char> source;
	private int _column;
	private int _line;
	private List<Token> _tokens = new List<Token>();
	public void Parse(string filename)
	{
		source = GetFileChars(filename);
		while (source.MoveNext())
		{
			char peek = source.Current;
			switch (_state)
			{
				case ParserState.Initial:
					if (_current == "###")
					{
						Emit(TokenType.StartSlide, _current);
						_current = "";//consume
						_state = ParserState.Frontmatter;
						break;
					}
					else
					{
					//	string token = ConsumeUntilWhitespace();
					//	Emit();
					}
					break;
				//if this is a control character, switch to some state depending on the character, etc.
				case ParserState.Frontmatter:
					if (_current == "---")
					{
						Emit(TokenType.EndFrontmatter, _current);
						_current = "";
					}
					break;
				case ParserState.ASCII:
					//if this is not another character, break out.
					if (peek == '\n')
					{
						
					}
					break;
				default:
					break;
			}

			_current += peek;
		}
	}

	private void Emit(TokenType tokenType, string value)
	{
		_tokens.Add(new Token(tokenType,value, _line, _column));
	}
	public IEnumerator<char> GetFileChars(string filePath)
	{
		_line = 0;
		_column = 0;
		if (!File.Exists(filePath))
			throw new FileNotFoundException("The specified file was not found.", filePath);

		using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
		using (var reader = new StreamReader(fileStream, Encoding.UTF8))
		{
			string line;
			_column = 0;
			while ((line = reader.ReadLine() ?? string.Empty) != string.Empty)
			{
				foreach (var c in line)
				{
					yield return c;
					_column++;
				}
				_line++;
			}
		}
	}
}