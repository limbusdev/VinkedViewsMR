using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CSVReader : MonoBehaviour
{
    

	// Use this for initialization
	void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

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
        /*
        string debugString = "";
        for(int i=0; i<outputGrid[0].Length; i++)
        {
            for(int ii=0; ii<outputGrid.Length; ii++)
            {
                debugString += outputGrid[ii][i] + "\t";
            }
            debugString += "\n";
        }

        Debug.Log(debugString);

        debugString = "";
        for(int i=0; i<outputGrid[0].Length; i++)
        {
            debugString += outputGrid[0][i] + "\n";
        }
        Debug.Log(debugString);
        */

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
