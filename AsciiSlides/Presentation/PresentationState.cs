namespace AsciiSlides;

public class PresentationState
{
	private Presentation _presentation = new Presentation();
	
	public int CurrentSlide;
	private int _currentSlide => CurrentSlide;
	public int RowCount = 60;
	public int ColumnCount = 80;
	public static Action OnCurrentSlideChanged;

	public void NavigateRelative(int delta)
	{
		if (delta == 0)
		{
			return;
		}
	}
}