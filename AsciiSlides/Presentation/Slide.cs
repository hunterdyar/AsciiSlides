namespace AsciiSlides;

public class Slide
{
	public string rawContent;
	public Frontmatter frontmatter;
	public Slide(Frontmatter frontmatter, string rawContent)
	{
		this.frontmatter = frontmatter;
		this.rawContent = rawContent;
	}
}