using System.IO;
using AsciiSlides.Parser;
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
		
		[Test]
		
		public void FullPresentations()
		{
			var presentation = PresentationParser.Parse(TestData.TestPres1);
			presentation = PresentationParser.Parse(TestData.TestPres2);

		}
	}
}