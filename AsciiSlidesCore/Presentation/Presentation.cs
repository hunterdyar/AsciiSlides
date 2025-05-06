namespace AsciiSlidesCore;

//a list of slides, etc.
//serializable and immutable.
public class Presentation
{
	public Frontmatter Frontmatter;
	public Slide[] Slides;
	public int SlideCount => Slides.Length;
	public string Path = string.Empty;

	public string FileName = string.Empty;

	//Construct an Empty Presentation
	public Presentation()
	{
		Frontmatter = new Frontmatter();
		Slides = [];
	}
}