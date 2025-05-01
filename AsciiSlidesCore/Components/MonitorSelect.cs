using Eto.Forms;
using Button = Eto.Forms.Button;
using Control = Eto.Forms.Control;
using Form = Eto.Forms.Form;
using Label = Eto.Forms.Label;
using Orientation = Eto.Forms.Orientation;
using RadioButton = Eto.Forms.RadioButton;
using Screen = Eto.Forms.Screen;

namespace AsciiSlidesCore.Components;

public class MonitorSelect : StackLayout
{
	private RadioButtonList _rb;
	private List<string> _monitors = new List<string>();
	private int defaultScreen;
	public MonitorSelect()
	{
		_rb = new RadioButtonList()
		{
			DataStore = _monitors
		};
		this.Items.Add(_rb);
		SetValuesFromMonitors();
		
	}

	private void SetValuesFromMonitors()
	{
		_monitors.Clear();
		for (var i = 0; i < Screen.Screens.ToArray().Length; i++)
		{
			var screen = Screen.Screens.ToArray()[i];
			if (screen == Screen.PrimaryScreen)
			{
				defaultScreen = i;
			}
			_monitors.Add(i+" "+screen.ToString());
		}
		Console.WriteLine(_monitors.Count);
	}
	
}