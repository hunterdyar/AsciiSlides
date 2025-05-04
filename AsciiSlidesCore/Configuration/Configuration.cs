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

	private static System.Configuration.Configuration _configuration;
	public static void LoadDefaultStyle()
	{
		BGColor = new Color(0.97f, 0.97f, 0.97f);
		FontColor = Colors.Black;
	}

	private static void LazyGetConfig()
	{
		if (_configuration == null)
		{
			_configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			
		}
	}
	public static void InitializeOnLaunch()
	{
		
		//load settings from file or create settings file.
		LoadDefaultStyle();
		
	}

	public static void SetKey(string key, string value)
	{
		LazyGetConfig();
		if (_configuration != null)
		{
			_configuration.AppSettings.Settings[key].Value = value;
		}
	}

	public static string GetKey(string key)
	{
		LazyGetConfig();
		if (_configuration != null)
		{
			var kvp= _configuration.AppSettings.Settings[key];
			if (kvp != null)
			{
				return kvp.Value;
			}
		}
		
		return null;
		
	}

	public static void SaveKeys()
	{
		LazyGetConfig();
		if (_configuration != null)
		{
			_configuration.Save(ConfigurationSaveMode.Minimal);
		}
	}
}