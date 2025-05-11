using System.Text;
using Eto.Forms;

namespace AsciiSlidesCore;

public class WebSlide : Slide
{
	private Uri url;
	public WebSlide(Presentation presentation, string url) : base(presentation, url)
	{
		this.url = new Uri(url);
	}

	public override void RenderTo(PresentationState state, WebView view, SlideViewMode mode)
	{
		//todo: so it isn't crashing... it's freezing. At the very least we need to move this to its own thread?
		view.Url = url;
		//
	}

	protected override void AppendContent(StringBuilder sb)
	{
		
	}
}