namespace AsciiSlides;

public class Frontmatter
{
	private Dictionary<string, string> _frontmatter = new Dictionary<string, string>();

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
}
