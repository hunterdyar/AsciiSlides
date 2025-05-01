using AsciiSlidesCore;
using AsciiSlidesCore.Configuration;
using Eto.Forms;

public class AsciiSlidesWindowsProgram {
    [STAThread]
    static void Main()
    {
        //Load settings from disc, etc.
        Configuration.InitializeOnLaunch();
        //run window.
        new Eto.Forms.Application().Run(new SlidesManager());
    }
}