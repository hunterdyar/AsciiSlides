using Eto.Drawing;
using Font = Eto.Drawing.Font;
using FontStyle = Eto.Drawing.FontStyle;

namespace AsciiSlides.Configuration;
using Keys = Eto.Forms.Keys;
public static class Configuration
{
	public static Eto.Forms.Keys ExitKey = Keys.Escape;
	public static Keys ToggleFullscreen = Keys.F;
	public static Keys Right = Keys.Right;
	public static Keys Left = Keys.Left;

	public static Font Font = new Font("Consolas",16,FontStyle.None,FontDecoration.None);
}