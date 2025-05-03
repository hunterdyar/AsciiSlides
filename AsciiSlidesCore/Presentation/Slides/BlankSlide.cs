using System.Text;

namespace AsciiSlidesCore;

public class BlankSlide : Slide
{

	public BlankSlide(string rawContent) : base(rawContent)
	{
	}

	protected override void AppendContent(StringBuilder sb)
	{
		if (!string.IsNullOrEmpty(RawContent))
		{
			sb.AppendLine($"<p style='text-align: center;'>{RawContent}</p>");
		}

		return;
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