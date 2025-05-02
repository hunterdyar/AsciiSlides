using Eto.Drawing;
using Eto.Forms;

namespace AsciiSlidesCore;

public class PresenterView : Form
{
	private readonly Label _notesView;
	private readonly WebView _previewView;
	private readonly Label _timerView;
	public PresenterView()
	{
		//generally done to not cast color on presenters.
		BackgroundColor = Colors.Black;
		Title = "Presenter View";
			
		_notesView = new Label() { Text = "Speaker Notes", TextColor = Colors.White };
		_previewView = new WebView();
		_timerView = new Label()
		{
			Text = "00:00",
			TextColor = Colors.White
		};
		//register events
		PresentationState.OnSlideChanged += OnCurrentSlideChanged;

		Closed += (sender, args) =>
		{
			PresentationState.OnSlideChanged -= OnCurrentSlideChanged;
		};
		
		//layout
		var layout = new DynamicLayout();
		layout.BeginHorizontal();
		layout.Add(_timerView);
		layout.AddSpace();
		layout.Add(_previewView);
		layout.EndHorizontal();
		layout.BeginVertical();
		layout.Add(_notesView);
		Content = layout;

		EventHandler.RegisterFormAsSlideController(this);

		Show();
	}

	private void OnCurrentSlideChanged(Slide slide)
	{
		_notesView.Text = slide.SpeakerNotes;
		_previewView.LoadHtml(SlidesManager.PresentationState.GetPreviewAsHTML(_previewView.Bounds));
	}
}