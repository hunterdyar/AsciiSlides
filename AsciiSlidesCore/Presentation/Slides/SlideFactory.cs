namespace AsciiSlidesCore;

public static class SlideFactory
{
	public static Slide CreateSlide(Presentation presentation, Frontmatter frontmatter,string rawContent, int number, string defaultType="ascii")
	{
		if(!frontmatter.TryGetKey("type", out string slideType))
		{
			slideType = defaultType;
		}
		
		Slide slide;
		switch (slideType.ToLower())
		{
			case "youtube":
				slide = new YTSlide(presentation, rawContent)
				{
					Frontmatter = frontmatter,
					SlideNumber = number
				};
				break;
			case "html":
				slide = new HTMLSlide(presentation, rawContent)
				{
					Frontmatter = frontmatter,
					SlideNumber = number
				};
				break;
			case "md":
			case "markdown":
				slide = new MarkdownSlide(presentation, rawContent)
				{
					Frontmatter = frontmatter,
					SlideNumber = number
				};
				break;
			case "image":
			case "img":
				slide = new ImageSlide(presentation, rawContent)
				{
					Frontmatter = frontmatter,
					SlideNumber = number
				};
				break;
			case "ascii":
			default:
				slide = new ASCIISlide(presentation, rawContent)
				{
					Frontmatter = frontmatter,
					SlideNumber = number
				};
				break;
		}

		bool hasSpeaker;
		string speakerNotes;
		//Set Speaker Notes
		if (frontmatter.TryGetKey("notes", out speakerNotes))
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