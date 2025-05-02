using Eto;

//bleh
using Eto.Forms;
using Form = Eto.Forms.Form;
using Button = Eto.Forms.Button;
using CheckBox = Eto.Forms.CheckBox;
using Label = Eto.Forms.Label;
using Orientation = Eto.Forms.Orientation;
using Size = Eto.Drawing.Size;

namespace AsciiSlidesCore;

public class SlidesManager : Form
{
    private Display? _display = null;
    public static bool IsPresentationLoaded = false;
    public static Presentation? Presentation = null;
    public static PresentationState PresentationState = new PresentationState();
    
    public SlidesManager()
    {
        Title = "ASCIISlides Manager";
        ClientSize = new Size(Configuration.ManagerWindowWidth, Configuration.ManagerWindowWidth);

        var presentButton = new Button { Text = "Present" };
        var presentCommand = new Command() { MenuText = "Present" };
        var inFullscreen = new CheckBox()
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

                _display = new Display(inFullscreen.Checked.Value);
            }else{
                Console.WriteLine("No Presentation or Empty presentation loaded.");
            }
        };
        
        presentButton.Command = presentCommand;
        presentButton.Enabled = IsPresentationLoaded;

        var loadFilePicker = new FilePicker();
        var fileLoadedLabel = new Label()
        {
            Text = "Loaded: None"
        };
        
        loadFilePicker.Filters.Add(new FileFilter("Text Documents", ".txt", ".text"));
        loadFilePicker.Filters.Add(new FileFilter("Markdown Documents",  ".md", ".markdown"));
        loadFilePicker.Filters.Add(new FileFilter("All", "*"));

        loadFilePicker.FileAction = FileAction.OpenFile;
        loadFilePicker.Title = "Open Presentation";
        loadFilePicker.FilePathChanged += (sender, args) =>
        {
            if (File.Exists(loadFilePicker.FilePath))
            {
                using var fileStream = new StreamReader(loadFilePicker.FilePath);
                try
                {
                    Presentation = Parser.PresentationParser.Parse(fileStream.ReadToEnd());
                    PresentationState = new PresentationState(Presentation);
                    Console.WriteLine("Loaded " + loadFilePicker.FilePath);
                    fileLoadedLabel.Text = "Loaded: " + Path.GetFileName(loadFilePicker.FilePath);
                    IsPresentationLoaded = true;
                    presentButton.Enabled = IsPresentationLoaded;
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    IsPresentationLoaded = false;
                    presentButton.Enabled = IsPresentationLoaded;
                    fileLoadedLabel.Text = "Error Loading:\n " +exception.Message;
                }
            }
            else
            {
                
                Console.WriteLine("File does not exist " + loadFilePicker.FilePath);
            }
        };
        
        //
        var contentLayout = new DynamicLayout();
        var fileGroup = new GroupBox()
        {
            Text = "File",
            Content = new StackLayout()
            {
                Orientation = Orientation.Vertical,
                VerticalContentAlignment = VerticalAlignment.Center,
                Spacing = 10,
                Items =
                {
                    loadFilePicker,
                    fileLoadedLabel,
                }
            },
            Width = Configuration.ManagerWindowWidth
        };
        contentLayout.Add(fileGroup);
        var presentGroup = new GroupBox()
            {
                Text = "Presentation",
                Width = Configuration.ManagerWindowWidth,
                Content = new StackLayout()
                {
                    Orientation = Orientation.Horizontal,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Spacing = 5,
                    Items =
                    {
                        presentButton,
                        inFullscreen,
                    }
                }

            };
        contentLayout.AddRow(presentGroup);
        contentLayout.AddSpace();
        Content = contentLayout;
        
        AsciiSlidesCore.EventHandler.RegisterFormAsSlideController(this);
    }
}
	