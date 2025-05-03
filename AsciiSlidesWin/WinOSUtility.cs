using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Shapes;
using AsciiSlidesCore;
using Eto.Forms;
using Eto.Wpf;
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
		for (int i = 0; i < allScreens.Length; i++)
		{
			allMonitors[i] = new MonitorInfo();
			allMonitors[i].name = allScreens[i].DeviceName;
			allMonitors[i].screenIndex = i;
		}

		return allMonitors;
	}
}