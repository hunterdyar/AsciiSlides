using Eto;
using Eto.Forms;

namespace AsciiSlidesCore.Components;

public class FilesComponent : GroupBox
{
	public string FilePath => FilePath;
	private string _filePath = string.Empty;
	public string FileName => _fileName;
	private string _fileName = string.Empty;
	private SlidesManager _slidesManager;
	public FilesComponent(SlidesManager slidesManager)
	{
		_slidesManager = slidesManager;
		var loadFilePicker = new FilePicker();
		var fileLoadedLabel = new Label()
		{
			Text = "Loaded: None"
		};
		PresentationState.OnPresentationLoaded += presentation =>
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
				_filePath = loadFilePicker.FilePath;
				_fileName = Path.GetFileName(_filePath);
				PickFile(loadFilePicker.FilePath);
			}
			else
			{
				Console.WriteLine("File does not exist " + loadFilePicker.FilePath);
				_filePath = string.Empty;
				_fileName = string.Empty;
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