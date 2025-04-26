using Eto.Forms;
using Eto.Drawing;
using MonoMac.AppKit;

namespace AsciiSlides;
public class SlidesManager : Form
{
    private Display? _display = null;
    public SlidesManager()
    {
        Title = "ASCIISlides Manager";
        ClientSize = new Size(300, 600);
        
        var presentButton = new Button { Text = "Present" };
        var presentCommand = new Command() { MenuText = "Present" };
        presentCommand.Executed += (sender, args) =>
        {
            Console.WriteLine("Present");
            if (_display != null)
            {
                _display.Close();
                _display.Dispose();
            } 
            _display = new Display();
            
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
                presentButton,
                new Label()
                {
                    Width = 200,
                    Height = 600,
                    Text = "This is text \n newline \n test"
                },
                
            }
        };
        
        var quitCommand = new Command { MenuText = "Quitsy", Shortcut = Application.Instance.CommonModifier | Keys.Q };
        quitCommand.Executed += (sender, e) => Application.Instance.Quit();
        Menu = new MenuBar
        {
            Items =
            {
                // File submenu
                new SubMenuItem { Text = "&Present", Items = { presentCommand } },
                // new SubMenuItem { Text = "&Edit", Items = { /* commands/items */ } },
                // new SubMenuItem { Text = "&View", Items = { /* commands/items */ } },
            },
            ApplicationItems =
            {
                // application (OS X) or file menu (others)
                new ButtonMenuItem { Text = "&Preferences..." },
            },
            QuitItem = quitCommand,
        };
    }
	
    [STAThread]
    static void Main()
    {
        new Application().Run(new SlidesManager());
    }
}