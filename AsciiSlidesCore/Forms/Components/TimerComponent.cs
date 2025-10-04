using Eto.Drawing;
using Eto.Forms;

namespace AsciiSlidesCore.Components;

public class TimerComponent : GroupBox
{
	//todo: SlideTimers will exist in the PresentationState class. Total, and perSlide.
	private SlideTimer _timer;
	private Label _time;
	
	public TimerComponent(string label)
	{
		_timer = new SlideTimer();

		_time = new Label()
		{
			TextColor = Colors.White,
			Font = new Font("Courier", 48, FontStyle.None, FontDecoration.None)
		};
		_time.Text = "00:00";
		
		
		Text = label;
		TextColor = Colors.White;
		Content = _time;
		_timer.Start();

		this.Load += (sender, args) =>
		{
			_timer.OnTimeChanged += OnTimeChanged;
		};
		this.UnLoad += (sender, args) =>
		{
			_timer.OnTimeChanged -= OnTimeChanged;
		};
	}

	private void OnTimeChanged(string time)
	{
		
		_time.Text = time;
	}
}