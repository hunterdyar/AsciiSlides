using System.Text;
using Rectangle = Eto.Drawing.Rectangle;

namespace AsciiSlides;

public class PresentationState
{
	private Presentation _presentation = new Presentation();
	
	public int CurrentSlide;
	private int _currentSlide => CurrentSlide;
	public int RowCount = 60;
	public int ColumnCount = 80;
	public static Action OnCurrentSlideChanged;
	private StringBuilder _builder = new StringBuilder();
	
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
		_builder.AppendLine("<body style=\"background-color:#F0F; font-family:Consolas;\">");
		_builder.AppendLine("<div class=\"container\">");
		_builder.AppendLine("<div class=\"slide\">");

		_builder.AppendLine("<h1>Presentation State</h1>");
		_builder.AppendLine("<p>Presentation State</p>");
		_builder.AppendLine("</div>");
		_builder.AppendLine("</div>");

		_builder.AppendLine("</body>");
		_builder.AppendLine("</html>");
		
		return _builder.ToString();
	}

	private void AppendStyle(Rectangle bounds)
	{
		_builder.AppendLine("<style>");
		
		
		
		_builder.Append($$$"""
		                body{
		                background: white;
		                           padding: 0;
		                           margin: 0;
		                       }
		                       .container {
		                           display: flex;
		                           align-items: center;
		                           padding: 0;
		                           justify-content: center;
		                       }
		                       .slide{
		                           background: tomato;
		                           flex-basis: auto;
		                           align-self: center;
		                           margin: auto;
		                           padding: 0;
		                           width: 500px;
		                           max-width: 1000px;
		                       }
		                """);
		
		_builder.AppendLine("</style>");
		Console.Write(_builder.ToString());
	}
}