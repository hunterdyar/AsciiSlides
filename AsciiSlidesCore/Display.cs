using System.Runtime.CompilerServices;
using Eto.Drawing;
using Eto.Forms;


using Form = Eto.Forms.Form;
using KeyEventArgs = Eto.Forms.KeyEventArgs;
using Keys = Eto.Forms.Keys;
using Label = Eto.Forms.Label;
using Panel = Eto.Forms.Panel;
using Rectangle = Eto.Drawing.Rectangle;

#if UNIX
#endif

namespace AsciiSlidesCore;

/// <summary>
/// Display is a window that renders the current Presentation with a given PresentationState.
/// </summary>
public class Display : Form
{
    public bool IsFullscreen => _isFullscreen;
    private bool _isFullscreen;

    private WebView _webPanel;

    //ascii character row/column counts set by the presentation, with per-slide overrides.

    public Display(bool inFullscreen)
    {
        // WindowState = WindowState.Normal;
        // Bounds = Screen.PrimaryScreen.Bounds;
        // Topmost = true;
        Topmost = inFullscreen;
        //todo: get the aspect ratio of the presentation, use a centered rectangle that large
        Bounds = new Rectangle(Screen.Bounds * 0.8f);
        Maximizable = true;
        Show();
        Focus();
        //Maximize();

        _webPanel = new Eto.Forms.WebView()
        {
            BackgroundColor = Colors.LightYellow,
            Width = 40,
            Height = 20,
            BrowserContextMenuEnabled = false,
        };
        this.Content = _webPanel;

        Console.WriteLine("Created Display.");
        //fullscreen
        SetFullscreen(inFullscreen);
        _webPanel.LoadHtml(SlidesManager.PresentationState.GetCurrentAsHTML(this.Bounds));
        //register
        AsciiSlidesCore.EventHandler.RegisterFormAsSlideController(this);
        
        //handle our own shortcuts.
        this.KeyDown += OnKeyDown;
        this.KeyUp += OnKeyUp;
        //handle events we care about
        PresentationState.OnCurrentSlideChanged += OnCurrentSlideChanged;

        //unregsister
        this.Closed += (sender, args) =>
        {
            //unhandle shortcuts
            this.KeyDown -= OnKeyDown;
            this.KeyUp -= OnKeyUp;
            //unhandle events
            PresentationState.OnCurrentSlideChanged -= OnCurrentSlideChanged;
        };
    //on resizing.... registering last to prevent multiple 
    this.LogicalPixelSizeChanged += (sender, args) =>
        {
            ResizePanel();
        };
        
    }

    private void OnCurrentSlideChanged()
    {
        _webPanel.LoadHtml(SlidesManager.PresentationState.GetCurrentAsHTML(this.Bounds)); 
    }

    private void ResizePanel()
    {
        Console.WriteLine("Resizing Panel.");
        //todo: right now we just reload everything, but once we support video embeddes, will have to do this by running some JS or such to dynamically change the style properties and not do a reload.
        _webPanel.LoadHtml(SlidesManager.PresentationState.GetCurrentAsHTML(this.Bounds));
    }

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        // Console.WriteLine("Display Hook OnKeyDown: "+e.Key.ToString());
        if (Configuration.ExitKey.Contains(e.Key))
        {
            Close();
        }
        // else if (e.Key == Configuration.ToggleFullscreen)
        // {
        //     SetFullscreen(!_isFullscreen);
        // }
        // if (e.Control)
        // {
        //     //Cycle Down list of screens.
        //     MoveScreens(-1);
        // }
        
    }

    private void OnKeyUp(object? sender, KeyEventArgs e)
    {

    }
    
    private void MoveScreens(int delta)
    {
        //untested, I only have 1 screen right now.
        var screens = Eto.Forms.Screen.Screens.ToList();
        var current = screens.IndexOf(this.Screen);
        var next = current + delta;
        
        //cycle
        if (next < 0)
        {
            next += screens.Count;
        }else if (next >= screens.Count)
        {
            next -= screens.Count;
        }
        
        this.Bounds = new Rectangle(screens[next].Bounds);
        ResizePanel();
    }

    public void LeaveFullscreen()
    {
        if (_isFullscreen)
        {
            SetFullscreen(false);
        }
    }

    public void EnterFullscreen()
    {
        if (!_isFullscreen)
        {
            SetFullscreen(true);
        }
    }
    private void SetFullscreen(bool fullscreen = true)
    {
        OSUtility.Instance.ToggleFullscreen(this, fullscreen);
    }
    
    
}