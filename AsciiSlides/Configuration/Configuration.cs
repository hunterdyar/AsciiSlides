using Eto.Drawing;
using Color = Eto.Drawing.Color;
using Font = Eto.Drawing.Font;
using FontStyle = Eto.Drawing.FontStyle;

namespace AsciiSlides.Configuration;
using Keys = Eto.Forms.Keys;
public static class Configuration
{
	//Configuration
	public static Eto.Forms.Keys[] ExitKey = [Keys.Escape, Keys.Q];
	public static Keys ToggleFullscreen = Keys.F;
	public static Keys Right = Keys.Right;
	public static Keys Left = Keys.Left;

	//Presentation Style
	public static Color BGColor;
	public static Color ASCIIAreaBGColor => BGColor;
	public static Color FontColor;

	public static void LoadDefaultStyle()
	{
		BGColor = new Color(0.97f, 0.97f, 0.97f);
		FontColor = Colors.Black;
	}
}