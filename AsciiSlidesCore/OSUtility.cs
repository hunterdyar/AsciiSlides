using Eto.Forms;

namespace AsciiSlidesCore;

public abstract class OSUtility
{
	public static OSUtility Instance { get; set; } = null!;
	public abstract bool ToggleFullscreen(Form form, Screen screen, bool fullscreen);

	public abstract MonitorInfo[] GetMonitors();
}