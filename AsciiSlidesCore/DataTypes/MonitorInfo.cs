using Eto.Forms;

namespace AsciiSlidesCore;

public class MonitorInfo : IListItem
{
	public string DisplayName;
	public Eto.Forms.Screen? Screen;
	public bool isFullscreen = true;
	public bool isNone = false;

	public MonitorInfo(Screen? screen, string displayName)
	{
		DisplayName = displayName;
		Screen = screen;
		Text = ToString();
		Key = ToString();
	}

	public static MonitorInfo Windowed = new MonitorInfo(null,"") { isFullscreen = false,Text="Windowed" };
	public static MonitorInfo None = new MonitorInfo(null,"") { isNone = true,Text="None" };
	
	//this is used for which screen to launch windowed on.
	public static MonitorInfo PrimaryScreen = new MonitorInfo(Screen.PrimaryScreen, "Primary Screen")
	{
		isFullscreen = true,
		Text = "Primary",
		isNone = false,
	};

	public override string ToString()
	{
		if (isNone)
		{
			return "None";
		}

		if (isFullscreen)
		{
			return DisplayName;
		}
		else
		{
			return "Windowed";
		}
	}

	public string Text { get; set; }
	public string Key { get; }
}