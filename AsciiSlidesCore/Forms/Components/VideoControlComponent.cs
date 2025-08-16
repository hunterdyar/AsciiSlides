using Eto.Drawing;
using Eto.Forms;

namespace AsciiSlidesCore.Components;

public class VideoControlComponent : GroupBox
{
	private Button _playPause;
	private Button _mute;
	private Slider _slider;
	public VideoControlComponent()
	{
		_playPause = new Button { Text = "Play/Pause" };
		_playPause.Click += PlayPauseOnClick;
		_mute = new Button { Text = "Mute" };
		_mute.Click += MuteOnClick;
		
		//the slider is going to require a callback from the video state.
		//we should represent the video state in a class or object in the YTSlide object... which is not mutable?
		//so in the presentationState object? i guess. anyway, it should be it's own thing, and not a spaghetti of string messages shouted back and forth
		//i can live with shouting strings one way (slideFunction) but back/forth is a mess.
		//gotta contain the javascript somewhere. in YTSlide makes the most sense, but again -- caching webviews is not a thing yet, so a reference to the webview is not something a slide has. or should have? maybe that's fine?
		// _slider = new Slider();
		// _slider
		//
		
		var table = new TableLayout();
		var buttonRow = new TableRow();
		buttonRow.Cells.Add(_playPause);
		buttonRow.Cells.Add(_mute);
		table.Rows.Add(buttonRow);
		table.Rows[0].Cells[0].ScaleWidth = true;
		table.Rows[0].Cells[1].ScaleWidth = false;
		
		Content = table;
		this.Text = "Video Controls";
		this.TextColor = Colors.White;
	}

	private void MuteOnClick(object? sender, EventArgs e)
	{
		SlidesManager.PresentationState?.CallSlideFunction("mute", "");
	}

	private void PlayPauseOnClick(object? sender, EventArgs e)
	{
		SlidesManager.PresentationState?.CallSlideFunction("playvideo", "");
	}

	// private void SetVolume(object? sender, EventArgs e)
	// {
	// 	// SlidesManager.PresentationState?.CallSlideFunction("volume", "");
	// }

	public void SetVisible(bool visible)
	{
		this.Visible = visible;
		_mute.Enabled = visible;
		_playPause.Enabled = visible;
	}
}