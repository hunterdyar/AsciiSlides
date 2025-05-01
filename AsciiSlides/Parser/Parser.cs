using System.Collections;
using Superpower;

namespace AsciiSlides.Parser;

public static class PresentationParser
{ 
	public static Presentation Parse(string source)
	{
		var tokenizer = new Tokenizer(source);
		return ParsePresentation(tokenizer.Tokens);
	}

	private static Presentation ParsePresentation(List<Token> tokens)
	{
		Presentation presentation = new Presentation();
		var t = new Queue<Token>(tokens);
		presentation.Frontmatter = ParseFrontmatter(ref t);
		
		var slides = new List<Slide>();
		while (t.Count > 0)
		{
			slides.Add(ParseSlide(ref t));
		}
		presentation.Slides = slides.ToArray();
		
		return presentation;
	}

	private static Slide ParseSlide(ref Queue<Token> tokens)
	{
		var startSlide = tokens.Dequeue();
		if (startSlide.Type != TokenType.StartSlide)
		{
			throw new Exception("Expected start slide.");
		}
		//optional delimiter? or does that even get emitted?
		Frontmatter f = ParseFrontmatter(ref tokens);
		var endFront = tokens.Dequeue();
		if (endFront.Type != TokenType.EndFrontmatter)
		{
			throw new Exception("Expected end front matter.");
		}
		var body = tokens.Dequeue();
		if (body.Type != TokenType.SlideBody)
		{
			throw new Exception("Expected slide body.");
		}
		return new Slide(f, body.Source);
	}

	private static Frontmatter ParseFrontmatter(ref Queue<Token> tokens)
	{
		var frontmatter = new Frontmatter();
		while (tokens.Peek().Type == TokenType.Ident)
		{
			var key = tokens.Dequeue();
			var value = tokens.Dequeue();
			if (value.Type != TokenType.Ident)
			{
				throw new Exception("Expected ident in frontmatter.");
			}
			var linebreak = tokens.Dequeue();
			frontmatter.AddKeyValuePair(key.Source,value.Source);
		}
		return frontmatter;
	}
}