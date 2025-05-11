using System.Text.RegularExpressions;

namespace AsciiSlidesCore.Parser;

public enum TokenType
{
	SlideSep,
	Key,
	Value
}

public class Token 
{
	public TokenType Type { get; set; }
	public string Source { get; set; }
	public Token(TokenType type, ReadOnlySpan<char> source)
	{
		Type = type;
		Source = source.ToString();
	}

	public override string ToString()
	{
		string s = Regex.Escape(Source.ToString());
		return Type.ToString() + "("+s+")";
	}
}
public ref struct Tokenizer
{
	public List<Token> Tokens => _tokens;

	private readonly List<Token> _tokens = new List<Token>();
	private int _position = 0;
	private char _current = '\0';
	public ReadOnlySpan<char> Source => _source;
	private ReadOnlySpan<char> _source;
	public Tokenizer(string source)
	{
		_source = source.AsSpan();	
		if (_position < _source.Length)
		{
			_current = _source[_position];
			TokenizeKeyValuePairs();
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
		TokenizeKeyValuePairs();
	}

	private void TokenizeKeyValuePairs()
	{
		while (_position < _source.Length)
		{
			ConsumeWhitespace(true);
			var next = _source[_position];
			if (!char.IsLetter(next))
			{
				break;
			}
			TokenizeKey();
			ConsumeWhitespace(false);
			Consume(':');
			ConsumeWhitespace(false);
			TokenizeValueDelimited();
			ConsumeWhitespace(true);
		}
		ConsumeWhitespace(true);
	}

	private static readonly char[] IdentifierPermitted = ['_', '#', '@', '$', '%', '&', '+','%','.','^','(',')','{','}','[',']'];

	private void TokenizeKey()
	{
		int p = _position;
		while (char.IsAsciiLetterOrDigit(_current) || IdentifierPermitted.Contains(_current) || _current == '_'|| _current == '-')
		{
			Next();
		}

		if (_position > p)
		{
			_tokens.Add(new Token(TokenType.Key, _source.Slice(p,  _position-p)));
		}
		else
		{
			throw new Exception($"Unexpected character at position {_position}. Want identifier.");
		}
	}

	private void Next()
	{
		_position++;
		if (_position >= _source.Length || _position < 0)
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
			while (char.IsWhiteSpace(_source[_position]))
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

	private void TokenizeValueDelimited()
	{
		//Tokenize Value String-Style Delimited.
		//todo: refactor into multi-quote parsing that doesn't use whitespace.
		if (_current == '"')
		{
			//todo: make readonly span
			string s = "";
			Next();
			bool escaping = false;
			while (_current != '"' || escaping)
			{
				if (_current == '\0')
				{
					throw new Exception("Unexpected end of file. a \" was never closed.");
				}
				escaping = _current == '\\';
				s+= _current;
				Next();
			}
			Next();//eat the closing "
			_tokens.Add(new Token(TokenType.Value, s));
			return;
		}else if (_current == '\'')
		{
			//todo: make readonly span
			string s = "";
			Next();
			bool escaping = false;
			while (_current != '\''|| escaping)
			{
				if (_current == '\0')
				{
					throw new Exception("Unexpected end of file. a ' was never closed.");
				}
				escaping = _current == '\\';
				s+= _current;
				Next();
			}
			Next();//eat the closing "
			_tokens.Add(new Token(TokenType.Value, s));
			return;
		}
		
		//how to decide what the custom delim is? Non-letterNumber is first guess.
		//we would tokenize the delim!
		//consume up to the next whitespace
		int openStart = _position;
		while (!char.IsWhiteSpace(_current))
		{
			Next();
		}

		//is this a delimiter or is this the value?
		
		//Find and consume the opening.
		var opening = _source.Slice(openStart, _position - openStart);
		var d = new Delimiter(ref opening);
		
		//consume up to the closing.
		var closingStartIndex = _source.Slice(_position).IndexOf(d.Close);
		if (closingStartIndex >= 0)
		{
			var body = _source.Slice(_position, closingStartIndex);
			_position += body.Length + d.Close.Length;
			_tokens.Add(new Token(TokenType.Value, body));
		}
		else
		{
			throw new Exception($"No Closing Delimiter {d.Close} found for {d.Open}");
		}
	}

	private string ReverseOpeningSymbols(string delimiter)
	{
		return delimiter.Replace('[',']').Replace('{','}').Replace('(',')').Replace('<','>');
	}

	private void TokenizeStartSlide()
	{
		ConsumeWhitespace(false);
		Consume('#');
		Consume('#');
		Consume('#');
		_tokens.Add(new Token(TokenType.SlideSep, _source.Slice(_position-3,3)));
		
		//###--- is allowed. it's a shorthand.
		if (_current != '-')
		{
			ConsumeWhitespace(true);
		}
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
		else
		{
			throw new Exception($"Unexpected character ({_current}) at position {_position}. Expected {c}");
		}
	}

	public void SetPosition(int newPos)
	{
		if (newPos > _position)
		{
			if (newPos < _source.Length)
			{
				_position = newPos;
				return;
			}
			else
			{
				throw new Exception("Unexpected end of file");
			}
		}

		throw new Exception("Can't rewind the tokenizer.");
	}
}