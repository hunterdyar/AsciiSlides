using System.Runtime.InteropServices;
using Eto.Drawing;
using Eto.Forms;
using Form = Eto.Forms.Form;
using KeyEventArgs = Eto.Forms.KeyEventArgs;
using Keys = Eto.Forms.Keys;
using Label = Eto.Forms.Label;
using Panel = Eto.Forms.Panel;
using Rectangle = Eto.Drawing.Rectangle;

#if UNIX
using MonoMac.AppKit;
#endif
namespace AsciiSlides;

/// <summary>
/// Display is a window that renders the current Presentation with a given PresentationState.
/// </summary>
public class Display : Form
{
    public bool IsFullscreen => _isFullscreen;
    private bool _isFullscreen;

    private WebView _webPanel;
    private Label _slideText = new Label()
    {
        Wrap = WrapMode.None,
        Font = Configuration.Configuration.Font,
        TextAlignment = TextAlignment.Center,
        AllowDrop = false,
        BackgroundColor = Colors.Aquamarine,
    };
    //ascii character row/column counts set by the presentation, with per-slide overrides.
    
    public Display(bool inFullscreen)
    {
        // WindowState = WindowState.Normal;
        // Bounds = Screen.PrimaryScreen.Bounds;
        // Topmost = true;
        Topmost = inFullscreen;
        //todo: get the aspect ratio of the presentation, use a centered rectangle that large
        Bounds = new Rectangle(Screen.Bounds*0.8f);
        Maximizable = true;
        Show();
        Focus();
        //Maximize();

        RegisterShortcuts();

        _webPanel = new Eto.Forms.WebView()
        {
            BackgroundColor = Colors.LightYellow,
            Width = 40,
            Height = 20,
            BrowserContextMenuEnabled = false,
        };
        _webPanel.LoadHtml(SlidesManager.PresentationState.GetCurrentAsHTML(this.Bounds));
        Console.WriteLine("Loaded. Reloading anyway.");
        _webPanel.Reload();
        this.Content = _webPanel;
        ResizePanel();
        
        
        Console.WriteLine("Created Display. Now go fullscreen");
        //fullscreen
        SetFullscreen(inFullscreen);
    }

    private void RegisterShortcuts()
    {
        this.KeyDown += OnKeyDown;
        this.KeyUp += OnKeyUp;
    }



    private void ResizePanel()
    {
        var b = Bounds;
        int h = SlidesManager.PresentationState.RowCount;
        int w = SlidesManager.PresentationState.ColumnCount;
        var aspect = w / (float)h;
        var screenAspect = b.Width / (float)b.Height;

        Console.WriteLine($"Resizing Panel. content: {w}x{h} = {aspect}, screen: {b.Width}x{b.Height} = {screenAspect}");

        if (aspect >= screenAspect)
        {
            //if aspect is equal, we can do either branch and it doesn't matter.
            
            //we are wider, and will letterbox top and bottom.
            //Set the width to full bounds width, adjust height by aspect.
            _slideText.Width = (int)(b.Width);
            _slideText.Height = (int)(b.Width / aspect);
        }else if (aspect < screenAspect)
        {
            //we are narrow, screen is wide. will letterbox sides.
            //set the height to full bounds height, adjust the width by aspet.
            //_panel.Width = (int)(b.Height * aspect);
         //   _panel.Height = b.Height;
            
            _slideText.Width = (int)(b.Height * aspect);
            _slideText.Height = (int)(b.Height);
           
            Console.WriteLine(
                $"Set Panel. content: {_slideText.Width}x{_slideText.Height}|{b.Height} ({_slideText.Width/(float)_slideText.Height})");

        }
        // _slideText.Font.LineHeight
    }

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Configuration.Configuration.ExitKey)
        {
            Console.WriteLine("Exiting...");
            Close();
        }else if (e.Key == Configuration.Configuration.ToggleFullscreen)
        {
            SetFullscreen(!_isFullscreen);
        }else if (e.Key == Keys.Right)
        {
            if (e.Control)
            {
                //Cycle up list of screens.
                MoveScreens(1);
            }
            else
            {
                SlidesManager.PresentationState?.NavigateRelative(1);
                //go right!
            }
        }else if (e.Key == Keys.Left)
        {
            if (e.Control)
            {
                //Cycle Down list of screens.
                MoveScreens(-1);
            }
            else
            {
                //Go back a slide.
                SlidesManager.PresentationState?.NavigateRelative(-1);
            }
        }
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
        #if UNIX
            //todo: Differentiate between mac and linux with different constant?
            var nativeView = this.ToNative();
            nativeView.CollectionBehavior = NSWindowCollectionBehavior.Default |
                                            NSWindowCollectionBehavior.FullScreenPrimary |
                                            NSWindowCollectionBehavior.CanJoinAllSpaces |
                                            NSWindowCollectionBehavior.FullScreenAllowsTiling;
            nativeView.ToggleFullScreen(nativeView);
            _isFullscreen = fullscreen;
        #elif WINDOWS
        var view = WinFormsHelpers.ToNative(this);
        if (view != null)
        {
            if (fullscreen)
            {
                view.FormBorderStyle = FormBorderStyle.None;
                view.TopMost = true;
                view.WindowState = FormWindowState.Maximized;
                _isFullscreen = true;
            }
            else
            {
                view.FormBorderStyle = FormBorderStyle.Sizable;
                view.WindowState = FormWindowState.Normal;
                _isFullscreen = false;
            }
        }

        #endif
    }
}