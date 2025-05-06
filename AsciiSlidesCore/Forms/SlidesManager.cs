using System.Windows.Input;
using AsciiSlidesCore.Components;
using Eto;
using Eto.Drawing;
using Eto.Forms;

//bleh
namespace AsciiSlidesCore;

public class SlidesManager : Form
{
    public Display Display => _display;
    private Display? _display = null;
    public SpeakerView SpeakerView => _speakerView;
    private SpeakerView? _speakerView = null;
    private OutputComponent _outputComponent;
    private FilesComponent _filesComponent;
    private SlideControlComponent _slideControlComponent;
    public static Presentation? Presentation = null;
    public static PresentationState PresentationState = new PresentationState();
    public static Action<Presentation> OnPresentationLoaded = delegate { };
    public static Action<string> OnPresentationFailedToLoad = delegate { };
    
    //commands
    public Command CloseCommand { get; private set; }
    
    public SlidesManager()
    {
        Title = "ASCIISlides Manager";
        ClientSize = new Size(Configuration.ManagerWindowWidth, Configuration.ManagerWindowHeight);
            
        var presentButton = new Button { Text = "Present" };
        var displayInFullscreen = new CheckBox()
        {
            Text = "Fullscreen",
            Checked = true,
        };

        CreateCommands();
        
        //Layout
        var contentLayout = new DynamicLayout();
        Content = contentLayout;

        //files
        FilesComponent.OnFilePicked += OnFilePicked;
        _filesComponent = new FilesComponent(this);
        contentLayout.Add(_filesComponent);
        
        //output
        _outputComponent = new OutputComponent(this);
        contentLayout.AddRow(_outputComponent);
        contentLayout.AddSpace();
        
        //slide control
        _slideControlComponent = new SlideControlComponent(this);
        contentLayout.AddRow(_slideControlComponent);
        contentLayout.AddSpace();

        //event registration
        InputHandler.RegisterFormAsSlideController(this);
        PresentationState.OnPresentationClosed += ClosePresentation;
        Closed += (sender, args) =>
        {
            //unregister from events
            PresentationState.OnPresentationClosed -= ClosePresentation;
            FilesComponent.OnFilePicked -= OnFilePicked;

            //cleanup and save
            OnClose();
        };
        
        //Now shrink the window to meet the size. The dynamic layout used the original given size, so currently it will set width to from config, and height by the layout.
        ClientSize = new Size(contentLayout.Size.Width, contentLayout.Size.Height);
    }

    private void CreateCommands()
    {
        CloseCommand = new Command()
        {
            MenuText = "Close Presentation",
            ToolTip = "Close Presentation",
            Shortcut = Application.Instance.CommonModifier | Keys.Escape,

        };
        CloseCommand.Executed += (sender, args) => { SlidesManager.PresentationState.ClosePresentation(); };
    }


    private void OnFilePicked(string path)
    {
        using var fileStream = new StreamReader(path);
        LoadPresentation(path,fileStream.ReadToEnd());
    }

    private void LoadPresentation(string path, string presentationText)
    {
        try
        {
            Presentation = Parser.PresentationParser.Parse(presentationText);
            Presentation.Path = Path.GetDirectoryName(path)+"\\";
            Presentation.FileName  = Path.GetFileNameWithoutExtension(path);
            PresentationState = new PresentationState(Presentation);
            //PresentationState.SetPresentationReady(true);//this gets called by listener. uhg.
            OnPresentationLoaded?.Invoke(Presentation);
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            PresentationState.SetPresentationReady(false);
            OnPresentationFailedToLoad?.Invoke(exception.Message);
        }
    }

    public void LaunchPresentation(DisplaySelection displaySelection)
    {
        Console.WriteLine($"Launching Presentation with {displaySelection}");
        if (PresentationState.IsPresentationReady)
        {
            Console.WriteLine("Present");

            if (_speakerView != null)
            {
                _speakerView.Close();
                if (!_speakerView.IsDisposed)
                {
                    _speakerView.Dispose();
                }
            }

            if (displaySelection.SpeakerNotesOutputType != OutputType.None)
            {
                if (displaySelection.SpeakerNotesOutputType == OutputType.Fullscreen)
                {
                    if (displaySelection.SpeakerNotesScreen?.Screen != null)
                    {
                        _speakerView = new SpeakerView(this, displaySelection.SpeakerNotesScreen?.Screen, true);
                    }
                    else
                    {
                        throw new Exception("Speaker Screen Not Set, but output type is fullscreen.");
                    }
                }
                else if (displaySelection.SpeakerNotesOutputType == OutputType.Windowed)
                {
                    _speakerView = new SpeakerView(this, displaySelection.SpeakerNotesScreen?.Screen, false);
                }
            }
            
            
            //is there a situation where we don't close previous?
            if (_display != null)
            {
                _display.Close();
                if (!_display.IsDisposed)
                {
                    _display.Dispose();
                }
            }
        
            
            if (displaySelection.DisplayOutputType != OutputType.None)
            {
                //close previous
                if (displaySelection.DisplayOutputType == OutputType.Fullscreen)
                {
                    if (displaySelection.DisplayScreen?.Screen != null)
                    {
                        _display = new Display(this, displaySelection.DisplayScreen?.Screen, true);
                    }
                    else
                    {
                        throw new Exception("Display Screen Not Set, but output type is fullscreen.");
                    }
                }else if (displaySelection.DisplayOutputType == OutputType.Windowed)
                {
                    _display = new Display(this, displaySelection.DisplayScreen?.Screen,false);
                }
            }

            
        }
        else
        {
            Console.WriteLine("No Presentation or Empty presentation loaded.");
        }

        PresentationState.SetIsPresenting(true);
    }

    public void ClosePresentation()
    {
        if (_display != null)
        {
            _display.Close();
        }

        if (_speakerView != null)
        {
            _speakerView.Close();
        }
    }
    private void OnClose()
    {
        _outputComponent.OnClose();
        Configuration.SaveKeys();
    }
}
	