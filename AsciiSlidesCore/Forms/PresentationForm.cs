using Eto.Drawing;
using Eto.Forms;

namespace AsciiSlidesCore;

public class PresentationForm : Form
{
	protected Screen _screen;
	public bool IsFullscreen => _isFullscreen;
	protected bool _isFullscreen = false;
	
	public PresentationForm(Screen screen, bool inFullscreen)
	{
		Topmost = inFullscreen;
		_screen = screen;

		Show();

		SetFullscreen(screen, inFullscreen);
		AsciiSlidesCore.EventHandler.RegisterFormAsSlideController(this);

	}

	protected void MoveScreens(int delta)
	{
		//untested, I only have 1 screen right now.
		var screens = Eto.Forms.Screen.Screens.ToList();
		var current = screens.IndexOf(this.Screen);
		var next = current + delta;

		//cycle
		if (next < 0)
		{
			next += screens.Count;
		}
		else if (next >= screens.Count)
		{
			next -= screens.Count;
		}

		this.Bounds = new Rectangle(screens[next].Bounds);
		ResizePanel();
	}

	protected void SetFullscreen(Screen screen, bool fullscreen = true)
	{
		_screen = screen;
		_isFullscreen = OSUtility.Instance.ToggleFullscreen(this, screen, fullscreen);
	}

	protected virtual void ResizePanel()
	{
		
	}
}