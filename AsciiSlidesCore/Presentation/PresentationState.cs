using System.Text;
using Rectangle = Eto.Drawing.Rectangle;

namespace AsciiSlidesCore;

public class PresentationState
{
	public static bool IsPresentationReady = false;
	public static bool IsPresenting = false;
	public static Action<string, string> OnSlideFunction = delegate {};
	public static Action<bool> OnIsPresentingChanged = delegate {};
	public static Action<bool> OnIsPresentationReadyChanged = delegate {};
	private static Slide EndOfSlideSlide = new BlankSlide(null,"end of presentation");
	public static Action<Slide> OnSlideChanged = delegate { };
	public static Action OnPresentationClosed = delegate { };
	public Presentation Presentation => _presentation;
	private Presentation _presentation = new Presentation();

	public Slide NextSlide => GetNextSlide();
	public Slide CurrentSlide => _presentation.Slides[_currentSlideIndex];
	private int _currentSlideIndex = 0;
	public SlideTimer TotalPresentationTimer => _totalPresentationTimer;
	private readonly SlideTimer _totalPresentationTimer;
	private readonly SlideTimer[] _slideTimers;
	public float Aspect => RowCount / (float)ColumnCount;

	public int RowCount = 30;
	public int ColumnCount = 40;
	private StringBuilder _builder = new StringBuilder();

	public PresentationState()
	{
		_presentation = new Presentation();
		_slideTimers = [];
		_totalPresentationTimer = new SlideTimer();
		_currentSlideIndex = 0;
	}
	public PresentationState(Presentation presentation)
	{
		_presentation = presentation;
		
		//initialize timers
		_totalPresentationTimer = new SlideTimer();
		_slideTimers = new SlideTimer[_presentation.SlideCount];
		for (int i = 0; i < _presentation.SlideCount; i++)
		{
			_slideTimers[i] = new SlideTimer();
		}
		
		_currentSlideIndex = 0;
		if (presentation.SlideCount > 0)
		{
			SetPresentationReady(true);
		}
		else
		{
			Console.WriteLine("Empty Presentation!");
			SetPresentationReady(false);
		}
	}

	public void SetPresentationReady(bool ready)
	{
		if (ready != IsPresentationReady)
		{
			IsPresentationReady = ready;
			OnIsPresentationReadyChanged?.Invoke(ready);
		}
	}

	public void NavigateRelative(int delta)
	{
		if (delta == 0)
		{
			return;
		}

		_currentSlideIndex += delta;
		if (_currentSlideIndex >= _presentation.SlideCount)
		{
			//asdflkj
			_currentSlideIndex = (_presentation.SlideCount - _currentSlideIndex);
		}
		else if (_currentSlideIndex < 0)
		{
			_currentSlideIndex = _presentation.SlideCount + _currentSlideIndex;
		}
		Navigate(_currentSlideIndex);
	}
	private void Navigate(int slide)
	{
		//exit from previous
		if (_currentSlideIndex != slide)
		{
			_presentation.Slides[_currentSlideIndex]?.ExitFromView();
		}
		
		_currentSlideIndex = slide;	
		
		
		//pre-process if necessary.
		if (!_presentation.Slides[_currentSlideIndex].PreProcessed)
		{
			_presentation.Slides[_currentSlideIndex].PreProcess();
		}
		
		OnSlideChanged?.Invoke(_presentation.Slides[_currentSlideIndex]);
		//now displayed, we hope, call the callback.
		_presentation.Slides[_currentSlideIndex].OnRender();
	}
	
	//todo: pre-process all in the background
		//cache things like the youtube video? would it be like keeping a separate webView when we leave and return?
		//a slide could provide a view instead of HTML, and then save it in the Slide data, and use a static one as the fallback.
	public string GetCurrentAsHTML(Rectangle bounds, SlideViewMode currentSpeaker = SlideViewMode.CurrentPresenting)
	{
		return _presentation.Slides[_currentSlideIndex].GetSlideAsHTML(this, bounds, currentSpeaker);
	}

	public string GetPreviewAsHTML(Rectangle bounds, SlideViewMode slideViewMode = SlideViewMode.Preview)
	{
		int preview = _currentSlideIndex + 1;
		if (preview >= _presentation.SlideCount || preview < 0)
		{
			return EndOfSlideSlide.GetSlideAsHTML(this, bounds, slideViewMode);
		}
		return _presentation.Slides[preview].GetSlideAsHTML(this, bounds, slideViewMode);
	}

	public void ClosePresentation()
	{
		SetIsPresenting(false);
		OnPresentationClosed?.Invoke();
	}

	public void SetIsPresenting(bool isPresenting)
	{
		IsPresenting = isPresenting;
		OnIsPresentingChanged?.Invoke(isPresenting);
	}

	public void CallSlideFunction(string slideFunctionName, string data)
	{
		//clean up, validate.
		//if it's a function we manage, then do it.
		//if it's a slide function, shout at the slide to do something about it.
		//if it's a display-side function, then yell into the void and hope the displayer does it.
		var sname = slideFunctionName.ToLower().Trim();
		switch (sname)
		{
			case "playvideo":
				OnSlideFunction?.Invoke("playvideo", data);
				break;
			case "stopvideo":
				OnSlideFunction?.Invoke("stopvideo", data);
				break;
			case "mute":
				OnSlideFunction?.Invoke("mute", data);
				break;
			case "volume":
				OnSlideFunction?.Invoke("volume", data);
				break;
			case "seek":
				OnSlideFunction?.Invoke("seek", data);
				break;
			
		}
		
		CurrentSlide?.OnSlideFunction(sname, data);
	}

	private Slide GetNextSlide()
	{
		var next = _currentSlideIndex + 1;
		if (next < _presentation.SlideCount)
		{
			return _presentation.Slides[next];
		}
		else
		{
			return _presentation.Slides[_currentSlideIndex];
		}
	}
}