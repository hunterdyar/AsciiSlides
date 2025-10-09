using System.Text;
using Eto.Drawing;

namespace AsciiSlidesCore;

public class ImageSlide : Slide
{
	private string source;
	private string fullImagepath;
	public ImageSlide(Presentation presentation, string url) : base(presentation, url)
	{
		source = url.Trim();
		fullImagepath = source;
	}

	protected override void AppendContent(StringBuilder sb)
	{
		//todo: properly determine path and our 'asked for' source. combine should do it, but getfullpath should to with current directory?
		Directory.SetCurrentDirectory(presentation.Path);
		fullImagepath = Path.GetFullPath(source);
		sb.Append("<div class=\"fill\" background-image: url(\'" + fullImagepath + "\');></div>");
	}

	protected override void AppendStyle(StringBuilder sb, PresentationState state, Rectangle bounds)
	{
		base.AppendStyle(sb, state, bounds);
		sb.AppendLine(@"
			<style>
				.fill {
					background-image: url('" + fullImagepath + @"');
					background-size: contain;
					background-repeat: no-repeat;
					background-position: center;
					width: 100%;
					height: 100%;
				}
			</style>");
	}

	protected override float GetAspect(PresentationState state, Rectangle bounds)
	{
		return bounds.Width / (float)bounds.Height;
	}
}