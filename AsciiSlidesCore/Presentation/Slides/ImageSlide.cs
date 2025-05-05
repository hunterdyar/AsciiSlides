using System.Text;

namespace AsciiSlidesCore;

public class ImageSlide : Slide
{
	private string source;
	public ImageSlide(Presentation presentation, string rawContent) : base(presentation, rawContent)
	{
		if (!Frontmatter.TryGetKey("src", out source))
		{
			source = rawContent;
		}

		source = source.Trim();
	
	}

	protected override void AppendContent(StringBuilder sb)
	{
		//todo: properly determine path and our 'asked for' source. combine should do it, but getfullpath should to with current directory?
		
		//todo: do this at parse-time by passing path into presentation during parse, not after.
		Directory.SetCurrentDirectory(presentation.Path);
		var s = Path.GetFullPath(source);
		sb.Append("<img src=\"" + s + "\" width=\"100%\" Height=\"100%\"/>");
	}
}