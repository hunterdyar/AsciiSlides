using System.Runtime.InteropServices;
using Eto.Drawing;
using Eto.Forms;
using Form = Eto.Forms.Form;
using Label = Eto.Forms.Label;
using Rectangle = Eto.Drawing.Rectangle;

#if UNIX
using MonoMac.AppKit;
#endif
namespace AsciiSlides;

public class Display : Form
{
    public bool IsFullscreen => _isFullscreen;
    private bool _isFullscreen;
    public Display()
    {
        // WindowState = WindowState.Normal;
        // Bounds = Screen.PrimaryScreen.Bounds;
        Topmost = true;
        //Bounds = new Rectangle(Screen.Bounds);
        Maximizable = true;
        Show();
        Focus();
        //Maximize();
       
        
        Content = new Label()
        {
            Text = "hello",
        };
        
        Console.WriteLine("Created Display. Now go fullscreen");
        //fullscreen
        SetFullscreen();
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