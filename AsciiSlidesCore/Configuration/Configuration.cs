using Eto.Drawing;
using Color = Eto.Drawing.Color;

namespace AsciiSlidesCore;
using Keys = Eto.Forms.Keys;
public static class Configuration
{

	//UX
	public static int ManagerWindowWidth = 300;
	public static int ManagerWindowHeight = 600;
	//Configuration
	public static Eto.Forms.Keys[] ClosePresentation = [Keys.Escape, Keys.Q];
	public static Keys ToggleFullscreen = Keys.F;
	public static Keys[] NextSlide = [Keys.Right, Keys.Space];
	public static Keys[] PreviousSlide = [Keys.Left, Keys.Backspace];

	//Presentation Style Defaults
	public static string BGColor = "#080000";
	public static string FontColor = "#E2DDD9";
	public static double FontSizeVMin = 4;

	public static void LoadDefaultStyle()
	{
		BGColor = "#080000";
		FontColor = "#E2DDD9";
		FontSizeVMin = 4;
	}

	
	public static void InitializeOnLaunch()
	{
		//load settings from file or create settings file.
		LoadDefaultStyle();
	}

	public static void SetKey(string key, string value)
	{
		OSUtility.Instance.SetSettingsKey(key,value);
	}

	public static string? GetKey(string key)
	{
		if (OSUtility.Instance.TryGetSettingsKey(key, out string value))
		{
			return value;
		}
		else
		{
			return null;
		}
	}

	public static void SaveKeys()
	{
		OSUtility.Instance.SaveSettingsKeys();	
	}


}