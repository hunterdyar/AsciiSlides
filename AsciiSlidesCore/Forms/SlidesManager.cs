using AsciiSlidesCore.Components;
using Eto;
using Eto.Drawing;
using Eto.Forms;

//bleh
namespace AsciiSlidesCore;

public class SlidesManager : Form
{
    private Display? _display = null;
    private PresenterView? _presenterDisplay = null;
    public static bool IsPresentationLoaded = false;
    public static Presentation? Presentation = null;
    public static PresentationState PresentationState = new PresentationState();
    
    public SlidesManager()
    {
        Title = "ASCIISlides Manager";
        ClientSize = new Size(Configuration.ManagerWindowWidth, Configuration.ManagerWindowHeight);

        var presentButton = new Button { Text = "Present" };
        var presentCommand = new Command() { MenuText = "Present" };
        var displayInFullscreen = new CheckBox()
        {
            Text = "Fullscreen",
            Checked = true,
        };
        presentCommand.Executed += (sender, args) =>
        {
            if (IsPresentationLoaded)
            {
                Console.WriteLine("Present");
                if (_display != null)
                {
                    _display.Close();
                    _display.Dispose();
                }

                _display = new Display(displayInFullscreen.Checked.Value);
            }else{
                Console.WriteLine("No Presentation or Empty presentation loaded.");
            }
        };
        
        presentButton.Command = presentCommand;
        presentButton.Enabled = IsPresentationLoaded;
        EventHandler.OnPresentationLoadedChanged += b =>
        {
            presentButton.Enabled = b;
        };

        var presenterViewButton = new Button { Text = "Presenter View" };
        var presenterViewCommand = new Command() { MenuText = "Presenter View" };
        var presenterViewInFullscreen = new CheckBox()
        {
            Text = "Fullscreen",
            Checked = true,
        };
        presenterViewCommand.Executed += (sender, args) =>
        {
            if (IsPresentationLoaded)
            {
                Console.WriteLine("Presenter View");
                if (_presenterDisplay != null)
                {
                    _presenterDisplay.Close();
                    _presenterDisplay.Dispose();
                }

                _presenterDisplay = new PresenterView();
            }
            else
            {
                Console.WriteLine("No Presentation or Empty presentation loaded.");
            }
        };

        presenterViewButton.Command = presenterViewCommand;
        presenterViewButton.Enabled = IsPresentationLoaded;
        EventHandler.OnPresentationLoadedChanged += b => { presenterViewButton.Enabled = b; };
        //
        var contentLayout = new DynamicLayout();
        var fileGroup = new FilesComponent();
        contentLayout.Add(fileGroup);
        var presentGroup = new GroupBox()
            {
                Text = "Presentation",
                Width = Configuration.ManagerWindowWidth,
                Content = new StackLayout()
                {
                    Orientation = Orientation.Vertical, 
                    Items = {
                                new StackLayout()
                                {
                                    Orientation = Orientation.Horizontal,
                                    VerticalContentAlignment = VerticalAlignment.Center,
                                    Spacing = 5,
                                    Items =
                                    {
                                        presentButton,
                                        displayInFullscreen,
                                    }
                                },
                                new StackLayout()
                                {
                                    Orientation = Orientation.Horizontal,
                                    VerticalContentAlignment = VerticalAlignment.Center,
                                    Spacing = 5,
                                    Items =
                                    {
                                        presenterViewButton,
                                        presenterViewInFullscreen,
                                    }
                                }
                            }
                }

            };
        contentLayout.AddRow(presentGroup);
        contentLayout.AddSpace();
        Content = contentLayout;

        EventHandler.OnFilePicked += OnFilePicked;
        EventHandler.RegisterFormAsSlideController(this);
        Closed += (sender, args) =>
        {
            EventHandler.OnFilePicked -= OnFilePicked;
        };
    }

    private void OnFilePicked(string path)
    {
        using var fileStream = new StreamReader(path);
        LoadPresentation(Path.GetFileName(path),fileStream.ReadToEnd());
    }

    private void LoadPresentation(string fileName, string presentationText)
    {
        try
        {
            Presentation = Parser.PresentationParser.Parse(presentationText);
            Presentation.FileName = fileName;
            PresentationState = new PresentationState(Presentation);
            SetPresentationLoaded(true);
            EventHandler.OnPresentationLoaded.Invoke(Presentation);
          
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            IsPresentationLoaded = false;
        }
    }

    private void SetPresentationLoaded(bool b)
    {
        IsPresentationLoaded = b;
        EventHandler.OnPresentationLoadedChanged.Invoke(IsPresentationLoaded);
    }
}
	