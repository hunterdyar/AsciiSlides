using System.IO;
using NUnit.Framework;
using Superpower;

namespace TestProject1
{
	public class Tests
	{
		[SetUp]
		public void Setup()
		{
		}

		[TestCase("hello")]
		[TestCase("42")]
		[TestCase("#0F0F0F")]
		[TestCase("#FFF")]
		[TestCase("10.2")]
		[TestCase("100e2.7")]

		public void Identifier(string source)
		{
			var ident = AsciiSlides.Parser.PresentationParser.Ident.Parse(source);
			Assert.AreEqual(source, ident);
		}

		[TestCase("Hello World")]
		[TestCase("HelloWorld\n")]
		public void BadIdentifier(string source)
		{
			var ident = AsciiSlides.Parser.PresentationParser.Ident.Parse(source);
			Assert.AreNotEqual(source, ident);
		}
		
		[Test]
		public void FrontmatterItem()
		{
			var fi = AsciiSlides.Parser.PresentationParser.FrontmatterItem.Parse("""type youtube""");
			Assert.AreEqual("type", fi.Item1.ToString());
			Assert.AreEqual("youtube", fi.Item2.ToString());
		}

		[Test]
		public void BasicSlide()
		{
			var slide = AsciiSlides.Parser.PresentationParser.Slide.Parse("""
			                                                              ###
			                                                              ---
				this is a slide!
""");
		}
		[TestCase()]
		public void Presentation()
		{
			var presentation = AsciiSlides.Parser.PresentationParser.Parse(TestData.TestPres1);
		}
	}
}