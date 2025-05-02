using AsciiSlidesCore;
using Eto.Forms;
using Application = Eto.Forms.Application;

namespace AsciiSlidesWin;

public class AsciiSlidesWindowsProgram {
    
    [STAThread]
    static void Main()
    {
        //Load settings from disc, etc.
        Configuration.InitializeOnLaunch();
        OSUtility.Instance = new WinOSUtility();
        //run window.
        new Application().Run(new SlidesManager());
    }
}