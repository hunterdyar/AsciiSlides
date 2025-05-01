namespace AsciiSlidesCore;

public class Slide
{
	public readonly string rawContent;
	public readonly Frontmatter frontmatter;
	public readonly bool HasSpeakerNotes = false;
	public readonly string SpeakerNotes; 
	public Slide(Frontmatter frontmatter, string rawContent)
	{
		this.frontmatter = frontmatter;
		this.rawContent = rawContent;
		
		//Set Speaker Notes
		if (frontmatter.TryGetKey("notes", out SpeakerNotes!))
		{
			HasSpeakerNotes = true;
		}
		else
		{
			//SpeakerNotes will be null, but we can surpress the compiler warning because this branch handles that case
			SpeakerNotes = string.Empty;
		}
	}
}