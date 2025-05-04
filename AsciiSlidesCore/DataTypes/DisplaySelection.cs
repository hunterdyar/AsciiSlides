using Eto.Forms;

namespace AsciiSlidesCore;

public enum OutputType
{
	None,
	Windowed,
	Fullscreen
}
public class DisplaySelection
{
	public OutputType DisplayOutputType;
	public MonitorInfo? DisplayScreen;
	public OutputType SpeakerNotesOutputType;
	public MonitorInfo? SpeakerNotesScreen;

	public void Swap()
	{
		(SpeakerNotesScreen,DisplayScreen) = (DisplayScreen, SpeakerNotesScreen);
		(SpeakerNotesOutputType,DisplayOutputType) = (DisplayOutputType, SpeakerNotesOutputType);
	}

	public override string ToString()
	{
		string ds = DisplayScreen?.ToString() ?? "None";
		string ss = SpeakerNotesScreen?.ToString() ?? "None";
		return $"Display Selection: Display: {DisplayOutputType.ToString()} ({ds}). Speaker: {SpeakerNotesOutputType.ToString()} ({ss})";
	}
}