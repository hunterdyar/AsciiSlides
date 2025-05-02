namespace AsciiSlidesCore;

public static class SlideFactory
{
	public static Slide CreateSlide(Frontmatter frontmatter, string rawContent, int number)
	{
		if(!frontmatter.TryGetKey("type", out string slideType))
		{
			slideType = "ascii";
		}
		
		Slide slide;
		switch (slideType.ToLower())
		{
			case "youtube":
				slide = new YTSlide
				{
					Frontmatter = frontmatter,
					RawContent = rawContent,
					SlideNumber = number
				};
				break;
			case "html":
				slide = new HTMLSlide
				{
					Frontmatter = frontmatter,
					RawContent = rawContent,
					SlideNumber = number
				};
				break;
			case "ascii":
			default:
				slide = new Slide
				{
					Frontmatter = frontmatter,
					RawContent = rawContent,
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