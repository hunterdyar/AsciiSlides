using AsciiSlidesCore;
using AsciiSlidesCore.Configuration;
using Eto.Forms;
using Application = Eto.Forms.Application;

public class AsciiSlidesWindowsProgram {
    [STAThread]
    static void Main()
    {
        //Load settings from disc, etc.
        Configuration.InitializeOnLaunch();
        //run window.
        new Application().Run(new SlidesManager());
    }
}