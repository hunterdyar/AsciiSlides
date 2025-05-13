using System.Text;
using Eto.Drawing;

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
		sb.AppendLine("""<div class="markdown">""");
		sb.AppendLine("""<div class="slide">""");
		sb.Append(asHTMl);
		sb.AppendLine("</div>");
		sb.AppendLine("</div>");

	}

	protected override void AppendStyle(StringBuilder sb, PresentationState state, Rectangle bounds)
	{
		var background = Frontmatter.GetKey("background", Configuration.BGColor);
		var fontcolor = Frontmatter.GetKey("textcolor", Configuration.FontColor);
		var fontsize = Frontmatter.GetKey("fontsize", Configuration.FontSizeVMin.ToString());
		sb.Append("<style>");
		sb.Append($$"""
		          body{
		              background-color: {{background}};
		              padding: 0;
		              margin: 0;
		              font-family: Consolas, monospace, ui-monospace;
		              color: {{fontcolor}};
		              font-size: {{fontsize}}vmin;
		              overflow: hidden;
		              scrollbar-width: none;
		          }
		          .markdown {
		            display: flex;
		            height: 100%;
		            flex-direction: column;
		            justify-content: center;
		          }
		          .slide{
		          margin: auto;
		          }
		          """);
		sb.Append("</style>");
	}
}