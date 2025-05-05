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
		Directory.SetCurrentDirectory(presentation.Path);
		source = Path.GetFullPath(source);
	}

	protected override void AppendContent(StringBuilder sb)
	{
		//todo: properly determine path and our 'asked for' source. Determine if it's relative or absolute, prepend path, etc etc. 
		//i can only assume there's a nice utility for that somewhere.
		
		sb.Append("<img src=\"" + source + "\" width=\"100%\" Height=\"100%\"/>");
	}
}