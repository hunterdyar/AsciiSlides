using System.Collections;
namespace AsciiSlidesCore.Parser;

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
		presentation.Frontmatter = ParseFrontmatter(ref t, null);
		
		var slides = new List<Slide>();
		while (t.Count > 0)
		{
			slides.Add(ParseSlide(ref t, ref presentation, slides.Count+1,presentation.Frontmatter));
		}
		presentation.Slides = slides.ToArray();
		
		return presentation;
	}

	private static Slide ParseSlide(ref Queue<Token> tokens, ref Presentation presentation, int slideNumber,
		Frontmatter defaultFrontmatter)
	{
		var startSlide = tokens.Dequeue();
		if (startSlide.Type != TokenType.StartSlide)
		{
			throw new Exception("Expected start slide.");
		}
		//optional delimiter? or does that even get emitted?
		Frontmatter f = ParseFrontmatter(ref tokens, defaultFrontmatter);
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
		return SlideFactory.CreateSlide(presentation, f, body.Source, slideNumber);
	}

	private static Frontmatter ParseFrontmatter(ref Queue<Token> tokens, Frontmatter? parentFrontmatter = null)
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
			frontmatter.AddKeyValuePair(key.Source,value.Source);
		}

		frontmatter.SetParentFrontmatter(parentFrontmatter);
		return frontmatter;
	}
}