using AsciiSlidesCore.Components;
using Eto.Drawing;
using Eto.Forms;

namespace AsciiSlidesCore;

public class SpeakerView : PresentationForm
{
	private readonly Scrollable _notesScrollable;
	private readonly Label _notesView;
	private readonly WebView _previewView;
	private readonly TimerComponent _timerView;
	private readonly ImageView _imageView;

	private readonly Splitter _lrSplitter;
	private readonly Splitter _currentSplitter;
	public SpeakerView(SlidesManager manager, Screen screen, bool inFullScreen) : base(manager, screen, inFullScreen)
	{
		_manager = manager;
		//generally done to not cast color on presenters.
		BackgroundColor = Colors.Black;
		Title = "Presenter View";
			
		_notesView = new Label()
		{
			Text = "Speaker Notes", TextColor = Colors.White, Font = new Font("Courier", 24),Wrap = WrapMode.Word,
			BackgroundColor = new Color(0.1f, 0.1f, 0.1f)
		};
		_previewView = new WebView();
		_imageView = new ImageView();
		_imageView.Size = this.Size * 2/3;
		_timerView = new TimerComponent("Time");

		
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
			Panel1 = _imageView,
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
		//SlidesManager.PresentationState.CurrentSlide.RenderTo(SlidesManager.PresentationState, _currentSlideView, SlideViewMode.CurrentSpeaker);
		SlidesManager.PresentationState.CurrentSlide.RenderTo(SlidesManager.PresentationState, _previewView, SlideViewMode.Preview);

		if (_manager.Display != null)
		{
			//_imageView.Image = OSUtility.Instance.ViewToBitmap(_manager.Display.View);
		}
	}

	protected override void OnCurrentSlideChanged(Slide slide)
	{
		//update the things.
		
		_notesView.Text = slide.SpeakerNotes;
		//we may or may not have a scrollbar anymore, so this width can grow or shrink from one slide to the next even if the horizontal bar doesn't change.
		_notesView.Width = _notesScrollable.VisibleRect.Width;
		//snap back to top of scrolling
		_notesScrollable.ScrollPosition = new Point(0, 0);
	//X	SlidesManager.PresentationState.CurrentSlide.RenderTo(SlidesManager.PresentationState, _currentSlideView, SlideViewMode.CurrentSpeaker);
		SlidesManager.PresentationState.CurrentSlide.RenderTo(SlidesManager.PresentationState, _previewView, SlideViewMode.Preview);
		_imageView.BackgroundColor = Colors.DarkGray;
	}

	public override void Init()
	{
		_manager.Display.OnRenderComplete += OnDisplayRenderComplete;
		base.Init();
	}

	protected override void OnClose()
	{
		if (_manager.Display != null)
		{
			_manager.Display.OnRenderComplete -= OnDisplayRenderComplete;
		}
	}
	private void OnDisplayRenderComplete(WebView renderingView)
	{
		CapturePresentation();
	}

	class BitmapTaskContainer
	{
		public Bitmap Image { get; set; }
	}
	

	private void CapturePresentation()
	{
		_manager.Display.Capture((b) =>
		{
			_imageView.Image = b;

		});
	}
}