using Eto.Mac.Forms.Cells;
using Superpower.Parsers;

namespace AsciiSlides.Parser;

public enum TokenType
{
	ASCII,
	StartSlide,
	EndFrontmatter,
	CustomDelim,
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
		return Type.ToString();
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
		}
		else
		{
			//the body is from here to the index, then we sometimes consume the delim? if it's custom?
			_tokens.Add(new Token(TokenType.SlideBody, _source.Substring(_position, l - end)));
			_position += end;
		}
	}

	private void TokenizeOptionalFrontmatter()
	{
		while (_position < _source.Length)
		{
			ConsumeWhitespace(true);
			var next = _source[_position];
			if (!char.IsLetter(next))
			{
				break;
			}
			TokenizeIdentifier();
			ConsumeWhitespace(false);
			TokenizeIdentifier();
		}
		ConsumeWhitespace(true);
	}

	private void TokenizeIdentifier()
	{
		var p = _position;
		while (char.IsAsciiLetterOrDigit(current))
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

	private void TokenizeStartSlide()
	{
		var p = _position;
		ConsumeWhitespace();
		Consume('#');
		Consume('#');
		Consume('#');
		_tokens.Add(new Token(TokenType.StartSlide, _source.Substring(p,3)));
		TokenizeCustomDelimOptional();
	}

	private void TokenizeEndFrontmatter()
	{
		ConsumeWhitespace(false);
		var p = _position;
		Consume('-');
		Consume('-');
		Consume('-');
		_tokens.Add(new Token(TokenType.EndFrontmatter, _source.Substring(p, 3)));
		TokenizeCustomDelimOptional();
		ConsumeLinebreak();
	}

	private void ConsumeLinebreak()
	{
		ConsumeWhitespace(false);
		if (current == '\r')
		{
			Next();
		}
		Consume('\n');
	}

	void TokenizeCustomDelimOptional()
	{
		if (current != '-' && current != '#' && !char.IsWhiteSpace(current) && current != '\0')
		{
			TokenizeIdentifier();
			_tokens[^1].Type = TokenType.CustomDelim;
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