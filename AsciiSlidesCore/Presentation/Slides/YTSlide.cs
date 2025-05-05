using System.Text;
using Eto.Drawing;

namespace AsciiSlidesCore;

public class YTSlide : Slide
{
	private string url;

	public YTSlide(Presentation presentation, string rawContent) : base(presentation, rawContent)
	{
		//you can make the url the body.
		if (!Frontmatter.TryGetKey("url", out url))
		{
			url = RawContent;
		}
	}

	protected override void AppendContent(StringBuilder sb)
	{
		//https://www.youtube.com/oembed?url=<URL>&format=<FORMAT>
		sb.Append($$$"""
		          <iframe
			width = "560" height = "315" src = "{{{url}}}" title =
			"YouTube video player" frameborder = "0" allow =
			"accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share" referrerpolicy =
			"strict-origin-when-cross-origin" allowfullscreen ></iframe >
""");
		throw new NotImplementedException("Dynamic YT slides not yet supported. Use HTML slide");

	}
}