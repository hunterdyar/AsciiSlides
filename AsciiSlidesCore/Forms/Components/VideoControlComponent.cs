using Eto.Forms;

namespace AsciiSlidesCore.Components;

public class VideoControlComponent : GroupBox
{
	private Button _playPause;

	public VideoControlComponent()
	{
		_playPause = new Button { Text = "Play/Pause" };
		_playPause.Click += PlayPauseOnClick;
		//
		Content = _playPause;
	}

	private void PlayPauseOnClick(object? sender, EventArgs e)
	{
		SlidesManager.PresentationState?.CallSlideFunction("playvideo");
	}
}