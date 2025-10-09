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
	private readonly FilePicker _filePicker;
	public FilesComponent(SlidesManager slidesManager)
	{
		_slidesManager = slidesManager;
		_filePicker = new FilePicker();
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

		_filePicker.Filters.Add(new FileFilter("Text Documents", ".txt", ".text"));
		_filePicker.Filters.Add(new FileFilter("Markdown Documents", ".md", ".markdown"));
		_filePicker.Filters.Add(new FileFilter("All", "*"));

		_filePicker.FileAction = FileAction.OpenFile;
		_filePicker.Title = "Open Presentation";

		//todo: when we save the file, and reload it, we don't realize we've reloaded it.
		//hacky temp is to add a 'reload' button.
		//data watch feature feels correct to me, however. Like, we're not loading the presentation, the user should feel like we're just pointing to a file.
		string? lastFile = Configuration.GetKey(LastFilePathSettingsKey);
		if (!string.IsNullOrEmpty(lastFile))
		{
			//we check again if the file exists.
			//this should call FilePathChanged.
			_filePicker.FilePath = lastFile;
		}
		_filePicker.FilePathChanged += (sender, args) => { TryPickFileFromPath(_filePicker.FilePath); };
		// ReSharper disable once VirtualMemberCallInConstructor
		Width = Configuration.ManagerWindowWidth;
		Text = "File";
		Content = new StackLayout()
		{
			Orientation = Orientation.Vertical,
			VerticalContentAlignment = VerticalAlignment.Center,
			Spacing = 10,
			Items =
			{
				_filePicker,
				fileLoadedLabel,
			}
		};

		if (!string.IsNullOrEmpty(_filePicker.FilePath))
		{
			TryPickFileFromPath(_filePicker.FilePath);
		}
	}

	private void TryPickFileFromPath(string filePath)
	{
		if (string.IsNullOrEmpty(filePath))
		{
			return;
		}
		
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