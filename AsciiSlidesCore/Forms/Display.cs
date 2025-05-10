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
    public WebView View => _webPanel;
    private WebView _webPanel;
    public Display(SlidesManager manager, Screen screen, bool inFullscreen) : base(manager, screen, inFullscreen)
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
        _webPanel.BrowserContextMenuEnabled = true;
        SlidesManager.PresentationState.CurrentSlide.RenderTo(SlidesManager.PresentationState, _webPanel);
        Title = "Slide " + SlidesManager.PresentationState.CurrentSlide.SlideNumber + "/" + SlidesManager.PresentationState.Presentation.SlideCount;
        //register
        
        Focus();
    }

    protected override void OnCurrentSlideChanged(Slide slide)
    {
        SlidesManager.PresentationState.CurrentSlide.RenderTo(SlidesManager.PresentationState, _webPanel);
        Title = "Slide " + slide.SlideNumber + "/" + SlidesManager.PresentationState.Presentation.SlideCount;
        
    }

    protected override void ResizePanel()
    {
        base.ResizePanel();
        //todo: right now we just reload everything, but once we support video embeddes, will have to do this by running some JS or such to dynamically change the style properties and not do a reload.
        //can use _webPanel.ExecuteScript()
        SlidesManager.PresentationState.CurrentSlide.RenderTo(SlidesManager.PresentationState, _webPanel);

    }
    
}