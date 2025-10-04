using Eto.Forms;

namespace AsciiSlidesCore.Components;

public class VideoCueControlComponent : GroupBox
{
	private Button _gotoButton;
	private VideoControlComponent _parent;
	private Cue _cue;
	public VideoCueControlComponent(Cue cue, VideoControlComponent control)
	{
		_parent = control;

		_cue = cue;
		
		_gotoButton = new Button();
		_gotoButton.Text = _cue.PrettyText();
		_gotoButton.Click += GotoButtonOnClick;
		Content = _gotoButton;
		
		Console.WriteLine("Cue Created " + _cue.PrettyText());
	}

	private void GotoButtonOnClick(object? sender, EventArgs e)
	{
		_parent.SeekTo(_cue.TotalSeconds().ToString());
	}
}