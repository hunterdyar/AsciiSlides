using Eto.Drawing;
using Eto.Forms;

using Form = Eto.Forms.Form;
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
    public Action OnRenderComplete;
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
        Title = "Slide " + SlidesManager.PresentationState.CurrentSlide.SlideNumber + "/" + SlidesManager.PresentationState.Presentation.SlideCount;

        //this is broken, the width and height of 'this' is still 0,0.
        SlidesManager.PresentationState.CurrentSlide.RenderTo(SlidesManager.PresentationState, _webPanel);
        
        Focus();

        _webPanel.DocumentLoaded += (sender, args) => 
        {
            OnRenderComplete?.Invoke();
        };
    }
    
    
    protected override void OnSlideFunction(string slideFunctionName)
    {
        Console.WriteLine($"Slide Function: {slideFunctionName}");
        switch (slideFunctionName)
        {
            case "playvideo":
                _webPanel.ExecuteScript("""
                                        console.log("play/pause video");
                                        playPauseVideo();
                                        """);
                break;
            case "mute":
                _webPanel.ExecuteScript("""
                                        console.log("mute video");
                                        muteToggleVideo();
                                        """);
                break;
        }
    }

    protected override void OnCurrentSlideChanged(Slide slide)
    {
        if (slide is YTSlide)
        {
            _webPanel.BrowserContextMenuEnabled =  true;
        }
        else
        {
            _webPanel.BrowserContextMenuEnabled = false;
        }
        
        SlidesManager.PresentationState.CurrentSlide.RenderTo(SlidesManager.PresentationState, _webPanel);
        Title = "Slide " + slide.SlideNumber + "/" + SlidesManager.PresentationState.Presentation.SlideCount;
        OnRenderComplete?.Invoke();
    }

    protected override void ResizePanel()
    {
        base.ResizePanel();
        //todo: right now we just reload everything, but once we support video embeddes, will have to do this by running some JS or such to dynamically change the style properties and not do a reload.
        //can use _webPanel.ExecuteScript()
        SlidesManager.PresentationState.CurrentSlide.RenderTo(SlidesManager.PresentationState, _webPanel);

    }

    protected override void OnKeyDown(object? sender, KeyEventArgs e)
    {
        base.OnKeyDown(sender, e);
        //todo: I think WebView2 is capturing keyboard input
    }

    public async Task Capture(Action<Bitmap> callback)
    {
        await OSUtility.Instance.CaptureWebViewAsync(_manager.Display.View, (capturedBitmap) =>
        {
            // Handle the captured bitmap here
           callback?.Invoke(capturedBitmap);
            // Add it to your form, save it, etc.
        });
    }
}