namespace AsciiSlidesCore;

public class Frontmatter
{
	private Dictionary<string, string> _frontmatter = new();
	private Frontmatter? _parent;
	public Frontmatter()
	{
		
	}
	public Frontmatter((string, string)[] valueTuples)
	{
		foreach (var valueTuple in valueTuples)
		{
			_frontmatter.Add(valueTuple.Item1.ToLower(), valueTuple.Item2);
		}
	}

	public void AddKeyValuePair(string key, string value)
	{
		if (_frontmatter.ContainsKey(key))
		{
			if (CanHaveMultipleSingleline(key))
			{
				_frontmatter[key] = _frontmatter[key] + "\n" + value.TrimEnd();	
			}
			else
			{
				_frontmatter[key] = value;
			}
		}
		else
		{
			_frontmatter.Add(key.ToLower(), value);
		}
	}

	

	public string GetKey(string key, string defaultValue)
	{
		if (TryGetKey(key, out var value))
		{
			return value;
		}
		return defaultValue;
	}
	public bool TryGetKey(string key, out string value)
	{
		if (_frontmatter.TryGetValue(key, out value!))
		{
			return true;
		}
		
		//now check our parent.
		if (_parent != null)
		{
			return _parent.TryGetKey(key, out value!);
		}
		
		value = string.Empty;
		return false;
	}

	//todo: this could be set parent, and then a recursive lookup. This will prevent the pointless data dupes.
	public void SetParentFrontmatter(Frontmatter? parentFrontmatter)
	{
		_parent = parentFrontmatter;
	}

	public static bool CanHaveMultipleSingleline(string key)
	{
		return key == "cue";
	}
}
