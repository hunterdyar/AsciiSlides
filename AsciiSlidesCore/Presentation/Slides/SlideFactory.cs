namespace AsciiSlidesCore;

public static class SlideFactory
{
	public static Slide CreateSlide(Presentation presentation, Frontmatter frontmatter, int number, string defaultType="ascii")
	{
		
		Slide slide;
		//todo: create constants for key values that are not strings. use enum lookup as keys for frontmatter?
		if (frontmatter.TryGetKey("ascii", out var body))
		{
			slide = new ASCIISlide(presentation, body)
			{
				Frontmatter = frontmatter,
				SlideNumber = number,
			};
		}else if (frontmatter.TryGetKey("youtube", out var url))
		{
			slide = new YTSlide(presentation, url)
			{
				Frontmatter = frontmatter,
				SlideNumber = number
			};
		}else if (frontmatter.TryGetKey("markdown", out var markdown))
		{
			slide = new MarkdownSlide(presentation, markdown)
			{
				Frontmatter = frontmatter,
				SlideNumber = number
			};
		}else if (frontmatter.TryGetKey("html", out var html))
		{
			slide = new HTMLSlide(presentation, html)
			{
				Frontmatter = frontmatter,
				SlideNumber = number
			};
		}else if (frontmatter.TryGetKey("image", out var src))
		{
			slide = new ImageSlide(presentation, src)
			{
				Frontmatter = frontmatter,
				SlideNumber = number
			};
		}else if (frontmatter.TryGetKey("web", out var externalURL))
		{
			slide = new WebSlide(presentation, externalURL)
			{
				Frontmatter = frontmatter,
				SlideNumber = number
			};
			Console.WriteLine("Creating web view, but it's slow and buggy.");
		}else if (frontmatter.TryGetKey("atext", out var texty))
		{
			slide = new FIGletSlide(presentation, texty)
			{
				Frontmatter = frontmatter,
				SlideNumber = number
			};
		}
		else
		{
			slide = new BlankSlide(presentation, "")
			{
				Frontmatter = frontmatter,
				SlideNumber = number
			};
		}
		
		bool hasSpeaker;
		//Set Speaker Notes
		if (frontmatter.TryGetKey("notes", out string speakerNotes))
		{
			hasSpeaker = true;
		}
		else
		{
			hasSpeaker = false;
			//SpeakerNotes will be null, but we can surpress the compiler warning because this branch handles that case
			speakerNotes = string.Empty;
		}
		
		slide.HasSpeakerNotes = hasSpeaker;
		slide.SpeakerNotes = speakerNotes;

		
		return slide;
	}
}