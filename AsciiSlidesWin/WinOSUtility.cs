
using System.Configuration;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using AsciiSlidesCore;
using Eto;
using Eto.Drawing;
using Eto.Forms;
using Eto.Wpf.Forms;
using Eto.Wpf.Forms.Controls;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using Application = Eto.Forms.Application;
using Form = Eto.Forms.Form;
using Point = Eto.Drawing.Point;
using Rectangle = Eto.Drawing.Rectangle;
using Window = System.Windows.Window;
using WindowState = Eto.Forms.WindowState;
using WindowStyle = Eto.Forms.WindowStyle;

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

	public override async Task CaptureWebViewAsync(WebView webView, Action<Bitmap> onCaptureCompleted)
	{
		// Ensure we're on the UI thread
		Application.Instance.EnsureUIThread();

		try
		{
			// Get the native WebView2 control from the Eto WebView
			var h = webView.Handler as WebView2Handler;
			var nativeControl = h.Control as WebView2;

			if (nativeControl == null)
			{
				throw new InvalidOperationException("The WebView's native control is not a WebView2 control.");
			}

			// Start the async capture process
			using var memoryStream = new MemoryStream();
			await nativeControl.CoreWebView2.CapturePreviewAsync(CoreWebView2CapturePreviewImageFormat.Png, memoryStream);

			// Reset the stream position to beginning
			memoryStream.Position = 0;
			// Create an Eto Bitmap from the memory stream
			var bitmap = new Bitmap(memoryStream);
			onCaptureCompleted(bitmap);
		}
		catch (Exception ex)
		{
			Console.WriteLine("Webview Capture Failed. Interupted? : "+ex);
			//throw new Exception("WebView capture failed", ex);
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
