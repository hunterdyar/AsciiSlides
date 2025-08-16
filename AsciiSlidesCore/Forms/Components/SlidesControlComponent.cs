using System.Runtime.Serialization.Json;
using Eto.Drawing;
using Eto.Forms;

namespace AsciiSlidesCore.Components;

public class SlidesControlComponent : GroupBox
{
	private Button _prevSlideButton;
	private Button _nextSlideButton;
	private ProgressBar _presentationProgressBar;

	public SlidesControlComponent()
	{
		_prevSlideButton = new Button();
		_prevSlideButton.Text = "<";
		_prevSlideButton.Click += (sender, args) =>
		{
			//todo: There should be a repository of Commands that are related to our menu commands. in SlidesManager? PresentationState? 
			SlidesManager.PresentationState?.NavigateRelative(-1);
		};
		_nextSlideButton = new Button();
		_nextSlideButton.Text = ">";
		_nextSlideButton.Click += (sender, args) => { SlidesManager.PresentationState?.NavigateRelative(1); };
		_presentationProgressBar = new ProgressBar();
		_presentationProgressBar.Height = 20;
		
		BackgroundColor = Colors.Gray;
		
		//the slideNumber-1 is just to make it like Floor; and the count-1 is for the 'end of presentation' slide to be ignored.
		PresentationState.OnIsPresentingChanged += b =>
		{
			_presentationProgressBar.MinValue = 0;
			_presentationProgressBar.MaxValue = SlidesManager.PresentationState.Presentation.SlideCount-1;
			_presentationProgressBar.Value = b ? SlidesManager.PresentationState.CurrentSlide.SlideNumber-1 : 0;
		};
		PresentationState.OnSlideChanged += slide =>
		{
			_presentationProgressBar.Value = SlidesManager.PresentationState.CurrentSlide.SlideNumber-1;
		};

		//todo: I don't know what i want this to look like... but i now haave a good idea what i don't want this to look like.
		//can i set max height from within a control?
		
		var table = new TableLayout();
		var buttonRow = new TableRow();
		buttonRow.Cells.Add(_prevSlideButton);
		buttonRow.Cells.Add(_presentationProgressBar);
		buttonRow.Cells.Add(_nextSlideButton);
		table.Rows.Add(buttonRow);
		table.Rows[0].Cells[0].ScaleWidth = false;
		table.Rows[0].Cells[1].ScaleWidth = true;
		table.Rows[0].Cells[2].ScaleWidth = false;

		Content = buttonRow;
	}
}