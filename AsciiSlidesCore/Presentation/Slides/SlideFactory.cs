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
		switch (slideType)
		{
			case "html":
				slide = new HTMLSlide
				{
					frontmatter = frontmatter,
					rawContent = rawContent,
					SlideNumber = number
				};
				break;
			case "ascii":
			default:
				slide = new Slide
				{
					frontmatter = frontmatter,
					rawContent = rawContent,
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