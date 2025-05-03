using Eto.Forms;

namespace AsciiSlidesCore;

public enum OutputType
{
	None,
	Windowed,
	Fullscreen
}
public struct DisplaySelection
{
	public OutputType DisplayOutputType;
	public Screen? DisplayScreen;
	public OutputType SpeakerNotesOutputType;
	public Screen? SpeakerNotesScreen; 
}