using System.Windows.Forms;
using AsciiSlidesCore;
using Eto.Forms;
using Eto.Wpf;
using Form = Eto.Forms.Form;

namespace AsciiSlidesWin;

public class WinOSUtility : OSUtility
{
	public override bool ToggleFullscreen(Form view, bool fullscreen)
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
			return false;
		}
	}
}