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
		view.Url = url;
		//
	}

	protected override void AppendContent(StringBuilder sb)
	{
		
	}
}