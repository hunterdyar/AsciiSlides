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
	public Screen? DisplayScreen;
	public OutputType SpeakerNotesOutputType;
	public Screen? SpeakerNotesScreen;

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