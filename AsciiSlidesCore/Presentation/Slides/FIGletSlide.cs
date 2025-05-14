using System.Text;
using FIGlet;
using FIGlet.Drawing;

namespace AsciiSlidesCore;

public class FIGletSlide : Slide
{
	private string text;
	private string output;
	private int width;
	public FIGletSlide(Presentation presentation, string rawContent) : base(presentation, rawContent)
	{
		text = rawContent.Trim();
		output = Write();
		width = output.IndexOf('\n');
		width = width == null ? output.Length : width + 1;
	}

	string Write()
	{
		// font = Frontmatter.GetKey("font",)
		FIGdriver driver = new FIGdriver()
		{
			Font = FIGdriver.DefaultFont,
		};
		driver.Write(text);
		driver.LayoutRule = LayoutRule.Fitting;
		return driver.ToString();
	}

	protected override void AppendContent(StringBuilder sb)
	{
		sb.AppendLine("<pre class=\"slide\">");
		sb.Append(output);
		sb.AppendLine("</pre>");
	}
}