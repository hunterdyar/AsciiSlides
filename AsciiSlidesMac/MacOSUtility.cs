using AsciiSlidesCore;
using Eto.Forms;
using MonoMac.AppKit;

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