using System.Text;
using Superpower;
using Superpower.Display;
using Superpower.Model;
using Superpower.Parsers;
using Superpower.Tokenizers;

namespace AsciiSlides.Parser;


public static class PresentationParser
{
	private static Presentation Instance;

	public static TextParser<string> Ident =
		from w in Character.WhiteSpace.IgnoreMany()
		from s in ((Character.LetterOrDigit.Or(Character.In('@','#','.','_','-','+','!','\'','\"'))).AtLeastOnce())
		select new string(s);

	public static TextParser<Unit> WhitespaceNoBreak =>
		from w in Character.In(' ', '\t', '\n', '\r').Many()
		select new Unit();

	public static TextParser<Unit> Linebreak =>
		from w in Character.EqualTo('\r').Optional()
		from x in Character.EqualTo('\n')
		select new Unit();
	
	public static TextParser<(string, string)> FrontmatterItem =>
		from w in WhitespaceNoBreak.Many()
		from i1 in Ident
		from w2 in WhitespaceNoBreak.Many()
		from i2 in Ident
		from w3 in WhitespaceNoBreak.Many()
		from w4 in Linebreak
		select (i1,i2);

	public static TextParser<Frontmatter> PresFrontmatter =>
		from w1 in Character.WhiteSpace.Many()
		from x in FrontmatterItem.Many().OptionalOrDefault()
		from w2 in Character.WhiteSpace.Many()
		select new Frontmatter(x);

	public static TextParser<string> SlideStart =>
		from w in WhitespaceNoBreak.Try()
		from b in Linebreak.Optional()
		from w2 in WhitespaceNoBreak.Many()
		from x in Character.EqualTo('#').Repeat(3)
		select x.ToString();

	public static TextParser<string> FrontEnd =>
		from x in Character.EqualTo('-').Repeat(3)
		select x.ToString();
	public static TextParser<string> PresContent =>
		//todo: exclude ###
		from x in Character.AnyChar.Many()
		// from y in SlideStart.don
		select x.ToString();

	public static TextParser<Slide> Slide =>
		from start in SlideStart
		from f in PresFrontmatter.OptionalOrDefault()
		from fe in FrontEnd
		from c in PresContent
		select new Slide(f, c);

	private static TextParser<Slide[]> Slides =>
		from s in Slide.Many()
		select s;

	private static TextParser<Presentation> Presentation =
		from f in PresFrontmatter
		from s in Slides
		select new Presentation(f, s);

	private static TextParser<Presentation> PresentationOnly => Presentation.AtEnd();

	public static Presentation Parse(string input)
	{
		return PresentationOnly.Parse(input);
	}
	
}