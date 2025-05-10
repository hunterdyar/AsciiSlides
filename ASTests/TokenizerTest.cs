using System;
using System.Diagnostics;
using AsciiSlidesCore.Parser;
using NUnit.Framework;

namespace TestProject1;

public class TokenizerTest
{
	[SetUp]
	public void Setup()
	{
	}

	[Test]
	public void TokenizeNoCloser()
	{
		var s = """
		        ###
		        key: "value"
		        """;
		var t = new Tokenizer(s);
		Assert.IsTrue(t.Matches(TokenType.SlideSep, 
			TokenType.Key, 
			TokenType.Value
			));
	}
	
	[Test]
	public void TokenizeSimple()
	{
		var s = """
		        ###
		        ascii: { hi }
		        ###
		        """;
		var t = new Tokenizer(s);
		Assert.IsTrue(t.Matches(
			TokenType.SlideSep,
			TokenType.Key,
			TokenType.Value,
			TokenType.SlideSep
		));
	}

	[Test]
	public void PresentationFrontmatter()
	{
		var s = """
		        key: "value"
		        another_key: "#000"
		        """;
		var t = new Tokenizer(s);
		Assert.IsTrue(t.Matches(
			TokenType.Key,
			TokenType.Value,
			TokenType.Key,
			TokenType.Value
		));
	}

	[Test]
	public void PresentationFrontmatterAndSlideFrontmatter()
	{
		var s = """
		        key: < value >
		        another_key: -- #000 --
		        ###
		        key: { value }
		        ###
		        body: { body }
		        """;
		var t = new Tokenizer(s);
		Assert.IsTrue(t.Matches(
			TokenType.Key,
			TokenType.Value,
			TokenType.Key,
			TokenType.Value,
			TokenType.SlideSep,
			TokenType.Key,
			TokenType.Value,
			TokenType.SlideSep,
			TokenType.Key,
			TokenType.Value
		));
	}

	[Test]
	public void FrontmatterStrings()
	{
		var s = """
		        key: "this is a string with an \" escape"
		        another_key: "this is a string"
		        ###
		        key: "this string
		        is on multiple
		        lines"
		        """;
		var t = new Tokenizer(s);
		Assert.IsTrue(t.Matches(
			TokenType.Key,
			TokenType.Value,
			TokenType.Key,
			TokenType.Value,
			TokenType.SlideSep,
			TokenType.Key,
			TokenType.Value
		));
	}



	[Test]
	public void SpeakerNotesTest()
	{
		var s = """
		        ###
		        notes: "hello"
		        ###
		        notes: {{ now
		        i am speaking
		        }}
		        """;
		var p = PresentationParser.Parse(s);
		Assert.IsTrue(p.SlideCount == 3);
		Assert.IsTrue(p.Slides[0].HasSpeakerNotes);
		Assert.IsTrue(p.Slides[0].SpeakerNotes == "hello");
		Assert.IsTrue(p.Slides[1].HasSpeakerNotes);
		Assert.IsTrue(p.Slides[1].SpeakerNotes == " now\ni am speaking\n" || p.Slides[1].SpeakerNotes == " now\r\ni am speaking\r\n" );
	}
}

public static class TokenizerTestExtensions
{
	public static bool Matches(this Tokenizer tokenizer, params TokenType[] test)
	{
		if (tokenizer.Tokens.Count != test.Length)
		{
			TestContext.Out.WriteLine("Comparing Tokens and Length");
			TestContext.Out.WriteLine($" GIVEN    |    WANTED");

			int c = int.Max(tokenizer.Tokens.Count, test.Length);
			for (int i = 0; i < c; i++)
			{
				string given = "xxx";
				string wanted = "xxx";
				if (tokenizer.Tokens.Count > i)
				{
					given = tokenizer.Tokens[i].ToString();
				}

				if (test.Length > i)
				{
					wanted = test[i].ToString();
				}

				TestContext.Out.WriteLine($"{given} | {wanted}");
			}
			return false;
		}
		for (int i = 0; i < test.Length; i++)
		{
			if (test[i] != tokenizer.Tokens[i].Type)
			{
				TestContext.Out.WriteLine($"Mismatch Between Tokenizer and expected at {i}. Got {tokenizer.Tokens[i].Type} but expected {test[i]}.");
				return false;
			}
		}

		return true;
	}
}