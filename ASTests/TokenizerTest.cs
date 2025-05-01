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
		        ---
		        hello
		        """;
		var t = new Tokenizer(s);
		Assert.IsTrue(t.Matches(TokenType.StartSlide, 
			TokenType.EndFrontmatter, 
			TokenType.SlideBody
			));
	}

	[Test]
	public void TokenizeNoBreak()
	{
		var s = """
		        ###---
		        hello
		        ###---
		        """;
		var t = new Tokenizer(s);
		Assert.IsTrue(t.Matches(TokenType.StartSlide,
			TokenType.EndFrontmatter,
			TokenType.SlideBody,
			TokenType.StartSlide,
			TokenType.EndFrontmatter
		));
	}
	[Test]
	public void TokenizeSimple()
	{
		var s = """
		        ###---
		        hello
		        ###---
		        """;
		var t = new Tokenizer(s);
		Assert.IsTrue(t.Matches(
			TokenType.StartSlide,
			TokenType.EndFrontmatter,
			TokenType.SlideBody,
			TokenType.StartSlide,
			TokenType.EndFrontmatter
		));
	}

	[Test]
	public void PresentationFrontmatter()
	{
		var s = """
		        key value
		        another_key #000
		        """;
		var t = new Tokenizer(s);
		Assert.IsTrue(t.Matches(
			TokenType.Ident,
			TokenType.Ident,
			TokenType.Linebreak,
			TokenType.Ident,
			TokenType.Ident,
			TokenType.Linebreak
		));
	}

	[Test]
	public void PresentationFrontmatterAndSlideFrontmatter()
	{
		var s = """
		        key value
		        another_key #000
		        ###
		        key value
		        ---
		        body
		        """;
		var t = new Tokenizer(s);
		Assert.IsTrue(t.Matches(
			TokenType.Ident,
			TokenType.Ident,
			TokenType.Linebreak,
			TokenType.Ident,
			TokenType.Ident,
			TokenType.Linebreak,
			TokenType.StartSlide,
			TokenType.Ident,
			TokenType.Ident,
			TokenType.Linebreak,
			TokenType.EndFrontmatter,
			TokenType.SlideBody
		));
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