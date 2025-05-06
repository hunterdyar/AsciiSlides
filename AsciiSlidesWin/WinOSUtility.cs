using System.Configuration;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AsciiSlidesCore;
using Eto.Drawing;
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
			view.Bounds = new Rectangle(screen.Bounds);
			view.WindowState = WindowState.Maximized;
			return true;
		}
		else
		{
			view.WindowStyle = WindowStyle.Default;
			view.WindowState = WindowState.Normal;
			view.Topmost = false;
			view.Bounds = new Rectangle((int)(screen.Bounds.Left + screen.Bounds.Width / 4),
				(int)(screen.Bounds.Top + screen.Bounds.Height / 4),
				(int)(screen.Bounds.Width / 2),
				(int)(screen.Bounds.Height / 2));
			return false;
		}
	}


	public override Bitmap ViewToBitmap(WebView view)
	{
		var n = view.ToNative();
		RenderTargetBitmap rtb = new RenderTargetBitmap((int)n.ActualWidth, (int)n.ActualHeight, 96, 96,
			PixelFormats.Pbgra32);
		rtb.Render(n);
		PngBitmapEncoder png = new PngBitmapEncoder();
		png.Frames.Add(BitmapFrame.Create(rtb));
		MemoryStream stream = new MemoryStream();
		png.Save(stream);
		Eto.Drawing.Bitmap bitmap = new Bitmap(stream);
		return bitmap;
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
			if (_configuration.AppSettings.Settings[key] == null)
			{
				_configuration.AppSettings.Settings.Add(key,value);
			}else
			{
				_configuration.AppSettings.Settings[key].Value = value;
			}
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
				value = kvp.Value;
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
