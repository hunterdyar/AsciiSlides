using Eto;
using Eto.Forms;

namespace AsciiSlidesCore.Components;

public class FilesComponent : GroupBox
{
	public static Action<string> OnFilePicked = delegate { };

	public string FilePath => FilePath;
	private string _filePath = string.Empty;
	public string FileName => _fileName;
	private string _fileName = string.Empty;
	private SlidesManager _slidesManager;
	private const string LastFilePathSettingsKey = "LastPresentationFile";
	public FilesComponent(SlidesManager slidesManager)
	{
		_slidesManager = slidesManager;
		var loadFilePicker = new FilePicker();
		var fileLoadedLabel = new Label()
		{
			Text = "No Presentation File Loaded"
		};
		SlidesManager.OnPresentationLoaded += presentation =>
		{
			fileLoadedLabel.Text = "Loaded: " + Path.GetFileName(presentation.FileName);
		};
		SlidesManager.OnPresentationFailedToLoad += message =>
		{
			fileLoadedLabel.Text = message;
		};

		loadFilePicker.Filters.Add(new FileFilter("Text Documents", ".txt", ".text"));
		loadFilePicker.Filters.Add(new FileFilter("Markdown Documents", ".md", ".markdown"));
		loadFilePicker.Filters.Add(new FileFilter("All", "*"));

		loadFilePicker.FileAction = FileAction.OpenFile;
		loadFilePicker.Title = "Open Presentation";

		//todo: when we save the file, and reload it, we don't realize we've reloaded it.
		//hacky temp is to add a 'reload' button.
		//data watch feature feels correct to me, however. Like, we're not loading the presentation, the user should feel like we're just pointing to a file.
		string lastFile = Configuration.GetKey(LastFilePathSettingsKey);
		if (!string.IsNullOrEmpty(lastFile))
		{
			//we check again if the file exists.
			//this should call FilePathChanged.
			loadFilePicker.FilePath = lastFile;
		}
		loadFilePicker.FilePathChanged += (sender, args) => { TryPickFileFromPath(loadFilePicker.FilePath); };

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

		if (!string.IsNullOrEmpty(loadFilePicker.FilePath))
		{
			TryPickFileFromPath(loadFilePicker.FilePath);
		}
	}

	private void TryPickFileFromPath(string filePath)
	{
		if (File.Exists(filePath))
		{
			_filePath = filePath;
			_fileName = Path.GetFileName(_filePath);
			PickFile(filePath);
		}
		else
		{
			Console.WriteLine("File does not exist " + filePath);
			_filePath = string.Empty;
			_fileName = string.Empty;
		}
	}


	void PickFile(string path)
	{
		Configuration.SetKey(LastFilePathSettingsKey, path);	
		OnFilePicked?.Invoke(path);
	}
}