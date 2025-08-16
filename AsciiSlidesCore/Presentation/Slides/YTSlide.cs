using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Eto.Drawing;

namespace AsciiSlidesCore;

public class YTSlide : Slide
{
	private Regex _ytIdMatcher =
		new Regex(
			@"^.*(?:(?:youtu\.be\/|v\/|vi\/|u\/\w\/|embed\/|shorts\/)|(?:(?:watch)?\?v(?:i)?=|\&v(?:i)?=))([^#\&\?]*).*",
			RegexOptions.Compiled);
	private string ytid;
	private string url;
	private Cue[] cues = [];
	private Cue startTime = Cue.StartCue;
	public YTSlide(Presentation presentation, string rawContent) : base(presentation, rawContent)
	{
		this.url = HttpUtility.HtmlEncode(rawContent.Trim());
	}

	public override void PreProcess()
	{
		base.PreProcess();

		if (url.Contains(".com") || url.Contains("youtu.be") || url.Contains("://"))
		{
			var match = _ytIdMatcher.Match(url);
			if (match.Success)
			{
				ytid = match.Groups[1].Value;
				Console.WriteLine($"url to id: {ytid}");
			}
		}
		//if we didn't match...
		if (string.IsNullOrEmpty(ytid))
		{
			//maybe direct value.
			ytid = url;
			Console.WriteLine($"url direct to id: {ytid}");
		}
		
		if (Frontmatter.TryGetKey("cue", out var cue))
		{
			List<Cue> cuesList = new List<Cue>();
			var cues = cue.Split("\n");
			foreach (var item in cues)
			{
				//skip empty lines
				if (cues.Length == 0)
				{
					continue;
				}

				if (Cue.TryCreateCue(item, out var c))
				{
					cuesList.Add(c);
				}
				else
				{
					//todo: print parser warning
				}
			}
			this.cues = cuesList.ToArray();
		}

		//todo: remove magic strings.
		if (Frontmatter.TryGetKey("starttime", out var starttime))
		{
			if (Cue.TryCreateCue(starttime, out var start))
			{
				this.startTime = start;
			}	
		}
	}

	protected override void AppendContent(StringBuilder sb)
	{
		sb.Append($$"""
		          <div class="markdown">
		          <div id="yt">
		          <div id="player" width="100%" height ="100%"></div>
		          </div>
		          </div>
		          <script>
		              var tag = document.createElement('script');
		              tag.src = "https://www.youtube.com/iframe_api";
		              var firstScriptTag = document.getElementsByTagName('script')[0];
		              firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);
		              var player;
		          
		              function onYouTubeIframeAPIReady() {
		                  player = new YT.Player('player', {
		                      height: '100%',
		                      width: '100%',
		                      videoId: '{{ytid}}',
		                      playerVars: {
		                          'playsinline': 1,
		                          'autoplay': true,
		                      },
		                      events: {
		                          'onReady': onPlayerReady,
		                          'onStateChange': onPlayerStateChange
		                      }
		                  });
		              }
		          
		              function onPlayerReady(event) {
		                  //let's just use the autoplay tag for now? idk this needs to come from a slide property and not the yt video
		                  // event.target.playVideo();
		              }
		              
		              function onPlayerStateChange(event) {
		                  
		              }
		          
		              function stopVideo() {
		                  player.stopVideo();
		              }
		              
		              function playVideo() {
		                  player.playVideo();
		              }
		          
		              // -1(unstarted)
		              // 0(ended)
		              // 1(playing)
		              // 2(paused)
		              // 3(buffering)
		              // 5(video cued).
		          
		              function playPauseVideo() {
		                  var state = player.getPlayerState();
		                  console.log("player state",state);
		                  if(state == 0){
		                      //ended! Restart it
		                      player.clearVideo();
		                      player.playVideo()
		                  }else if(state == 1){
		                      //playing, pause it
		                      player.pauseVideo();
		                  }else if(state == 2 || state == -1){//paused or unstarted
		                      player.playVideo();
		                  }else if(state == 5){
		                      //cued!
		                      player.playVideo();
		                  }else if(state == 3){
		                      //buffering.... don't know what we should do cus i don't know if it's trying to play or not?
		                      //i'll guess that it's trying to play.
		                      player.pauseVideo();
		                  }
		              }
		              function muteToggleVideo(){
		                if(player.isMuted()){
		                  player.unMute();
		                }else{
		                  player.mute();
		                }
		              }
		          
		              // var yt = document.getElementById("ytcontainer");
		              // console.log()
		              // yt.children[0].width = "100%"
		              // yt.children[0].height = "100%"
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
