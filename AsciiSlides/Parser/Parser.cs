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

	private static TextParser<string> Ident =
		from w in Character.WhiteSpace.IgnoreMany()
		from s in (Character.LetterOrDigit.Or(Character.In(['#','@','&','_','-','^','+','~']))).Many()
		select s.ToString();

	private static TextParser<(string, string)> frontmatterItem =>
		from f in Superpower.Parse.Sequence(Ident, Ident)
		from w in Character.WhiteSpace.Many()
		select f;

	private static TextParser<Frontmatter> PresFrontmatter =>
		from x in frontmatterItem.Many().OptionalOrDefault()
		from w in Character.WhiteSpace.Many()
		select new Frontmatter(x);

	private static TextParser<string> SlideStart =>
		from x in Character.EqualTo('#').Repeat(3)
		select x.ToString();

	private static TextParser<string> FrontEnd =>
		from x in Character.EqualTo('-').Repeat(3)
		select x.ToString();
	private static TextParser<string> PresContent =>
		from x in Character.AnyChar.Many()
		select x.ToString();

	private static TextParser<Slide> Slide =>
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