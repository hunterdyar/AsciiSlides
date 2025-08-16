namespace AsciiSlidesCore;

public class Cue
{
	private string raw;
	public readonly int Hours = 0;
	public readonly int Minutes = 0;
	public readonly int Seconds = 0;

	public Cue(int hours, int minutes, int seconds)
	{
		//timespan is just a lazy way to convert >60 seconds into minutes/seconds, etc.
		//but we could probably use timespan as the actual storage type?
		TimeSpan t = new TimeSpan(hours, minutes, seconds);
		this.Hours = t.Hours;
		this.Minutes = t.Minutes;
		this.Seconds = t.Seconds;
	}

	public Cue(int minutes, int seconds)
	{
		TimeSpan t = new TimeSpan(0,minutes, seconds);
		this.Hours = t.Hours;
		this.Minutes = t.Minutes;
		this.Seconds = t.Seconds;
	}
	
	/// <summary>
	/// Trues to parse 0:0, then 0m0s, then using timespan.
	/// timespan will parse "0:10" as 0 hours, 10 minutes; so we do our own first that matches video timestamps, assuming minutes not hours on the left.
	/// it also fails on "24:00:00 or "0:0:60" as invalid, and we'd rather overflow.
	/// </summary>
	/// <param name="source"></param>
	/// <param name="cue"></param>
	/// <returns></returns>
	public static bool TryCreateCue(string source, out Cue cue)
	{
		var colonSplit = source.Split(':');
		if (colonSplit.Length == 3)
		{
			//todo: make tryParse
			int hours = int.Parse(colonSplit[0]);
			int minutes = int.Parse(colonSplit[1]);
			int seconds = int.Parse(colonSplit[2]);
			cue = new Cue(hours, minutes, seconds);
			return true;
		}else if (colonSplit.Length == 2)
		{
			//todo: make tryParse
			int minutes = int.Parse(colonSplit[0]);
			int seconds = int.Parse(colonSplit[1]);
			cue = new Cue(minutes, seconds);
			return true;
		}else if (colonSplit.Length == 4)
		{
			//milliseconds not currently supported
			int hours = int.Parse(colonSplit[0]);
			int minutes = int.Parse(colonSplit[1]);
			int seconds = int.Parse(colonSplit[2]);
			int milliseconds = int.Parse(colonSplit[3]);
			var t = new TimeSpan(hours, minutes, seconds, milliseconds);
			cue = new Cue(t.Hours, t.Minutes, t.Seconds);
			return true;
		}
		else
		{
			TimeSpan t = new TimeSpan(0);
			var mi = source.IndexOf('m');
			var si = source.IndexOf('s');
			var hi = source.IndexOf('h');
			if (mi > 0 || si > 0 || hi > 0)
			{
				var m = GetPrecedingDigits(mi, source);
				var s = GetPrecedingDigits(si, source);
				var h = GetPrecedingDigits(hi, source);

				t = new TimeSpan(0, h, m, s);
				cue = new Cue(t.Hours, t.Minutes, t.Seconds);
				return true;
			}
		}
		
		if (System.TimeSpan.TryParse(source.Trim(), out TimeSpan timespan))
		{
			cue = new Cue(timespan.Hours,timespan.Minutes, timespan.Seconds)
			{
				raw = source,
			};
			return true;
		}

		cue = null;
		return false;
	}

	private static int GetPrecedingDigits(int mi, string source)
	{
		var digits = "";
		for (int i = mi-1; i >= 0; i--)
		{
			var c = source[i];
			if (char.IsDigit(c))
			{
				digits += c;
			}
			else
			{
				break;
			}
		}

		if (digits.Length > 0)
		{ 
			if(int.TryParse(digits, out int result))
			{
				return result;
			}

			return 0;
		}
		else
		{
			return 0;
		}
	}

	public static Cue StartCue = new Cue(0,0,0)
	{
		raw = ""
	};
}