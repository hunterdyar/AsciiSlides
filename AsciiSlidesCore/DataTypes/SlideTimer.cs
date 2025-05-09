using System.Timers;
using Timer = System.Timers.Timer;

namespace AsciiSlidesCore;

public class SlideTimer
{
	public Action<string> OnTimeChanged;
	
	private readonly Eto.Forms.UITimer _timer;
	public TimeSpan Time => _time;
	private  TimeSpan _time = TimeSpan.Zero;
	private readonly TimeSpan _delta = TimeSpan.FromMilliseconds(1000);
	private double _totalSeconds;
	public SlideTimer()
	{
		_timer = new Eto.Forms.UITimer();
		_timer.Elapsed += OnElapsed;
		_timer.Interval = 1;
		_totalSeconds = -1;
		_timer.Start();
	}

	public void Start()
	{
		_timer.Start();
	}

	public void Pause()
	{
		_timer.Stop();
	}

	public void Reset()
	{
		_timer.Stop();
		_totalSeconds = -1;
		_time = TimeSpan.Zero;
		//_timer.Start();
	}

	private void OnElapsed(object? sender, EventArgs eventArgs)
	{
		_time = _time.Add(_delta/_timer.Interval);
		if (Math.Abs(_time.TotalSeconds - _totalSeconds) > 0.01f)
		{
			OnTimeChanged?.Invoke(this.ToString());
			_totalSeconds = _time.TotalSeconds;
		}
	}

	public override string ToString()
	{
		return _time.Minutes.ToString("00") + ":" + _time.Seconds.ToString("00");
	}
}