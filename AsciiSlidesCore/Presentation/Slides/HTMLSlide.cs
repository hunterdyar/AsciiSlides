using System.Text;
using Eto.Drawing;

namespace AsciiSlidesCore;

public class HTMLSlide : Slide
{
	private string style;

	public HTMLSlide(string rawContent) : base(rawContent)
	{
		//Coding design note.... should this be in the factory?
		if (!Frontmatter.TryGetKey("style", out style))
		{
			style = "";
		}
	}

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