using System.Text;

namespace AsciiSlidesCore;

public class ASCIISlide : Slide
{
	public int RowCount;
	public int ColumnCount;

	public ASCIISlide(Presentation presentation, string rawContent) : base(presentation, rawContent)
	{
		(RowCount, ColumnCount) = rawContent.GetRowColCount();
	}

	protected override void AppendContent(StringBuilder sb)
	{
		sb.AppendLine("<pre class=\"slide\">");
		sb.AppendLine(RawContent);
		sb.AppendLine("</pre>");
	}

	//todo: I don't really like this setup for thinking about when we do or don't override, and how we deal with whitespace.
	
	// protected override int GetColCount(PresentationState state)
	// {
	// 	return ColumnCount;
	// }
	//
	// protected override int GetRowCount(PresentationState state)
	// {
	// 	return RowCount;
	// }
	//
	// protected override float GetAspect(PresentationState state)
	// {
	// 	return (float)RowCount / (float)ColumnCount;
	// }
}