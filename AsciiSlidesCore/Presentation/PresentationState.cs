using System.Text;
using Rectangle = Eto.Drawing.Rectangle;

namespace AsciiSlidesCore;

public class PresentationState
{
	public static bool IsPresentationReady = false;
	public static Action<bool> OnIsPresentationReadyChanged = delegate {};
	public static Action<Presentation> OnPresentationLoaded = delegate { };

	private static Slide EndOfSlideSlide = new BlankSlide("end of slide");
	public static Action<Slide> OnSlideChanged = delegate { };
	public Presentation Presentation => _presentation;
	private Presentation _presentation = new Presentation();
	
	public Slide CurrentSlide => _presentation.Slides[_currentSlideIndex];
	private int _currentSlideIndex = 0;
	public float Aspect => RowCount / (float)ColumnCount;
	public int RowCount = 30;
	public int ColumnCount = 40;
	private StringBuilder _builder = new StringBuilder();

	public PresentationState()
	{
		_presentation = new Presentation();
		_currentSlideIndex = 0;
	}
	public PresentationState(Presentation presentation)
	{
		_presentation = presentation;
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
		}else if (_currentSlideIndex < 0)
		{
			_currentSlideIndex = _presentation.SlideCount + _currentSlideIndex;
		}
		OnSlideChanged?.Invoke(_presentation.Slides[_currentSlideIndex]);
	}
	
	public string GetCurrentAsHTML(Rectangle bounds)
	{
		return _presentation.Slides[_currentSlideIndex].GetSlideAsHTML(this, bounds, false);
	}

	public string GetPreviewAsHTML(Rectangle bounds)
	{
		int preview = _currentSlideIndex + 1;
		if (preview >= _presentation.SlideCount || preview < 0)
		{
			return EndOfSlideSlide.GetSlideAsHTML(this,bounds,false);
		}
		return _presentation.Slides[_currentSlideIndex].GetSlideAsHTML(this, bounds, false);
	}
}