namespace AsciiSlidesCore;

/// <summary>
/// Determines if the slide should make nose, autoplay video, etc.
/// the preview window is not just the texture of the main window (sadly), it's its own html canvas. So we have to tell it when to and not to link elements.
/// </summary>
public enum SlideViewMode
{
	CurrentPresenting,
	Preview,
	CurrentSpeaker
}