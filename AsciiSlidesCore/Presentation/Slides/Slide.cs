using System.Text;
using Eto.Drawing;
using Eto.Forms;

namespace AsciiSlidesCore;

public abstract class Slide
{
	protected Presentation presentation;
	public string RawContent;
	public Frontmatter Frontmatter;
	public bool HasSpeakerNotes = false;
	public string SpeakerNotes = String.Empty; 
	private StringBuilder _sb = new();
	public int SlideNumber;
	public bool PreProcessed => _preProcessed;
	protected bool _preProcessed = false;

	public Slide(Presentation presentation, string rawContent)
	{
		this.presentation = presentation;
		Frontmatter = new Frontmatter();
		RawContent = rawContent;
		SlideNumber = 0;
	}

	public virtual void OnSlideFunction(string slideFunctionName)
	{

	}
	
	/// <summary>
	/// Should be called once and only once before first time loaded.
	/// </summary>
	public virtual void PreProcess()
	{
		_preProcessed = true;
	}
	
	public virtual void RenderTo(PresentationState state, WebView view, SlideViewMode mode= SlideViewMode.CurrentPresenting)
	{
		view.LoadHtml(GetSlideAsHTML(state, view.Bounds, mode));
	}

	public string GetSlideAsHTML(PresentationState state, Rectangle windowBounds, SlideViewMode viewMode)
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

	protected abstract void AppendContent(StringBuilder sb);
	protected virtual void AppendStyle(StringBuilder sb, PresentationState state, Rectangle bounds)
	{
		sb.AppendLine("<style>");

		//these are clearly bad defaults... 
		int h = 0;
		int w = 0;
		
		var screenAspect = bounds.Width / (float)bounds.Height;
		float aspect = GetAspect(state, bounds);
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
		int fontHeight = (int)Math.Floor(h / (float)GetRowCount(state));

		var background = Frontmatter.GetKey("background", Configuration.BGColor);
		var fontcolor = Frontmatter.GetKey("textcolor", Configuration.FontColor);

		sb.Append($$"""
		             body{
		                 background-color: {{background}};
		                 
		                 padding: 0;
		                 margin: 0;
		                 font-family: Consolas, monospace, ui-monospace;
		                 font-size: {{fontHeight}}px;
		                 color: {{fontcolor}};
		                 overflow: hidden;
		                 scrollbar-width: none;
		             }
		             .container {
		                 padding: 0;
		                 display: block;
		                 width: {{w}}px;
		                 height: {{h}}px;
		                 margin-left: {{marginLeft}}px;
		                 margin-right: {{marginLeft}}px;
		                 margin-bottom: {{marginTop}}px;
		                 margin-top: {{marginTop}}px;
		                 background-color: {{background}};
		             }
		                 .slide{
		             }
		             """);
		sb.AppendLine("</style>");

		//append user-specific styling.
		if (Frontmatter.TryGetKey("style", out string value))
		{
			_sb.AppendLine("<style>");
			_sb.AppendLine(value);
			_sb.AppendLine("</style>");
		}
	}

	protected virtual int GetRowCount(PresentationState state)
	{
		if (!Frontmatter.TryGetKey("rows", out var rows))
		{
			return state.RowCount;
		}

		if (int.TryParse(rows, out var rowcount))
		{
			return rowcount;
		}
		Console.WriteLine($"Warning: Unable to parse int ({rows})");
		
		return state.RowCount;
	}
	

	protected virtual float GetAspect(PresentationState state, Rectangle bounds)
	{
		//todo: rows/cols...
		return state.Aspect;
	}

	public virtual void OnRender()
	{
		//do nothing
	}
}