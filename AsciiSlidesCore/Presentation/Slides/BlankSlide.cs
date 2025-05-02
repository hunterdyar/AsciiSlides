using System.Text;

namespace AsciiSlidesCore;

public class BlankSlide : Slide
{
	private readonly string _message;

	public BlankSlide(string message)
	{
		_message = message;
	}

	protected override void AppendContent(StringBuilder sb)
	{
		if (!string.IsNullOrEmpty(_message))
		{
			sb.AppendLine($"<p style='text-align: center;'>{_message}</p>");
		}
		base.AppendContent(sb);
	}

	protected override string GetSlideBGColor()
	{
		return "#000";
	}

	protected override string GetSlideTextColor()
	{
		return "#FFF";
	}
}