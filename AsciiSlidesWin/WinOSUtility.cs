using System.Configuration;
using AsciiSlidesCore;
using Eto.Forms;
using Form = Eto.Forms.Form;
using Rectangle = Eto.Drawing.Rectangle;
namespace AsciiSlidesWin;

public class WinOSUtility : OSUtility
{
	public override bool ToggleFullscreen(Form view, Eto.Forms.Screen screen, bool fullscreen)
	{
		if (fullscreen)
		{
			view.WindowStyle = WindowStyle.None;
			view.Topmost = true;
			view.WindowState = WindowState.Maximized;
			return true;
		}
		else
		{
			view.WindowState = WindowState.Normal;
			view.WindowStyle = WindowStyle.Default;
			view.Bounds = new Rectangle(screen.Bounds / 2);
			return false;
		}
	}

	public override MonitorInfo[] GetMonitors()
	{
		var allScreens = AsciiSlidesWin.Screen.AllScreens.ToArray();
		MonitorInfo[] allMonitors = new MonitorInfo[allScreens.Length];
		var etoscreens = Eto.Forms.Screen.Screens.ToArray();
		for (int i = 0; i < allScreens.Length; i++)
		{
			//todo: check assumption that the Winscreens and the EtoForm screens are aligned/the same. 
			//I fear it won't be when things like plug/unplug happen during runtime because of cacheing.
			allMonitors[i] = new MonitorInfo(etoscreens[i], allScreens[i].DeviceName);
			
		}

		return allMonitors;
	}

	#region Settings

	

	private System.Configuration.Configuration _configuration;

	private void LazyGetConfig()
	{
		if (_configuration == null)
		{
			_configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
		}
	}
	public override void SetSettingsKey(string key, string value)
	{
		LazyGetConfig();
		if (_configuration != null)
		{
			_configuration.AppSettings.Settings[key].Value = value;
		}
	}

	public override bool TryGetSettingsKey(string key, out string value)
	{
		LazyGetConfig();
		if (_configuration != null)
		{
			var kvp = _configuration.AppSettings.Settings[key];
			if (kvp != null)
			{
				value= kvp.Value;
				return true;
			}
		}
		value = null;
		return false;
	}

	public override void SaveSettingsKeys()
	{
		LazyGetConfig();
		if (_configuration != null)
		{
			_configuration.Save(ConfigurationSaveMode.Minimal);
		}	
	}

	#endregion

}