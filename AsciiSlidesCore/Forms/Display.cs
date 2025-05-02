using Eto.Drawing;
using Eto.Forms;

using Form = Eto.Forms.Form;
using KeyEventArgs = Eto.Forms.KeyEventArgs;

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
    private bool _isFullscreen = false;
    private WebView _webPanel;
    
    public Display(bool inFullscreen)
    {
        Title = "Presentation";
        Topmost = inFullscreen;
        Maximizable = true;
        Show();
        Focus();

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
        Title = "Slide " + SlidesManager.PresentationState.CurrentSlide.SlideNumber + "/" + SlidesManager.PresentationState.Presentation.SlideCount;
        //register
        AsciiSlidesCore.EventHandler.RegisterFormAsSlideController(this);
        
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
    this.LogicalPixelSizeChanged += (sender, args) =>
        {
            ResizePanel();
        };
        
    }

    private void OnCurrentSlideChanged(Slide slide)
    {
        _webPanel.LoadHtml(SlidesManager.PresentationState.GetCurrentAsHTML(this.Bounds));
        Title = "Slide " + slide.SlideNumber + "/" + SlidesManager.PresentationState.Presentation.SlideCount;
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

    private void SetFullscreen(bool fullscreen = true)
    {
        _isFullscreen = OSUtility.Instance.ToggleFullscreen(this, fullscreen);
    }
    
    
}