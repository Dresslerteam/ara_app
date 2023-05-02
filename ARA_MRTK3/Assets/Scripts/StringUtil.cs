using System;

public class StringUtil
{
    public static string AddSpace(string inString)
    {
        string outString = "";

        for (int i = 0; i < inString.Length; i++)
        {
            outString += inString[i];

            if (i + 1 < inString.Length && !Char.IsWhiteSpace(inString[i]) && !Char.IsUpper(inString[i]) && Char.IsUpper(inString[i + 1]))
            {
                outString += " ";
            }
        }
        return outString;
    }
}