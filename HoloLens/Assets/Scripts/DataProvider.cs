using Model.Attributes;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using System.Linq;


public class DataProvider : MonoBehaviour
{
    

    public TextAsset[] csvFile;
    public DataSet[] dataSets;

    private string[] variables;
    private string[] units;
    private Dictionary<string, string[]> stringDatasets;
    private Dictionary<string, float[]> floatDatasets;
    private Dictionary<string, LevelOfMeasurement> variableTypes;

    public string[] GetAvailableVariables()
    {
        return variables;
    }

    public string[] GetNominalDataSet(string key)
    {
        return stringDatasets[key];
    }

    public float[] GetOrdinalDataSet(string key)
    {
        return floatDatasets[key];
    }

    public LevelOfMeasurement GetTypeOfVariable(string key)
    {
        return variableTypes[key];
    }

    private void Awake()
    {
        dataSets = new DataSet[csvFile.Length];

        
        for(int i=0; i<csvFile.Length; i++)
        {
            dataSets[i] = ImportDataSet(csvFile[i]);
        }
        

        //Debug.Log(dataSets[0]);

        //Debug.Log(dataSets[1]);
    }

    private DataSet ImportDataSet(TextAsset csvFile)
    {
        // Initialize Sets
        stringDatasets = new Dictionary<string, string[]>();
        floatDatasets = new Dictionary<string, float[]>();
        variableTypes = new Dictionary<string, LevelOfMeasurement>();

        // Initialize Grid to store values
        string[][] grid = CSVReader.SplitCsvGrid(csvFile.text);
        variables = new string[grid.Length];
        units = new string[grid.Length];

        LevelOfMeasurement type = LevelOfMeasurement.NOMINAL;
        float parseResult = 0;

        // Iterate over Grid
        for(int variable = 0; variable < grid.Length; variable++)
        {
            string currentVariableName = grid[variable][0];
            variables[variable] = currentVariableName;

            if(float.TryParse(grid[variable][1], out parseResult))
            {
                type = LevelOfMeasurement.RATIO;
            }
            variableTypes.Add(currentVariableName, type);


            if(type == LevelOfMeasurement.RATIO)
            {
                float[] newDataSet = new float[grid[variable].Length - 1];
                floatDatasets.Add(currentVariableName, newDataSet);

                for(int i = 1; i < grid[variable].Length; i++)
                {
                    if(float.TryParse(grid[variable][i], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat, out parseResult))
                    {
                        newDataSet[i - 1] = parseResult;
                    } else
                    {
                        newDataSet[i - 1] = float.NaN;
                    }
                }

                string ds = currentVariableName;
                for(int i = 0; i < newDataSet.Length; i++)
                {
                    ds += "\n" + newDataSet[i];
                }
                //Debug.Log(ds);
            } else /* LevelOfMeasurement.NOMINAL */
            {
                string[] newDataSet = new string[grid[variable].Length - 1];
                stringDatasets.Add(currentVariableName, newDataSet);

                for(int i = 1; i < grid[variable].Length; i++)
                {
                    newDataSet[i - 1] = grid[variable][i];
                }

                string ds = currentVariableName;
                for(int i = 0; i < newDataSet.Length; i++)
                {
                    ds += "\n" + newDataSet[i];
                }
                //Debug.Log(ds);
            }
        }

        return AssembleDataSet(variables, stringDatasets, floatDatasets, null, null, null);
    }

    private static DataSet AssembleDataSet(
        string[] variables, 
        IDictionary<string,string[]> stringVars, 
        IDictionary<string, float[]> floatVars,
        IDictionary<string, int[]> intVars,
        IDictionary<string, Vector2[]> vec2Vars,
        IDictionary<string, Vector3[]> vec3Vars)
    {
        int sampleCount = floatVars.First().Value.Length;
        int stringVarsCount = stringVars.Count;
        int floatVarsCount = floatVars.Count;
        int intVarsCount = 0;
        int vector2VarsCount = 0;
        int vector3VarsCount = 0;

        var infoObjs = new List<InformationObject>();

        for(int sample = 0; sample < sampleCount; sample++)
        {
            // Create new information object
            InformationObject obj = new InformationObject(
                new GenericAttribute<string>[stringVarsCount],
                new GenericAttribute<float>[floatVarsCount],
                new GenericAttribute<int>[intVarsCount],
                new GenericAttribute<Vector2>[vector2VarsCount],
                new GenericAttribute<Vector3>[vector3VarsCount]
                );

            // Fill new information object's float attributes
            int variable = 0;
            foreach(string variableName in floatVars.Keys)
            {
                var newFloatAttribute = new GenericAttribute<float>(
                    variableName, floatVars[variableName][sample], LevelOfMeasurement.RATIO);
                obj.attributesFloat[variable] = newFloatAttribute;
                variable++;
            }

            // Fill new information object's string attributes
            variable = 0;
            foreach(string variableName in stringVars.Keys)
            {
                var newAttribute = new GenericAttribute<string>(
                    variableName, stringVars[variableName][sample], LevelOfMeasurement.NOMINAL);
                obj.attributesString[variable] = newAttribute;
                variable++;
            }

            infoObjs.Add(obj);
        }

        DataSet dataSet0 = new DataSet("DataSet0", "", infoObjs);

        return dataSet0;
    }
    
}
