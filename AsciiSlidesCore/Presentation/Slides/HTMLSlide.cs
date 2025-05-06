using System.Text;
using Eto.Drawing;

namespace AsciiSlidesCore;

public class HTMLSlide : Slide
{
	private string style;

	public HTMLSlide(Presentation presentation, string rawContent) : base(presentation, rawContent)
	{
		style = Frontmatter.GetKey("style", "");
	}

	//todo: definable font size in screen space.
	protected override void AppendContent(StringBuilder sb)
	{
		sb.Append(RawContent);
	}

	protected override void AppendStyle(StringBuilder sb, PresentationState state, Rectangle bounds)
	{
		base.AppendStyle(sb, state, bounds);
		sb.AppendLine("<style>\n");
		sb.Append(style);
		sb.AppendLine("\n</style>");
	}
}