using System.Text.RegularExpressions;

namespace AsciiSlidesCore.Parser;

public enum TokenType
{
	ASCII,
	StartSlide,
	EndFrontmatter,
	Delimiter,
	Ident,
	Linebreak,
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
		var s = Regex.Escape(Source);
		return Type.ToString() + "("+s+")";
	}
}
public class Tokenizer
{
	private enum TState
	{
		Initial
	}
	public List<Token> Tokens => _tokens;
	private List<Token> _tokens = new List<Token>();
	private int _position = 0;
	private char current = '\0';
	private char PosCurrent => _source[_position];
	
	private string _source;
	private TState _state = TState.Initial;
	public Tokenizer(string source)
	{
		_source = source;
		if (_position < _source.Length)
		{
			current = _source[_position];
			
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
			current = '\0';
		}
		else
		{
			//the body is from here to the index, then we sometimes consume the delim? if it's custom?
			_tokens.Add(new Token(TokenType.SlideBody, _source.Substring(_position, end)));
			_position += end;
			current = _source[_position];
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
			TokenizeIdentifier();
			TokenizeLinebreak();
		}
		ConsumeWhitespace(true);
	}

	private static readonly char[] IdentifierPermitted = ['_', '#', '@', '$', '%', '&', '+','%','.','^','(',')','{','}','[',']'];

	private void TokenizeIdentifier()
	{
		var p = _position;
		while (char.IsAsciiLetterOrDigit(current) || IdentifierPermitted.Contains(current))
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

	public void Next()
	{
		_position++;
		if (_position >= _source.Length)
		{
			current = '\0';
		}
		else
		{
			current = _source[_position];
		}
	}

	private void ConsumeWhitespace(bool consumeLineBreaks = false)
	{
		if (consumeLineBreaks)
		{
			while (char.IsWhiteSpace(_source, _position))
			{
				Next();
			}
		}
		else
		{
			if (current == ' ' || current == '\t' || current == '\u00A0' || current == '\v' || current == '\f')
			{
				Next();
			}
		}
	}

	public void string ConsumeTokenWithOptionalCustomDelimiter()
	{
		if (current == '"')
		{
			//consume normal string.... emit that.
		}
		//how to decide what the custom delim is? Non-letterNumber is first guess.
		//we would tokenize the delim!
		string delimiter = "\n";
		//consume up to closing delimiter.
		if (IsOpeningSymbol(current))
		{
			//instead of going up to the newline, we will go to the delim.
			delimiter = current.ToString();
			Next();
			while (!char.IsWhiteSpace(current))
			{
				delimiter += current;
				Next();
			}
			_tokens.Add(new Token(TokenType.Delimiter, delimiter));
		}

		
		int closeDelimiter = _source.Substring(_position).IndexOf(delimiterStarter, StringComparison.Ordinal);
		if (closeDelimiter < 0)
		{
			//"asdf" will fail, " adf" will succeed.
			throw new Exception("Unable to find close for opening delimiter: " + delimiter);
		}
		
	}

	private bool IsOpeningSymbol(char c)
	{
		var openings = new char[]{'[', '{', '(', '<'};
		return openings.Contains(c);
	}

	private void TokenizeStartSlide()
	{
		ConsumeWhitespace();
		Consume('#');
		Consume('#');
		Consume('#');
		_tokens.Add(new Token(TokenType.StartSlide, _source.Substring(_position-3,3)));
		TokenizeCustomDelimOptional();
		
		//###--- is allowed. it's a shorthand.
		if (current != '-')
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
		if (current == '\r')
		{
			Next();
		}
		
		//line break and end of file are the same. 
		if (current != '\0')
		{
			Consume('\n');
		}
	}

	void TokenizeCustomDelimOptional()
	{
		if (current != '-' && current != '#' && !char.IsWhiteSpace(current) && current != '\0')
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

	void TokenizeLinebreak()
	{
		ConsumeWhitespace(false);
		if (current == '\r')
		{
			Next();
		}

		if (current == '\n')
		{
			_tokens.Add(new Token(TokenType.Linebreak, _source.Substring(_position,1)));
			Next();
		}
	}
	
	public void Consume(char c)
	{
		if (_position >= _source.Length)
		{
			throw new Exception("Unexpected end of file.");
		}

		if (current == c)
		{
			Next();
		}
	}
	

}