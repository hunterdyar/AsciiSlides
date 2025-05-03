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
		throw new NotImplementedException();
	}
}