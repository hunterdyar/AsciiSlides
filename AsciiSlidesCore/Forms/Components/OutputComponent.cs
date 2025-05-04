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

	private List<MonitorInfo> _options = new ();
	private int primaryDisplayOptionIndex = 0;
	private SlidesManager _slidesManager;
	private int _windowedIndex;
	private int _noneIndex = 0;

	public OutputComponent(SlidesManager slidesManager)
	{
		_slidesManager = slidesManager;
		Text = "Output";
		
		//Create Components
		_presentationDropdown = new DropDown();
		_presentationDropdown.DropDownClosed += (sender, args) =>
		{
			PresentationDropdownOnDropDownClosed(_presentationDropdown.SelectedIndex);
		};
		
		_speakerDropdown = new DropDown();
		_speakerDropdown.DropDownClosed += (sender, args) =>
		{
			SpeakerDropdownOnDropDownClosed(_speakerDropdown.SelectedIndex);
		};
		_swapButton = new Button { Text = "Swap" };
		_presentButton = new Button { Text = "Present", Height = 25 };
		var PresentCommand = new Command()
		{
			MenuText = "Present",
			ToolTip = "Launch Presentation",
			Shortcut = Application.Instance.CommonModifier | Keys.P
		};
		var SwapOutputsCommand = new Command()
		{
			MenuText = "Swap Displays",
			ToolTip = "Swap Outputs",
			Shortcut = Application.Instance.CommonModifier | Keys.P
		};
		PresentCommand.Executed += (sender, args) => { _slidesManager.LaunchPresentation(this.DisplaySelection); };
		SwapOutputsCommand.Executed += SwapOutputsCommandOnExecuted;
		_presentButton.Command = PresentCommand;
		_swapButton.Command = SwapOutputsCommand;
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
				),
				new TableRow(
					_presentButton
					)
				{
					
				}
			}
		};
		Content = layout;
	}

	//todo: Right now, SelectedIndex and DisplaySelection object are manually/externally sync'd. I don't love it.
	//I also don't love keeping the Windowed and None options in the same list... The list should be a list of Option objects and Bound to the dropdown form. one source of truth and data flow.
	private void SwapOutputsCommandOnExecuted(object? sender, EventArgs e)
	{
		//swap indices
		int speaker = _speakerDropdown.SelectedIndex;
		int display = _presentationDropdown.SelectedIndex;
		(speaker, display) = (display, speaker);
		//
		//swap the display...
		DisplaySelection.Swap();
		
		//finish the swap
		_speakerDropdown.SelectedIndex = speaker;
		_presentationDropdown.SelectedIndex = display;
	}

	private void SpeakerDropdownOnDropDownClosed(int newIndex)
	{
		if (newIndex == _windowedIndex)
		{
			//todo: pull this from a config file. Where it was when it last closed.
			DisplaySelection.SpeakerNotesScreen = MonitorInfo.PrimaryScreen;
			DisplaySelection.SpeakerNotesOutputType = OutputType.Windowed;
			return;
		}
		else if (newIndex == _noneIndex)
		{
			DisplaySelection.SpeakerNotesScreen = null;
			DisplaySelection.SpeakerNotesOutputType = OutputType.None;
			return;
		}

		SetDropdownToDifferentMonitor(_speakerDropdown, _presentationDropdown);
		int i = newIndex;
		//todo: cache screens, reload at appropriate times.
		var s = Screen.Screens.Count();
		if (i >= 0 && i < s)
		{
			DisplaySelection.SpeakerNotesScreen = _options[i];
		}
		else
		{
			throw new Exception("Not a valid screen/monitor index, but it must be?");
		}
	}

	private void PresentationDropdownOnDropDownClosed(int newIndex)
	{

		if (newIndex == _windowedIndex)
		{
			DisplaySelection.DisplayScreen = MonitorInfo.PrimaryScreen;
			DisplaySelection.DisplayOutputType = OutputType.Windowed;
			return;
		}
		else if (newIndex == _noneIndex)
		{
			DisplaySelection.DisplayScreen = null;
			DisplaySelection.DisplayOutputType = OutputType.None;
			return;
		}
		
		SetDropdownToDifferentMonitor(_presentationDropdown, _speakerDropdown);
		int i = newIndex;
		
		var s = Screen.Screens.Count();
		if (i >= 0 && i < s)
		{
			DisplaySelection.DisplayScreen = _options[_presentationDropdown.SelectedIndex];
			DisplaySelection.DisplayOutputType = OutputType.Fullscreen;
		}
		else
		{
			throw new Exception("Not a valid screen/monitor index, but it must be?");
		}
		
	}

	private void SetDropdownToDifferentMonitor(DropDown changed, DropDown other)
	{
		var selectedIndex = changed.SelectedIndex;
		//we can have two windowed displays!
		if (selectedIndex == _windowedIndex)
		{
			return;
		}

		if (selectedIndex == _noneIndex)
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
				//we just set the speaker to be the output instead of the speaker view. That's probably wrong, but it's allowed.
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
		DisplaySelection = new DisplaySelection();
		_options.Clear();
		var monitors = OSUtility.Instance.GetMonitors();
		int screenCount = 0;
		for (var i = 0; i < monitors.Length; i++)
		{
			var screen = monitors[i].Screen;
			if (screen == Screen.PrimaryScreen)
			{
				primaryDisplayOptionIndex = i;
			}
			_options.Add(monitors[i]);
			screenCount++;
		}
		_windowedIndex = _options.Count;
		_options.Add(MonitorInfo.Windowed);
		_noneIndex = _options.Count;
		_options.Add(MonitorInfo.None);

		_speakerDropdown.DataStore = _options;
		_speakerDropdown.SelectedIndex = primaryDisplayOptionIndex;
		_presentationDropdown.DataStore = _options;

		if (screenCount == 0)
		{
			_presentationDropdown.SelectedIndex = _noneIndex;
			_speakerDropdown.SelectedIndex = _noneIndex;
			
			DisplaySelection.DisplayOutputType = OutputType.Windowed;
			DisplaySelection.SpeakerNotesOutputType = OutputType.Windowed;
		}else 
		if (screenCount == 1)
		{
			_presentationDropdown.SelectedIndex = primaryDisplayOptionIndex;
			_speakerDropdown.SelectedIndex = _noneIndex;

			DisplaySelection.DisplayScreen = _options[primaryDisplayOptionIndex];

			if (DisplaySelection.DisplayScreen == null)
			{
				throw new Exception("Display Screen Not Set! Why don't we know the primary screen?");
			}
			
			DisplaySelection.SpeakerNotesOutputType = OutputType.None;
			DisplaySelection.DisplayOutputType = OutputType.Fullscreen;

		}else if (screenCount > 1)
		{
			DisplaySelection.SpeakerNotesScreen = _options[primaryDisplayOptionIndex];
			_presentationDropdown.SelectedIndex = primaryDisplayOptionIndex;
			
			int externalScreen = GetNextOutputOption(primaryDisplayOptionIndex);
			DisplaySelection.DisplayScreen = _options[externalScreen];
			_speakerDropdown.SelectedIndex = externalScreen;
			
			DisplaySelection.DisplayOutputType = OutputType.Fullscreen;
			DisplaySelection.SpeakerNotesOutputType = OutputType.Fullscreen;
		}
		
	}
}