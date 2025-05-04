using System.Configuration;
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
	public static Eto.Forms.Keys[] ExitKey = [Keys.Escape, Keys.Q];
	public static Keys ToggleFullscreen = Keys.F;
	public static Keys[] NextSlide = [Keys.Right, Keys.Space];
	public static Keys[] PreviousSlide = [Keys.Left, Keys.Backspace];

	//Presentation Style Defaults
	public static Color BGColor;
	public static Color FontColor;

	public static void LoadDefaultStyle()
	{
		BGColor = new Color(0.97f, 0.97f, 0.97f);
		FontColor = Colors.Black;
	}

	public static void InitializeOnLaunch()
	{
		//load settings from file or create settings file.
		LoadDefaultStyle();
		var c = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
		
	}

	public static void SetKey(string key, string value)
	{
		var c = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
		if (c != null)
		{
			c.AppSettings.Settings.Add(key,value);
		}
	}

	public static string GetKey(string key)
	{
		var c = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
		if (c != null)
		{
			var kvp= c.AppSettings.Settings[key];
			if (kvp != null)
			{
				return kvp.Value;
			}
		}
		
		return null;
		
	}

	public static void SaveKeys()
	{
		var c = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
		if (c != null)
		{
			c.Save();
		}
	}
	
}