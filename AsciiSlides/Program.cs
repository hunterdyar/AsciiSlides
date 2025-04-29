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
        
        Content = new StackLayout()
        {
            Items =
            {
                new Button()
                {
                    Text = "Open File",
                },
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
        new Application().Run(new SlidesManager());
    }
}