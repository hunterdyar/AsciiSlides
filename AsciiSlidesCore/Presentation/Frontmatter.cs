namespace AsciiSlidesCore;

public class Frontmatter
{
	private Dictionary<string, string> _frontmatter = new();

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
		_frontmatter.Add(key.ToLower(), value);
	}
	
	public bool TryGetKey(string key, out string value)
	{
		if (_frontmatter.TryGetValue(key, out value))
		{
			return true;
		}
		else
		{
			value = string.Empty;
			return false;
		}
	}
}
