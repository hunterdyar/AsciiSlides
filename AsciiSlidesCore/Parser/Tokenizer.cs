using System.Text.RegularExpressions;

namespace AsciiSlidesCore.Parser;

public enum TokenType
{
	StartSlide,
	EndFrontmatter,
	Delimiter,
	Ident,
	SlideBody
}
public class Token 
{
	public TokenType Type { get; set; }
	public string Source { get; set; }
	public Token(TokenType type, string source)
	{
		Type = type;
		Source = source;
	}

	public override string ToString()
	{
		string s = Regex.Escape(Source);
		return Type.ToString() + "("+s+")";
	}
}
public class Tokenizer
{
	private static readonly char[] OpeningBraceCharacters = ['[', '{', '(', '<'];
	
	public List<Token> Tokens => _tokens;
	private readonly List<Token> _tokens = new List<Token>();
	private int _position = 0;
	private char _current = '\0';
	private char PosCurrent => _source[_position];
	
	private readonly string _source;
	public Tokenizer(string source)
	{
		_source = source;
		if (_position < _source.Length)
		{
			_current = _source[_position];
			
			TokenizeOptionalFrontmatter();
			while (_position < _source.Length)
			{
				TokenizeSlide();
			}
		}
		else
		{
			//empty string! we're done!
			return;
		}
	}

	private void TokenizeSlide()
	{
		TokenizeStartSlide();
		TokenizeOptionalFrontmatter();
		TokenizeEndFrontmatter();
		ConsumeSlideBody();
	}

	private void ConsumeSlideBody()
	{
		//if using custom delimiter, findNext
		int l = _source.Length - _position;
		if (l == 0)
		{
			//end of file, but it's okay.
			return;
		}
		var end = _source.Substring(_position,l).IndexOf("###", StringComparison.Ordinal);
		if (end <0)
		{
			//the slide is from here to the end of the file.
			_tokens.Add(new Token(TokenType.SlideBody, _source.Substring(_position)));
			_position = _source.Length;
			_current = '\0';
		}
		else
		{
			//the body is from here to the index, then we sometimes consume the delim? if it's custom?
			_tokens.Add(new Token(TokenType.SlideBody, _source.Substring(_position, end)));
			_position += end;
			_current = _source[_position];
		}
	}

	private void TokenizeOptionalFrontmatter()
	{
		while (_position < _source.Length)
		{
			ConsumeWhitespace(false);
			var next = _source[_position];
			if (!char.IsLetter(next))
			{
				break;
			}
			TokenizeIdentifier();
			ConsumeWhitespace(false);
			Consume(':');
			ConsumeWhitespace(false);
			TokenizeIdentifierValueDelimited();
			ConsumeWhitespace(true);
		}
		ConsumeWhitespace(true);
	}

	private static readonly char[] IdentifierPermitted = ['_', '#', '@', '$', '%', '&', '+','%','.','^','(',')','{','}','[',']'];

	private void TokenizeIdentifier()
	{
		int p = _position;
		while (char.IsAsciiLetterOrDigit(_current) || IdentifierPermitted.Contains(_current))
		{
			Next();
		}

		if (_position > p)
		{
			_tokens.Add(new Token(TokenType.Ident, _source.Substring(p,  _position-p)));
		}
		else
		{
			throw new Exception($"Unexpected character at position {_position}. Want identifier.");
		}
	}

	private void Next()
	{
		_position++;
		if (_position >= _source.Length)
		{
			_current = '\0';
		}
		else
		{
			_current = _source[_position];
		}
	}

	private void ConsumeWhitespace(bool consumeLineBreaks)
	{
		if (_position >= _source.Length)
		{
			return;
		}
		if (consumeLineBreaks)
		{
			while (char.IsWhiteSpace(_source, _position))
			{
				Next();
				if (_position >= _source.Length)
				{
					break;
				}
			}
		}
		else
		{
			if (_current == ' ' || _current == '\t' || _current == '\u00A0' || _current == '\v' || _current == '\f')
			{
				Next();
			}
		}
	}

	private void TokenizeIdentifierValueDelimited()
	{
		if (_current == '"')
		{
			string s = "";
			Next();
			bool escaping = false;
			while (_current != '"' || escaping)
			{
				escaping = _current == '\\';
				s+= _current;
				Next();
			}
			Next();//eat the closing "
			_tokens.Add(new Token(TokenType.Ident, s));
			return;
		}//else
		
		//how to decide what the custom delim is? Non-letterNumber is first guess.
		//we would tokenize the delim!
		string delimiter;
		//consume up to closing delimiter.
		if (IsOpeningSymbol(_current))
		{
			//instead of going up to the newline we will go to the delim.
			delimiter = _current.ToString();
			Next();
			while (!char.IsWhiteSpace(_current))
			{
				delimiter += _current;
				Next();
			}
		}
		else
		{
			TokenizeIdentifier();
			return;
		}

		delimiter = ReverseOpeningSymbols(delimiter);
		int closePos = _source.Substring(_position).IndexOf(delimiter, StringComparison.Ordinal);
		if (closePos >= 0)
		{
			_tokens.Add(new Token(TokenType.Ident,_source.Substring(_position, closePos)));
			_position += closePos+delimiter.Length;
			_current = _source[_position];//can't do Next()
		}
		else
		{
			//"asdf" will fail, "asdf" will succeed.
			throw new Exception("Unable to find close for opening delimiter: " + delimiter);
		}
		
	}

	private string ReverseOpeningSymbols(string delimiter)
	{
		return delimiter.Replace('[',']').Replace('{','}').Replace('(',')').Replace('<','>');
	}

	private bool IsOpeningSymbol(char c)
	{
		return OpeningBraceCharacters.Contains(c);
	}

	private void TokenizeStartSlide()
	{
		ConsumeWhitespace(false);
		Consume('#');
		Consume('#');
		Consume('#');
		_tokens.Add(new Token(TokenType.StartSlide, _source.Substring(_position-3,3)));
		TokenizeCustomDelimOptional();
		
		//###--- is allowed. it's a shorthand.
		if (_current != '-')
		{
			ConsumeWhitespace(true);
		}
	}

	private void TokenizeEndFrontmatter()
	{
		ConsumeWhitespace(false);
		Consume('-');
		Consume('-');
		Consume('-');
		_tokens.Add(new Token(TokenType.EndFrontmatter, _source.Substring(_position-3, 3)));
		TokenizeCustomDelimOptional();
		ConsumeWhitespace(false);
		ConsumeLinebreak();
	}

	private void ConsumeLinebreak()
	{
		ConsumeWhitespace(false);
		if (_current == '\r')
		{
			Next();
		}
		
		//line break and end of file are the same. 
		if (_current != '\0')
		{
			Consume('\n');
		}
	}

	private void TokenizeCustomDelimOptional()
	{
		if (_current != '-' && _current != '#' && !char.IsWhiteSpace(_current) && _current != '\0')
		{
			TokenizeIdentifier();
			if (_tokens[^1].Type == TokenType.Ident)
			{
				_tokens[^1].Type = TokenType.Delimiter;
			}
			else
			{
				//huh
			}
		}
	}

	private void Consume(char c)
	{
		if (_position >= _source.Length)
		{
			throw new Exception("Unexpected end of file.");
		}

		if (_current == c)
		{
			Next();
		}
	}
}