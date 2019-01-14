using System.Linq;
using UnityEngine;

/// <summary>
/// Based on CSVReader by Dock. (24/8/11) http://starfruitgames.com
/// 
/// Simplified to fit this project by Georg Eckert (2018-09-01)
/// </summary>
public class CSVReader : MonoBehaviour
{
    public static string[][] SplitCsvGrid(string csvText)
    {
        string[][] outputGrid;
        string[] lines = csvText.Split("\n"[0]);

        int width = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            string[] row = SplitCsvLine(lines[i]);
            width = Mathf.Max(width, row.Length);
        }

        outputGrid = new string[width][];
        for (int i = 0; i < (width); i++) outputGrid[i] = new string[lines.Length];

        for(int row=0; row<lines.Length; row++)
        {
            //Debug.Log(lines[row]);
            string[] rowString = SplitCsvLine(lines[row]);
            for (int col = 0; col < rowString.Length; col++)
            {
                outputGrid[col][row] = rowString[col];
            }
        }

        return outputGrid;
    }

    public static string[] SplitCsvLine(string line)
    {
        return (from System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches(line,
            @"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)",
            System.Text.RegularExpressions.RegexOptions.ExplicitCapture)
                select m.Groups[1].Value).ToArray();
    }
}
