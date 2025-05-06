using AsciiSlidesCore;
using Eto.Drawing;
using Eto.Forms;
using MonoMac.AppKit;
using MonoMac.CoreGraphics;
using RectangleF = System.Drawing.RectangleF;

namespace AsciiSlidesMac;

public class MacOSUtility : OSUtility
{
	public override bool ToggleFullscreen(Form form, Screen screen, bool fullscreen)
	{
		var nativeView = form.ToNative();
		nativeView.CollectionBehavior = NSWindowCollectionBehavior.Default |
		                                NSWindowCollectionBehavior.FullScreenPrimary |
		                                NSWindowCollectionBehavior.CanJoinAllSpaces |
		                                NSWindowCollectionBehavior.FullScreenAllowsTiling;
		nativeView.ToggleFullScreen(nativeView);
		return fullscreen;
	}

	public override Bitmap ViewToBitmap(WebView view)
	{
		var n = view.ToNative();
		var bm = n.BitmapImageRepForCachingDisplayInRect(new CGRect(view.Bounds.X, view.Bounds.Y, view.Bounds.Width,
			view.Bounds.Height));
		throw new NotImplementedException("I haven't figured out the tobitmap thing on mac.");
	}

	override public MonitorInfo[] GetMonitors()
	{
		NSScreen[] screens = NSScreen.Screens;
		Screen[] allEto = Screen.Screens.ToArray();
		if (screens.Length != allEto.Length)
		{
			throw new Exception("eto and native mismatch?");
		}
		
		MonitorInfo[] result = new MonitorInfo[screens.Length];
		for (var i = 0; i < screens.Length; i++)
		{
			result[i] = new MonitorInfo(allEto[i], screens[i].Description);
		}

		return result;
	}

	public override void SetSettingsKey(string key, string value)
	{
		
	}

	public override bool TryGetSettingsKey(string key, out string value)
	{
		value = null;
		return false;
	}

	public override void SaveSettingsKeys()
	{
		
	}
}