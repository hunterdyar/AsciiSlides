using System.Text;
using Rectangle = Eto.Drawing.Rectangle;

namespace AsciiSlides;

public class PresentationState
{
	private Presentation _presentation = new Presentation(new Frontmatter(),[]);
	
	public int CurrentSlide;
	private int _currentSlide => CurrentSlide;
	public int RowCount = 30;
	public int ColumnCount = 40;
	public static Action OnCurrentSlideChanged;
	private StringBuilder _builder = new StringBuilder();

	public PresentationState(Presentation presentation)
	{
		_presentation = presentation;
		CurrentSlide = 0;
	}
	public void NavigateRelative(int delta)
	{
		if (delta == 0)
		{
			return;
		}
	}

	public string GetCurrentAsHTML(Rectangle bounds)
	{
		_builder.Clear();
		_builder.AppendLine("<html>");
		_builder.AppendLine("<head>");
		AppendStyle(bounds);
		_builder.AppendLine("</head>");
		_builder.AppendLine("<body>");
		_builder.AppendLine("<div class=\"container\">");
		_builder.AppendLine("<pre class=\"slide\">");
		_builder.AppendLine(_presentation.Slides[CurrentSlide].rawContent);
		_builder.AppendLine("</pre>");
		_builder.AppendLine("</div>");
		_builder.AppendLine("</body>");
		_builder.AppendLine("</html>");
		
		return _builder.ToString();
	}

	private void AppendStyle(Rectangle bounds)
	{
		_builder.AppendLine("<style>");

		//these are clearly bad defaults... 
		int h = 0;
		int w = 0;
		
		var aspect = ColumnCount / (float)RowCount;
		var screenAspect = bounds.Width / (float)bounds.Height;
		
		if (aspect >= screenAspect)
		{
			//if aspect is equal, we can do either branch and it doesn't matter.

			//we are wider, and will letterbox top and bottom.
			//Set the width to full bounds width, adjust height by aspect.
			w = (int)(bounds.Width);
			h = (int)(bounds.Width / aspect);
		}
		else if (aspect < screenAspect)
		{
			//we are narrow, screen is wide. will letterbox sides.
			//set the height to full bounds height, adjust the width by aspet.
			w = (int)(bounds.Height * aspect);
			h = (int)(bounds.Height);
		}
		int marginLeft = (bounds.Width - w) / 2;
		int marginTop = (bounds.Height - h) / 2;
		int fontHeight = (int)Math.Floor(h / (float)RowCount);
		_builder.Append($$$"""
		                body{
		                   background-color: color: #{{{Configuration.Configuration.BGColor.ToHex()}}};
		                   padding: 0;
		                   margin: 0;
		                   font-family: Consolas, monospace, ui-monospace;
		                   font-size: {{{fontHeight}}}px;
		                   color: #{{{Configuration.Configuration.FontColor.ToHex()}}};
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
		                  background-color: color: #{{{Configuration.Configuration.ASCIIAreaBGColor.ToHex()}}};
		                   }
		                    .slide{
		                    }
		                """);
		Console.WriteLine($"Setting width height to: {w}x{h}, margins left/top: {marginLeft}/{marginTop}");
		_builder.AppendLine("</style>");
	}
}