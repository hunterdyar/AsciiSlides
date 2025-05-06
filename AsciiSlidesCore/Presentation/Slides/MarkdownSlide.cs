using System.Text;

namespace AsciiSlidesCore;

public class MarkdownSlide : Slide
{
	private string asHTMl;
	public MarkdownSlide(Presentation presentation, string rawContent) : base(presentation, rawContent)
	{
		asHTMl = Markdig.Markdown.ToHtml(RawContent);
	}

	protected override void AppendContent(StringBuilder sb)
	{
		sb.Append(asHTMl);
	}
}