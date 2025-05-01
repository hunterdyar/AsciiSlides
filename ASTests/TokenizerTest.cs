using System;
using System.Diagnostics;
using AsciiSlides.Parser;
using NUnit.Framework;

namespace TestProject1;

public class TokenizerTest
{
	[SetUp]
	public void Setup()
	{
	}

	[Test]
	public void TokenizeSimple()
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