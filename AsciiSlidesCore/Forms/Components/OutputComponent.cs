using System.ComponentModel;
using Eto.Drawing;
using Eto.Forms;

namespace AsciiSlidesCore.Components;

public class OutputComponent : GroupBox
{
	public DisplaySelection DisplaySelection; 
	
	private DropDown _presentationDropdown;
	private DropDown _speakerDropdown;
	private Button _swapButton;
	private Button _presentButton;

	private ListItemCollection _options = new ();
	private int primaryDisplayOptionIndex = 0;
	private SlidesManager _slidesManager;
	private int _windowedIndex;

	public OutputComponent(SlidesManager slidesManager)
	{
		_slidesManager = slidesManager;
		Text = "Output";
		_presentationDropdown = new DropDown();
		_presentationDropdown.DropDownClosed += PresentationDropdownOnDropDownClosed;
		_speakerDropdown = new DropDown();
		_speakerDropdown.DropDownClosed += SpeakerDropdownOnDropDownClosed;
		_swapButton = new Button { Text = "Swap" };
		_presentButton = new Button { Text = "Present", Height = 25 };
		var PresentCommand = new Command()
		{
			MenuText = "Present",
			ToolTip = "Launch Presentation",
			Shortcut = Application.Instance.CommonModifier | Keys.P
		};
		PresentCommand.Executed += (sender, args) => { _slidesManager.LaunchPresentation(); };
		_presentButton.Command = PresentCommand;
		PopulateOptions();
		DisplaySelection = new DisplaySelection();
		if (_options.Count == 0)
		{
			
		}
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
				),
				new TableRow(
					_presentButton
					)
			}
		};
		Content = layout;
	}

	private void SpeakerDropdownOnDropDownClosed(object? sender, EventArgs e)
	{
		SetDropdownToDifferentMonitor(_speakerDropdown, _presentationDropdown);	
	}

	private void PresentationDropdownOnDropDownClosed(object? sender, EventArgs e)
	{
		SetDropdownToDifferentMonitor(_presentationDropdown, _speakerDropdown);
	}

	private void SetDropdownToDifferentMonitor(DropDown changed, DropDown other)
	{
		var selectedIndex = changed.SelectedIndex;
		//we can have two windowed displays!
		if (selectedIndex == _windowedIndex)
		{
			return;
		}

		//if they are not windowed and we are not windowed, try to become different monitors.
		if (selectedIndex == other.SelectedIndex &&
		    other.SelectedIndex != _windowedIndex)
		{
			if (_options.Count <= 1)
			{
				return;
			}

			if (_options.Count == 2)
			{
				//uh oh! only one monitor!
				//we just set the speaker to be the output instead of the presenter view. That's probably wrong, but it's allowed.
				other.SelectedIndex = _windowedIndex;
			}

			//if two ot more monitors, just go to the next available output
			if (_options.Count > 2)
			{
				other.SelectedIndex = GetNextOutputOption(other.SelectedIndex);
			}
		}
	}

	private int GetNextOutputOption(int f)
	{
		int original = f;
		for (int i = 0; i < _options.Count; i++)
		{
			f++;
			if (f >= _options.Count)
			{
				f = 0;
			}

			if (f == _windowedIndex)
			{
				continue;
			}

			if (f == original)
			{
				continue;
			}

			return f;
		}

		return f;
	}
	
	private void PopulateOptions()
	{
		_options.Clear();
		var monitors = OSUtility.Instance.GetMonitors();
		
		int screenCount = Screen.Screens.ToArray().Length;
		for (var i = 0; i < monitors.Length; i++)
		{
			var screen = Screen.Screens.ToArray()[monitors[i].screenIndex];
			if (screen == Screen.PrimaryScreen)
			{
				primaryDisplayOptionIndex = i;
			}
			_options.Add(monitors[i].name);
		}
		_options.Add("Windowed");
		_windowedIndex = _options.Count;

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