using System.Diagnostics.Tracing;
using Markdig.Helpers;

namespace AsciiSlidesCore.Parser;

public class Delimiter
{
    public readonly string Open;
    public readonly string Close;
    public Delimiter(ref ReadOnlySpan<char> opening)
    {
        int letterCount = 0;
        int numberCount = 0;
        int lowerCount = 0;
        int upperCount = 0;
        //count 
       
        (letterCount, numberCount, lowerCount, upperCount) = LetterCount(ref opening);

        if (letterCount == 0 && numberCount == 0)
        {
            //symbols only
            this.Open = opening.ToString();
            if (TryPlaceholderClosing(ref opening, out Close))
            {
                return;
            }
            else if (TryBraceClosing(ref opening, out Close))
            {
                return;
            }
        }
        
        //ALLCAPSDELIM
        if (opening.Length == upperCount+numberCount)
        {
            this.Open = opening.ToString();
            Close = opening.ToString();
            return;
        }

        throw new BadDelimiterException(opening.ToString(), "Invalid Delimiter. Delimiters must be <{([braces] or ALLCAPS.");
    }

    private static (int letterCount, int numberCount, int lowerCount, int upperCount) LetterCount(ref ReadOnlySpan<char> opening)
    {
        int letterCount = 0;
        int numberCount = 0;
        int lowerCount = 0;
        int upperCount = 0;
        for (int i = 0; i < opening.Length; i++)
        {
            char c = opening[i];
            if (char.IsWhiteSpace(c))
            {
                throw new BadDelimiterException("opening", "Has Whitespace.");
            }

            if (char.IsLetter(c))
            {
                letterCount++;
                if (char.IsLower(c))
                {
                    lowerCount++;
                }else if (char.IsUpper(c))
                {
                    upperCount++;
                }
            }
            else if (char.IsNumber(c))
            {
                numberCount++;
            }
        }

        return (letterCount, numberCount, lowerCount, upperCount);
    }
    
    private static bool TryBraceClosing(ref ReadOnlySpan<char> opening, out string o)
    {
        o = opening.ToString();
        for (int i = 0; i < opening.Length; i++)
        {
            char ch = opening[i];
            if (ch.IsSymmetricDelimiter())
            {
                continue;
            }
            
            if (ch.IsLeftBrace(out var r))
            {
                o = o.Replace(ch,r);
                continue;
            }
            
            //two above ifs are the only valid option for brace-style delims.
            return false;
        }

        o = o.ToString().Reverse();
        return true;
    }

    private static bool TryPlaceholderClosing(ref ReadOnlySpan<char> open, out string o)
    {
        switch (open)
        {
            case "<!--":
                o = "--!>";
                return true;
            case "START":
                o = "STOP";
                return true;
            case "BEGIN":
                o= "END";
                return true;
            case "\"\"": //double quotes
                o = "\"\"";
                return true;
            case "'":
                o = "'";
                return true;
        }

        o = open.ToString();
        return false;
    }
}