using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public enum DataType
{
    ORDINAL, NOMINAL
}

public class DataProvider : MonoBehaviour
{
    

    public TextAsset csvFile;

    private string[] variables;
    private string[] units;
    private Dictionary<string, string[]> nominalDatasets;
    private Dictionary<string, float[]> ordinalDatasets;
    private Dictionary<string, DataType> datasetTypes;

    public string[] GetAvailableVariables()
    {
        return variables;
    }

    public string[] GetNominalDataSet(string key)
    {
        return nominalDatasets[key];
    }

    public float[] GetOrdinalDataSet(string key)
    {
        return ordinalDatasets[key];
    }

    public DataType GetTypeOfVariable(string key)
    {
        return datasetTypes[key];
    }

    private void Awake()
    {
        nominalDatasets = new Dictionary<string, string[]>();
        ordinalDatasets = new Dictionary<string, float[]>();
        datasetTypes = new Dictionary<string, DataType>();

        string[][] grid = CSVReader.SplitCsvGrid(csvFile.text);
        variables = new string[grid.Length];
        units = new string[grid.Length];

        DataType type = DataType.NOMINAL;
        float parseResult = 0;

        for (int variable = 0; variable < grid.Length; variable++)
        {
            string currentVariableName = grid[variable][0];
            variables[variable] = currentVariableName;

            if (float.TryParse(grid[variable][1], out parseResult))
            {
                type = DataType.ORDINAL;
            }
            datasetTypes.Add(currentVariableName, type);




            if (type == DataType.ORDINAL)
            {
                float[] newDataSet = new float[grid[variable].Length - 1];
                ordinalDatasets.Add(currentVariableName, newDataSet);

                for (int i = 1; i < grid[variable].Length; i++)
                {
                    if (float.TryParse(grid[variable][i], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat, out parseResult))
                    {
                        newDataSet[i - 1] = parseResult;
                    }
                    else
                    {
                        newDataSet[i - 1] = float.NaN;
                    }
                }

                string ds = currentVariableName;
                for (int i = 0; i < newDataSet.Length; i++)
                {
                    ds += "\n" + newDataSet[i];
                }
                Debug.Log(ds);
            }
            else /* DataType.NOMINAL */
            {
                string[] newDataSet = new string[grid[variable].Length - 1];
                nominalDatasets.Add(currentVariableName, newDataSet);

                for (int i = 1; i < grid[variable].Length; i++)
                {
                    newDataSet[i - 1] = grid[variable][i];
                }

                string ds = currentVariableName;
                for (int i = 0; i < newDataSet.Length; i++)
                {
                    ds += "\n" + newDataSet[i];
                }
                Debug.Log(ds);
            }


        }
    }

    // Use this for initialization
    void Start ()
    {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
