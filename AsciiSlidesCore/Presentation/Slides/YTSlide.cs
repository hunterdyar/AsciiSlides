using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Web;
using Eto.Drawing;

namespace AsciiSlidesCore;

public class YTSlide : Slide
{
	private string url;
	private string html;
	public YTSlide(Presentation presentation, string rawContent) : base(presentation, rawContent)
	{
		this.url = HttpUtility.HtmlEncode(rawContent.Trim());
	}

	public override void PreProcess()
	{
		base.PreProcess();
		var http = new HttpClient()
		{
			// BaseAddress = new Uri("https://www.youtube.com/oembed"),
		};
		var r = http.GetFromJsonAsync<YTOEmbedResponse>("https://www.youtube.com/oembed?url=" + this.url);
		r.Wait();
		if (r.IsCompletedSuccessfully)
		{
			this.html = r.Result.html;
		}
		else
		{
			Console.WriteLine("bad url!");
		}
		//https://www.youtube.com/oembed?url=<URL>&format=<FORMAT>
	}

	protected override void AppendContent(StringBuilder sb)
	{
		//todo: replace width/height with correct w/h.
		sb.Append(html);
	}

	public override void OnRender()
	{
		//todo: ask the webview to call some javascript?
		base.OnRender();
	}
}

public record YTOEmbedResponse
{
	public string title;
	public string author_name;
	public string author_url;
	public string type;
	public int height;
	public int width;
	public string version;
	public string provider_name;
	public string provider_url;
	public int thumbnail_height;
	public int thumbnail_width;
	public string thumbnail_url;
	public string html;
}