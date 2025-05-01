using System.Text;
using AsciiSlidesCore;
using AsciiSlidesCore.Configuration;
using AsciiSlidesMac;
using Eto;
using Eto.Forms;
using Eto.Drawing;

using MonoMac.AppKit;

//bleh
using Form = Eto.Forms.Form;
using Application = Eto.Forms.Application;

public class AsciiSlidesMacProgram
{
    [STAThread]
    static void Main()
    {
        //Load settings from disc, etc.
        Configuration.InitializeOnLaunch();
        OSUtility.Instance = new MacOSUtility();
        //run window.
        new Application().Run(new SlidesManager());
    }
}