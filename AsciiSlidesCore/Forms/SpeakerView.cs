using Eto.Drawing;
using Eto.Forms;

namespace AsciiSlidesCore;

public class SpeakerView : PresentationForm
{
	private readonly Scrollable _notesScrollable;
	private readonly Label _notesView;
	private readonly WebView _previewView;
	private readonly Label _timerView;
	private readonly WebView _currentSlideView;

	private readonly Splitter _lrSplitter;
	private readonly Splitter _currentSplitter;
	public SpeakerView(SlidesManager manager, Screen screen, bool inFullScreen) : base(manager, screen, inFullScreen)
	{
		//generally done to not cast color on presenters.
		BackgroundColor = Colors.Black;
		Title = "Presenter View";
			
		_notesView = new Label()
		{
			Text = "Speaker Notes", TextColor = Colors.White, Font = new Font("Courier", 24),Wrap = WrapMode.Word,
			BackgroundColor = new Color(0.1f, 0.1f, 0.1f)
		};
		_previewView = new WebView();
		_currentSlideView = new WebView();
		_currentSlideView.Size = new Size((int)(this.Size.Width*0.6), (int)(this.Size.Height*0.6));
		_timerView = new Label()
		{
			Text = "00:00",
			TextColor = Colors.White
		};

		
		//layout
		_lrSplitter = new Splitter
		{
			Orientation = Orientation.Horizontal,
		};
		
		_notesScrollable = new Scrollable();
		_notesScrollable.Border = BorderType.None;
		_notesScrollable.Content = _notesView;
		_notesScrollable.ExpandContentHeight = true;
		_notesScrollable.ExpandContentWidth = false;
		_lrSplitter.PositionChanged += (s, e) =>
		{
			_notesView.Width = _notesScrollable.VisibleRect.Width;
		};
		_currentSplitter = new Splitter()
		{
			Orientation = Orientation.Vertical,
			Panel1 = _currentSlideView,
			Panel2 = _notesScrollable
		};
		
		_lrSplitter.Panel1 = _currentSplitter;
		var rightbar = new DynamicLayout();
		//right col
		rightbar.BeginVertical();
		rightbar.AddCentered(_previewView, 0, new Size(0,0),false,false);
		rightbar.AddRow(_timerView);
		rightbar.EndVertical();
		_lrSplitter.Panel2 = rightbar;
		
		Padding = new Padding(10);
		Content = _lrSplitter;
		
		//adjust sizes after calculations. (sets inner to outer)
		_notesView.Width = _notesScrollable.VisibleRect.Width;
		_currentSlideView.LoadHtml(SlidesManager.PresentationState.GetCurrentAsHTML(_currentSlideView.Bounds));
		_previewView.LoadHtml(SlidesManager.PresentationState.GetPreviewAsHTML(_previewView.Bounds));

	}

	protected override void OnCurrentSlideChanged(Slide slide)
	{
		//update the thingns.
		
		_notesView.Text = slide.SpeakerNotes;
		//we may or may not have a scrollbar anymore, so this width can grow or shrink from one slide to the next even if the horizontal bar doesn't change.
		_notesView.Width = _notesScrollable.VisibleRect.Width;
		//snap back to top of scrolling
		_notesScrollable.ScrollPosition = new Point(0, 0);
		_currentSlideView.LoadHtml(SlidesManager.PresentationState.GetCurrentAsHTML(_currentSlideView.Bounds));
		_previewView.LoadHtml(SlidesManager.PresentationState.GetPreviewAsHTML(_previewView.Bounds));
	}
}