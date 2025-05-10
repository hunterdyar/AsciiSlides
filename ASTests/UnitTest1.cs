using System.IO;
using AsciiSlidesCore.Parser;
using NUnit.Framework;

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
		//	var presentation = PresentationParser.Parse(TestData.TestPres1);
		var	presentation = PresentationParser.Parse(TestData.TestPres2);

		}
	}
}