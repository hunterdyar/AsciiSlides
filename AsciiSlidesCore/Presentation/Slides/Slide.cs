using System.Text;
using Eto.Drawing;

namespace AsciiSlidesCore;

public class Slide
{
	public string RawContent;
	public Frontmatter Frontmatter;
	public bool HasSpeakerNotes = false;
	public string SpeakerNotes; 
	private StringBuilder _sb = new();

	public int SlideNumber;

	public Slide()
	{
		Frontmatter = new Frontmatter();
		RawContent = "";
		SlideNumber = 0;
	}

	/// <param name="isPreview">If not primary slide view (don't autoplay videos no matter what, etc.)</param>
	public string GetSlideAsHTML(PresentationState state, Rectangle windowBounds, bool isPreview)
	{
		_sb.Clear();
		_sb.AppendLine("<html>");
		_sb.AppendLine("<head>");
		AppendStyle(_sb, state, windowBounds);
		_sb.AppendLine("</head>");
		_sb.AppendLine("<body>");
		_sb.AppendLine("<div class=\"container\">");
		AppendContent(_sb);
		_sb.AppendLine("</div>");
		_sb.AppendLine("</body>");
		_sb.AppendLine("</html>");

		return _sb.ToString();
	}

	protected virtual void AppendContent(StringBuilder sb)
	{
		sb.AppendLine("<pre class=\"slide\">");
		sb.AppendLine(RawContent);
		sb.AppendLine("</pre>");
	}
	protected virtual void AppendStyle(StringBuilder sb, PresentationState state, Rectangle bounds)
	{
		sb.AppendLine("<style>");

		//these are clearly bad defaults... 
		int h = 0;
		int w = 0;
		
		var screenAspect = bounds.Width / (float)bounds.Height;
		
		if (state.Aspect >= screenAspect)
		{
			//if aspect is equal, we can do either branch and it doesn't matter.

			//we are wider, and will letterbox top and bottom.
			//Set the width to full bounds width, adjust height by aspect.
			w = (int)(bounds.Width);
			h = (int)(bounds.Width / state.Aspect);
		}
		else if (state.Aspect < screenAspect)
		{
			//we are narrow, screen is wide. will letterbox sides.
			//set the height to full bounds height, adjust the width by aspet.
			w = (int)(bounds.Height * state.Aspect);
			h = (int)(bounds.Height);
		}
		int marginLeft = (bounds.Width - w) / 2;
		int marginTop = (bounds.Height - h) / 2;
		int fontHeight = (int)Math.Floor(h / (float)state.RowCount);
		sb.Append($$$"""
		             body{
		                 background-color: color: #{{{Configuration.BGColor.ToHex()}}};
		                 padding: 0;
		                 margin: 0;
		                 font-family: Consolas, monospace, ui-monospace;
		                 font-size: {{{fontHeight}}}px;
		                 color: #{{{Configuration.FontColor.ToHex()}}};
		                 overflow: hidden;
		                 scrollbar-width: none;
		             }
		             .container {
		                 padding: 0;
		                 display: block;
		                 width: {{{w}}}px;
		                 height: {{{h}}}px;
		                 margin-left: {{{marginLeft}}};
		                 margin-right: {{{marginLeft}}};
		                 marigin-bottom: {{{marginTop}}};
		                 margin-top: {{{marginTop}}};
		                 background-color: color: #{{{Configuration.ASCIIAreaBGColor.ToHex()}}};
		             }
		                 .slide{
		             }
		             """);
		sb.AppendLine("</style>");
	}

	protected virtual string GetSlideBGColor()
	{
		if (Frontmatter.TryGetKey("background", out string bg))
		{
			return bg;
		}
		return $"#{Configuration.BGColor.ToHex()}";
	}

	protected virtual string GetSlideTextColor()
	{
		if (Frontmatter.TryGetKey("textcolor", out var fontColor))
		{
			return fontColor;
		}

		return $"#{Configuration.BGColor.ToHex()}";
	}
}