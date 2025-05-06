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
}