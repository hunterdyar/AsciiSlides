using System.Text;

namespace AsciiSlidesCore;

public class MarkdownSlides : Slide
{
	private string asHTMl;
	public MarkdownSlides(string rawContent) : base(rawContent)
	{
			asHTMl = Markdig.Markdown.ToHtml(RawContent);
	}

	protected override void AppendContent(StringBuilder sb)
	{
		sb.Append(asHTMl);
	}
}