namespace AsciiSlidesCore;

public abstract class OSUtility
{
	public static OSUtility Instance { get; set; } = null!;
	public abstract bool ToggleFullscreen(Eto.Forms.Form form, bool fullscreen);

	public abstract MonitorInfo[] GetMonitors();
}