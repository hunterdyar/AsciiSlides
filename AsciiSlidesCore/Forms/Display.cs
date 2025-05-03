using Eto.Drawing;
using Eto.Forms;

using Form = Eto.Forms.Form;
using KeyEventArgs = Eto.Forms.KeyEventArgs;

using Rectangle = Eto.Drawing.Rectangle;

#if UNIX
#endif

namespace AsciiSlidesCore;

/// <summary>
/// Display is a window (the fullscreen presentation) that renders the current Presentation using a webview, from a given PresentationState.
/// </summary>
public class Display : PresentationForm
{
   
    private WebView _webPanel;
    public Display(Screen screen, bool inFullscreen) : base(screen, inFullscreen)
    {
        Title = "Presentation";
        Maximizable = true;
        
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
        _webPanel.LoadHtml(SlidesManager.PresentationState.GetCurrentAsHTML(this.Bounds));
        Title = "Slide " + SlidesManager.PresentationState.CurrentSlide.SlideNumber + "/" + SlidesManager.PresentationState.Presentation.SlideCount;
        //register
        
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

        Focus();
    }

    private void OnCurrentSlideChanged(Slide slide)
    {
        _webPanel.LoadHtml(SlidesManager.PresentationState.GetCurrentAsHTML(this.Bounds));
        Title = "Slide " + slide.SlideNumber + "/" + SlidesManager.PresentationState.Presentation.SlideCount;
    }

    protected override void ResizePanel()
    {
        base.ResizePanel();
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
        else if (e.Key == Configuration.ToggleFullscreen)
        {
            SetFullscreen(_screen, !_isFullscreen);
        }
    }

    private void OnKeyUp(object? sender, KeyEventArgs e)
    {

    }
    
}