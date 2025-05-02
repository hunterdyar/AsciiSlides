using AsciiSlidesCore;
using Application = Eto.Forms.Application;

namespace AsciiSlidesMac;
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