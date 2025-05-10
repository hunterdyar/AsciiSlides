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
		var presentation = new Presentation();
		var t = new Queue<Token>(tokens);
		presentation.Frontmatter = ParseFrontmatter(ref t, null);
		
		var slides = new List<Slide>();
		while (t.Count > 0)
		{
			slides.Add(ParseSlide(ref t, ref presentation, slides.Count+1,presentation.Frontmatter));
		}
		//end of slideshow slide.
		slides.Add(new BlankSlide(presentation, "end of presentation"));
		presentation.Slides = slides.ToArray();
		
		return presentation;
	}

	private static Slide ParseSlide(ref Queue<Token> tokens, ref Presentation presentation, int slideNumber,
		Frontmatter defaultFrontmatter)
	{
		var startSlide = tokens.Dequeue();
		if (startSlide.Type != TokenType.SlideSep)
		{
			throw new Exception("Expected start slide.");
		}
		//optional delimiter? or does that even get emitted?
		Frontmatter f = ParseFrontmatter(ref tokens, defaultFrontmatter);
		return SlideFactory.CreateSlide(presentation, f, slideNumber);
	}

	private static Frontmatter ParseFrontmatter(ref Queue<Token> tokens, Frontmatter? parentFrontmatter = null)
	{
		var frontmatter = new Frontmatter();
		while (tokens.Count > 0 && tokens.Peek().Type == TokenType.Key)
		{
			var key = tokens.Dequeue();
			var value = tokens.Dequeue();
			if (value.Type != TokenType.Value)
			{
				throw new Exception($"Expected Value, got {value.ToString()}");
			}
			frontmatter.AddKeyValuePair(NormalizeKey(key.Source),value.Source);
		}

		frontmatter.SetParentFrontmatter(parentFrontmatter);
		return frontmatter;
	}

	private static string NormalizeKey(string key)
	{
		key = key.ToLower().Trim();
		switch (key)
		{
			case "md":
			case "mark":
			case "markdown":
			case "richtext":
				return "markdown";
			case "ascii":
			case "text":
			case "plaintext":
			case "a":
				return "ascii";
			case "html":
			case "htm":
			case "source":
				return "html";
			case "style":
			case "css":
				return "style";
			case "youtube":
			case "yt":
				return "youtube";
			case "web":
			case "link":
				return "web";
		}

		return key;
	}
}