using System.Text;

namespace AsciiSlidesCore;

public class BlankSlide : Slide
{

	public BlankSlide(Presentation presentation, string rawContent) : base(presentation, rawContent)
	{
		this.Frontmatter.AddKeyValuePair("background","black");
		this.Frontmatter.AddKeyValuePair("textcolor", "white");

	}

	protected override void AppendContent(StringBuilder sb)
	{
		if (!string.IsNullOrEmpty(RawContent))
		{
			sb.AppendLine($"<p style='text-align: center;'>{RawContent}</p>");
		}

		return;
	}
	
}