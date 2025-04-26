using System.Runtime.InteropServices;
using Eto.Drawing;
using Eto.Forms;
#if UNIX
using MonoMac.AppKit;
#endif
namespace AsciiSlides;

public class Display : Form
{
    public Display()
    {
        // WindowState = WindowState.Normal;
        // Bounds = Screen.PrimaryScreen.Bounds;
        Topmost = true;
        Bounds = new Rectangle(Screen.PrimaryScreen.Bounds);
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
        GoFullscreen();
        
    }

    private void GoFullscreen()
    {
        #if UNIX
            //todo: Differentiate between mac and linux with different constant?
            var nativeView = this.ToNative();
            nativeView.CollectionBehavior = NSWindowCollectionBehavior.Default |
                                            NSWindowCollectionBehavior.FullScreenPrimary |
                                            NSWindowCollectionBehavior.CanJoinAllSpaces |
                                            NSWindowCollectionBehavior.FullScreenAllowsTiling;
            nativeView.ToggleFullScreen(nativeView);
        #endif
    }
}