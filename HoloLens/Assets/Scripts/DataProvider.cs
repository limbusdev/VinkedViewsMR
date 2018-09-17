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
    
    private void Awake()
    {
        dataSets = new DataSet[csvFile.Length];

        
        for(int i=0; i<csvFile.Length; i++)
        {
            dataSets[i] = ImportDataSet(csvFile[i]);
        }
    }

    private DataSet ImportDataSet(TextAsset csvFile)
    {
        // Initialize Sets
        var nominalDataSets = new Dictionary<string, string[]>();
        var ordinalDataSets = new Dictionary<string, int[]>();
        var intervalDataSets = new Dictionary<string, int[]>();
        var ratioDataSets = new Dictionary<string, float[]>();
        
        var variableTypes = new Dictionary<string, LevelOfMeasurement>();

        // Initialize Grid to store values
        var grid = CSVReader.SplitCsvGrid(csvFile.text);
        var variables = new string[grid.Length];
        var units = new string[grid.Length];
        var loms = new LevelOfMeasurement[grid.Length];


        LevelOfMeasurement type = LevelOfMeasurement.NOMINAL;

        // For every column / variable in grid
        for(int variable = 0; variable < grid.Length; variable++)
        {
            // Extract variable name from first row
            var currentVariableName = grid[variable][0];
            variables[variable] = currentVariableName;

            // Extract Level of Measurement from second row
            var lom = grid[variable][1];
            switch(lom)
            {
                case "ordinal":
                    type = LevelOfMeasurement.ORDINAL;
                    break;
                case "interval":
                    type = LevelOfMeasurement.INTERVAL;
                    break;
                case "ratio":
                    type = LevelOfMeasurement.RATIO;
                    break;
                default:
                    type = LevelOfMeasurement.NOMINAL;
                    break;
            }

            loms[variable] = type;
            variableTypes.Add(currentVariableName, type);

            switch(type)
            {
                case LevelOfMeasurement.ORDINAL:
                    int parseResultI = 0;
                    int[] newDataSetInt = new int[grid[variable].Length - 2];
                    ordinalDataSets.Add(currentVariableName, newDataSetInt);

                    // For every row in this column
                    for(int i = 2; i < grid[variable].Length; i++)
                    {
                        if(int.TryParse(grid[variable][i], out parseResultI))
                        {
                            newDataSetInt[i - 2] = parseResultI;
                        } else
                        {
                            newDataSetInt[i - 2] = int.MinValue;
                        }
                    }
                    break;
                case LevelOfMeasurement.INTERVAL:
                    int parseResultI2 = 0;
                    int[] newDataSetInt2 = new int[grid[variable].Length - 2];
                    intervalDataSets.Add(currentVariableName, newDataSetInt2);

                    // For every row in this column
                    for(int i = 2; i < grid[variable].Length; i++)
                    {
                        if(int.TryParse(grid[variable][i], out parseResultI2))
                        {
                            newDataSetInt2[i - 2] = parseResultI2;
                        } else
                        {
                            newDataSetInt2[i - 2] = int.MinValue;
                        }
                    }
                    break;
                case LevelOfMeasurement.RATIO:
                    float parseResultF = 0;
                    float[] newDataSetFloat = new float[grid[variable].Length - 2];
                    ratioDataSets.Add(currentVariableName, newDataSetFloat);

                    // For every row in this column
                    for(int i = 2; i < grid[variable].Length; i++)
                    {
                        if(float.TryParse(grid[variable][i], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat, out parseResultF))
                        {
                            newDataSetFloat[i - 2] = parseResultF;
                        } else
                        {
                            newDataSetFloat[i - 2] = float.NaN;
                        }
                    }
                    break;
                default:
                    string[] newDataSetString = new string[grid[variable].Length - 2];
                    nominalDataSets.Add(currentVariableName, newDataSetString);

                    for(int i = 2; i < grid[variable].Length; i++)
                    {
                        if(grid[variable][i] == null)
                        {
                            bool x = true;
                            object iu = grid[variable][i];
                        }

                        string value = grid[variable][i];
                        newDataSetString[i - 2] = (value.Length > 0) ? value : "unknown";
                    }
                    break;
            }
            
        }

        return AssembleDataSet(variables, nominalDataSets, ordinalDataSets, intervalDataSets, ratioDataSets, null, null);
    }

    private static DataSet AssembleDataSet(
        string[] variables, 
        IDictionary<string,string[]> nomVars,
        IDictionary<string, int[]> ordVars,
        IDictionary<string, int[]> ivlVars,
        IDictionary<string, float[]> ratioVars,
        IDictionary<string, Vector2[]> vec2Vars,
        IDictionary<string, Vector3[]> vec3Vars)
    {
        int sampleCount = ratioVars.First().Value.Length;
        int nomVarsCount = nomVars.Count;
        int ordVarsCount = ordVars.Count;
        int ivlVarsCount = ivlVars.Count;
        int ratioVarsCount = ratioVars.Count;
        
        int vector2VarsCount = 0;
        int vector3VarsCount = 0;

        var infoObjs = new List<InformationObject>();

        for(int sample = 0; sample < sampleCount; sample++)
        {
            // Create new information object
            InformationObject obj = new InformationObject(
                new GenericAttribute<string>[nomVarsCount],
                new GenericAttribute<int>[ordVarsCount],
                new GenericAttribute<int>[ivlVarsCount],
                new GenericAttribute<float>[ratioVarsCount],
                new GenericAttribute<Vector2>[vector2VarsCount],
                new GenericAttribute<Vector3>[vector3VarsCount]
                );


            // Fill new information object's string attributes
            int variable = 0;
            foreach(string variableName in nomVars.Keys)
            {
                var newAttribute = new GenericAttribute<string>(
                    variableName, nomVars[variableName][sample], LevelOfMeasurement.NOMINAL);
                obj.nominalAtt[variable] = newAttribute;
                variable++;
            }

            // Fill new information object's string attributes
            variable = 0;
            foreach(string variableName in ordVars.Keys)
            {
                var newAttribute = new GenericAttribute<int>(
                    variableName, ordVars[variableName][sample], LevelOfMeasurement.ORDINAL);
                obj.ordinalAtt[variable] = newAttribute;
                variable++;
            }

            // Fill new information object's interval attributes
            variable = 0;
            foreach(string variableName in ivlVars.Keys)
            {
                var newAttribute = new GenericAttribute<int>(
                    variableName, ivlVars[variableName][sample], LevelOfMeasurement.INTERVAL);
                obj.intervalAtt[variable] = newAttribute;
                variable++;
            }

            // Fill new information object's float attributes
            variable = 0;
            foreach(string variableName in ratioVars.Keys)
            {
                var newFloatAttribute = new GenericAttribute<float>(
                    variableName, ratioVars[variableName][sample], LevelOfMeasurement.RATIO);
                obj.ratioAtt[variable] = newFloatAttribute;
                variable++;
            }

            infoObjs.Add(obj);
        }

        DataSet dataSet = new DataSet("DataSet", "", infoObjs);

        return dataSet;
    }
    
}
