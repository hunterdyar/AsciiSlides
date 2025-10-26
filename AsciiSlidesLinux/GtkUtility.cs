using AsciiSlidesCore;
using Eto.Drawing;
using Eto.Forms;

namespace AsciiSlidesMac;

public class GtkUtility : OSUtility
{
    public override bool ToggleFullscreen(Form form, Screen screen, bool fullscreen)
    {
        throw new NotImplementedException();
    }

    public override MonitorInfo[] GetMonitors()
    {
        var allScreens = Screen.Screens.ToArray();
        
        MonitorInfo[] allMonitors = new MonitorInfo[allScreens.Length];
        var etoscreens = Eto.Forms.Screen.Screens.ToArray();
        for (int i = 0; i < allScreens.Length; i++)
        {
            //todo: get monitor name from native thing.
            allMonitors[i] = new MonitorInfo(etoscreens[i], i.ToString());

        }

        return allMonitors;
    }

    public override void SetSettingsKey(string key, string value)
    {
    }

    public override bool TryGetSettingsKey(string key, out string value)
    {
        value = null;
        return false;
    }

    public override void SaveSettingsKeys()
    {
    }

    public override Task CaptureWebViewAsync(WebView webView, Action<Bitmap> onCaptureCompleted)
    {
        throw new NotImplementedException();
    }
}