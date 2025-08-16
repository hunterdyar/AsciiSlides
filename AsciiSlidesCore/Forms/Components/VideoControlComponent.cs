using Eto.Drawing;
using Eto.Forms;

namespace AsciiSlidesCore.Components;

public class VideoControlComponent : GroupBox
{
	private Button _playPause;
	private Button _mute;

	public VideoControlComponent()
	{
		_playPause = new Button { Text = "Play/Pause" };
		_playPause.Click += PlayPauseOnClick;
		_mute = new Button { Text = "Mute" };
		_mute.Click += MuteOnClick;
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
		SlidesManager.PresentationState?.CallSlideFunction("mute");
	}

	private void PlayPauseOnClick(object? sender, EventArgs e)
	{
		SlidesManager.PresentationState?.CallSlideFunction("playvideo");
	}

	public void SetVisible(bool visible)
	{
		this.Visible = visible;
		_mute.Enabled = visible;
		_playPause.Enabled = visible;
	}
}