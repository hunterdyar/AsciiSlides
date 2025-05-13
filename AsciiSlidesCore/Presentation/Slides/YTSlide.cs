using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Web;
using Eto.Drawing;

namespace AsciiSlidesCore;

public class YTSlide : Slide
{
	private string url;
	public YTSlide(Presentation presentation, string rawContent) : base(presentation, rawContent)
	{
		this.url = HttpUtility.HtmlEncode(rawContent.Trim());
	}

	protected override void AppendContent(StringBuilder sb)
	{
		sb.Append("""
		          <div class="markdown">
		              <div id="ytcontainer">

		              </div>
		          </div>
		          <script>
		              function getOembed(yturl, callback)
		              {
		                  var xmlHttp = new XMLHttpRequest();
		                  xmlHttp.onreadystatechange = function() {
		                      if (xmlHttp.readyState == 4 && xmlHttp.status == 200)
		                          callback(JSON.parse(xmlHttp.response));
		                  }
		                  var uri = encodeURI("https://www.youtube.com/oembed?url="+yturl);
		                  console.log(uri)
		                  xmlHttp.open("GET", uri, true); // true for asynchronous
		                  xmlHttp.send(null);
		              }
		              getOembed("https://youtu.be/_F-6UzROZsY",(res)=>{
		                  var yt = document.getElementById("ytcontainer");
		                  console.log(res)
		                  yt.innerHTML = res.html;
		                  yt.children[0].width = "100%"
		                  yt.children[0].height = "100%"

		              })
		          </script>
		          """);
	}

	protected override void AppendStyle(StringBuilder sb, PresentationState state, Rectangle bounds)
	{
		sb.Append("""
		          <style>
		                  body{
		                      background-color: #010101;
		                      padding: 0;
		                      margin: 0;
		                      font-family: Consolas, monospace, ui-monospace;
		                      font-size: 4vh;
		                      color: #F0F;
		                      overflow: hidden;
		                      scrollbar-width: none;
		                  }
		                  #ytcontainer{
		                      width: 100%;
		                      height: 100%;
		                  }
		          </style>
		          """);
	}

	public override void OnRender()
	{
		//todo: ask the webview to call some javascript?
		base.OnRender();
	}
}
