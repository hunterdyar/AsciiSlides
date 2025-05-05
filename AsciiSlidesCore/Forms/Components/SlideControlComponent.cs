using Eto.Forms;

namespace AsciiSlidesCore.Components;

public class SlideControlComponent : GroupBox
{
	private SlidesManager _slidesManager;
	
	private Button _closeButton;
	
	public SlideControlComponent(SlidesManager manager)
	{
		_slidesManager = manager;
		Text = "Slide Control";
		
		//Progress Bar
		
		//Next Slide
		//Previous Slide
		
		//Reload Slide (restart video)
		//Close Presentation
		_closeButton = new Button { Text = "Close Presentation" };
		_closeButton.Command = _slidesManager.CloseCommand;
		_closeButton.Enabled = PresentationState.IsPresenting;
		PresentationState.OnIsPresentingChanged += b =>
		{
			_closeButton.Enabled = b;
		};

		var layout = new DynamicLayout();
		layout.AddRow(_closeButton);
		Content = layout;
	}
}