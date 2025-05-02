using System.Text;
using Eto.Drawing;

namespace AsciiSlidesCore;

public class Slide
{
	public readonly string rawContent;
	public readonly Frontmatter frontmatter;
	public readonly bool HasSpeakerNotes = false;
	public readonly string SpeakerNotes; 
	private StringBuilder _sb = new StringBuilder();

	public readonly int SlideNumber;

	public Slide()
	{
		frontmatter = new Frontmatter();
		rawContent = "";
		SlideNumber = 0;
	}
	public Slide(Frontmatter frontmatter, string rawContent, int number)
	{
		this.frontmatter = frontmatter;
		this.rawContent = rawContent;
		this.SlideNumber = number;
		
		//Set Speaker Notes
		if (frontmatter.TryGetKey("notes", out SpeakerNotes!))
		{
			HasSpeakerNotes = true;
		}
		else
		{
			//SpeakerNotes will be null, but we can surpress the compiler warning because this branch handles that case
			SpeakerNotes = string.Empty;
		}
	}


	/// <param name="isPreview">If not primary slide view (don't autoplay videos no matter what, etc)</param>
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
		sb.AppendLine(rawContent);
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
		Console.WriteLine($"Setting width height to: {w}x{h}, margins left/top: {marginLeft}/{marginTop}");
		sb.AppendLine("</style>");
	}

	protected virtual string GetSlideBGColor()
	{
		if (frontmatter.TryGetKey("background", out var bg))
		{
			if (bg != null)
			{
				return bg;
			}
		}
		return $"#{Configuration.BGColor.ToHex()}";
	}

	protected virtual string GetSlideTextColor()
	{
		if (frontmatter.TryGetKey("textcolor", out var fontColor))
		{
			if (fontColor != null)
			{
				return fontColor;
			}
		}

		return $"#{Configuration.BGColor.ToHex()}";
	}
}