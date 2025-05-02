using Eto;
using Eto.Forms;

namespace AsciiSlidesCore.Components;

public class FilesComponent : GroupBox
{
	public FilesComponent()
	{
		var loadFilePicker = new FilePicker();
		var fileLoadedLabel = new Label()
		{
			Text = "Loaded: None"
		};
		EventHandler.OnPresentationLoaded += presentation =>
		{
			fileLoadedLabel.Text = "Loaded: " + Path.GetFileName(presentation.FileName);
		};


		loadFilePicker.Filters.Add(new FileFilter("Text Documents", ".txt", ".text"));
		loadFilePicker.Filters.Add(new FileFilter("Markdown Documents", ".md", ".markdown"));
		loadFilePicker.Filters.Add(new FileFilter("All", "*"));

		loadFilePicker.FileAction = FileAction.OpenFile;
		loadFilePicker.Title = "Open Presentation";
		loadFilePicker.FilePathChanged += (sender, args) =>
		{
			if (File.Exists(loadFilePicker.FilePath))
			{
				PickFile(loadFilePicker.FilePath);
			}
			else
			{
				Console.WriteLine("File does not exist " + loadFilePicker.FilePath);
			}
		};

		Width = Configuration.ManagerWindowWidth;
		Text = "File";
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
		};
	}

	void PickFile(string path)
	{
		EventHandler.OnFilePicked?.Invoke(path);
	}
}