namespace AsciiSlides;

//a list of slides, etc.
//serializable and immutable.
public class Presentation
{
	public Frontmatter Frontmatter;
	public Slide[] Slides;
	public int SlideCount => Slides.Length;

	//Construct an Empty Presentation
	public Presentation()
	{
		Frontmatter = new Frontmatter();
		Slides = Array.Empty<Slide>();
	}

	public Presentation(Frontmatter frontmatter, Slide[] slides)
	{
		this.Frontmatter = frontmatter;
		this.Slides = slides;
	}
}