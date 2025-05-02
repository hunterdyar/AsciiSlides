using System.ComponentModel;
using Eto.Drawing;
using Eto.Forms;

namespace AsciiSlidesCore.Components;

public class OutputComponent : GroupBox
{
	private DropDown _presentationDropdown;
	private DropDown _speakerDropdown;
	private Button _swapButton;

	private ListItemCollection _options = new ();
	private int primaryDisplayOptionIndex = 0;
	public OutputComponent()
	{
		Text = "Output";
		_presentationDropdown = new DropDown();
		_speakerDropdown = new DropDown();
		PopulateOptions();
		

		//layout
		var layout = new TableLayout()
		{
			Spacing = new Size(3, 5),
			Rows =
			{
				new TableRow(
					new Label() { Text = "Presentation" },
					null,
					new Label() { Text = "Speaker View" }
				),
				new TableRow(
					_presentationDropdown,
					_swapButton,
					_speakerDropdown
				)
			}
		};
		Content = layout;
	}

	private void PopulateOptions()
	{
		_options.Clear();
		int screenCount = Screen.Screens.ToArray().Length;
		for (var i = 0; i < screenCount; i++)
		{
			var screen = Screen.Screens.ToArray()[i];
			if (screen == Screen.PrimaryScreen)
			{
				primaryDisplayOptionIndex = i;
			}
			_options.Add(screen.ToString());
		}
		_options.Add("Windowed");

		_speakerDropdown.DataStore = _options;
		_speakerDropdown.SelectedIndex = primaryDisplayOptionIndex;
		_presentationDropdown.DataStore = _options;
		if (screenCount == 0)
		{
			_presentationDropdown.SelectedIndex = 0;
		}else 
		if (screenCount == 1)
		{
			_presentationDropdown.SelectedIndex = primaryDisplayOptionIndex;
		}else if (screenCount > 1)
		{
			_presentationDropdown.SelectedIndex = 0;
		}
	}
}