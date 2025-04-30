using System.Text;
using Eto;
using Eto.Forms;
using Eto.Drawing;
using Form = Eto.Forms.Form;
using MonoMac.AppKit;
using Application = Eto.Forms.Application;
using Button = Eto.Forms.Button;
using CheckBox = Eto.Forms.CheckBox;
using Keys = Eto.Forms.Keys;
using Label = Eto.Forms.Label;
using Orientation = Eto.Forms.Orientation;
using Size = Eto.Drawing.Size;

namespace AsciiSlides;
public class SlidesManager : Form
{
    private Display? _display = null;
    public static Presentation Presentation = new Presentation();
    public static PresentationState PresentationState = new PresentationState();
    public SlidesManager()
    {
        Title = "ASCIISlides Manager";
        ClientSize = new Size(300, 600);
        
        var presentButton = new Button { Text = "Present" };
        var presentCommand = new Command() { MenuText = "Present" };
        var inFullscreen = new CheckBox()
        {
            Text = "Fullscreen",
            Checked = true,
        };
        presentCommand.Executed += (sender, args) =>
        {
            Console.WriteLine("Present");
            if (_display != null)
            {
                _display.Close();
                _display.Dispose();
            } 
            _display = new Display(inFullscreen.Checked.Value);
            
        };
        presentButton.Command = presentCommand;
        
        var loadFilePicker = new FilePicker();
        loadFilePicker.Filters.Add(new FileFilter("Text Documents",".txt",".text",",md"));
        loadFilePicker.FileAction = FileAction.OpenFile;
        loadFilePicker.Title = "Open Presentation";
        loadFilePicker.FilePathChanged += (sender, args) =>
        {
            using var fileStream = new StreamReader(loadFilePicker.FilePath);
            Presentation = Parser.PresentationParser.Parse(fileStream.ReadToEnd());
            Console.WriteLine("Loaded "+loadFilePicker.FilePath);
        };
        Content = new StackLayout()
        {
            Items =
            {
                loadFilePicker,
                new StackLayout(){
                    Orientation = Orientation.Horizontal,
                    VerticalContentAlignment = VerticalAlignment.Center,
                    Spacing = 5,
                    Items = {presentButton,
                    inFullscreen,
                    }
                }

                
            }
        };
    }
	
    [STAThread]
    static void Main()
    {
        //Load settings from disc, etc.
        Configuration.Configuration.InitializeOnLaunch();
        //run window.
        new Application().Run(new SlidesManager());
    }
}