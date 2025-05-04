using Eto.Drawing;
using Eto.Forms;

namespace AsciiSlidesCore;

public abstract class PresentationForm : Form
{
	protected Screen _screen;
	public bool IsFullscreen => _isFullscreen;
	protected bool _isFullscreen = false;
	
	public PresentationForm(Screen screen, bool inFullscreen)
	{
		//Topmost = inFullscreen;
		_screen = screen;

		Show();

		SetFullscreen(screen, inFullscreen);
		AsciiSlidesCore.InputHandler.RegisterFormAsSlideController(this);

		//handle our own shortcuts.
		this.KeyDown += OnKeyDown;
		this.KeyUp += OnKeyUp;
		//handle events we care about
		PresentationState.OnSlideChanged += OnCurrentSlideChanged;

		//unregsister
		this.Closed += (sender, args) =>
		{
			//unhandle shortcuts
			this.KeyDown -= OnKeyDown;
			this.KeyUp -= OnKeyUp;
			//unhandle events

			PresentationState.OnSlideChanged -= OnCurrentSlideChanged;
		};
		//on resizing.... registering last to prevent multiple 
		this.LogicalPixelSizeChanged += (sender, args) => { ResizePanel(); };
	}

	protected virtual void OnCurrentSlideChanged(Slide obj)
	{
		
	}

	protected virtual void OnKeyUp(object? sender, KeyEventArgs e)
	{
		// Console.WriteLine("Display Hook OnKeyDown: "+e.Key.ToString());
		if (Configuration.ExitKey.Contains(e.Key))
		{
			Close();
		}
		else if (e.Key == Configuration.ToggleFullscreen)
		{
			SetFullscreen(_screen, !_isFullscreen);
		}
	}

	protected virtual void OnKeyDown(object? sender, KeyEventArgs e)
	{
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