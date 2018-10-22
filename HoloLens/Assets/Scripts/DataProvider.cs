using Model.Attributes;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using System.Linq;
using Model;

public class DataProvider : MonoBehaviour
{
    [System.Serializable]
    public class DictFile { public string name; public TextAsset file; }

    [SerializeField]
    public TextAsset[] csvFiles;
    public DictFile[] ordinalDicts;
    public DataSet[] dataSets;
    public static int dataSetIDs = 0;
    
    private void Awake()
    {
        dataSets = new DataSet[csvFiles.Length];
        
        for(int i=0; i<csvFiles.Length; i++)
        {
            dataSets[i] = ImportDataSet(csvFiles[i]);
            dataSetIDs++;
        }
    }

    private IDictionary<string, IDictionary<int, string>> ParseDictionaries()
    {
        var dictionaries = new Dictionary<string, IDictionary<int, string>>();
        foreach(var d in ordinalDicts)
        {
            var dict = OrdinalValueTranslator.CreateDictionary(d.file.text);
            dictionaries.Add(d.name, dict);
        }

        return dictionaries;
    }

    /// <summary>
    /// Parses the given files contents and puts them into an instance of DataSet
    /// </summary>
    /// <param name="csvFile">file to parse</param>
    /// <returns>DataSet filled with the files contents</returns>
    private DataSet ImportDataSet(TextAsset csvFile)
    {
        // Initialize Sets
        var nominalAttributes   = new Dictionary<string, string[]>();
        var ordinalAtributes    = new Dictionary<string, int[]>();
        var intervalAttributes  = new Dictionary<string, int[]>();
        var ratioAttributes     = new Dictionary<string, float[]>();
        
        var attributeLoMs = new Dictionary<string, LoM>();
        var intervalTranslators = new Dictionary<string, string>();

        // Initialize Grid to store values
        var grid = CSVReader.SplitCsvGrid(csvFile.text);
        var attributes = new string[grid.Length];
        var loms = new LoM[grid.Length];
        
        LoM type;

        // For every column / variable in grid
        for(int variable = 0; variable < grid.Length; variable++)
        {
            // Extract variable name from first row
            var currentVariableName = grid[variable][0];
            attributes[variable] = currentVariableName;

            // Extract Level of Measurement from second row
            var lom = grid[variable][1];

            
            // Detect level of measurement
            switch(lom)
            {
                case "ordinal":
                    type = LoM.ORDINAL;
                    break;
                case "interval":
                    type = LoM.INTERVAL;
                    break;
                case "ratio":
                    type = LoM.RATIO;
                    break;
                default:
                    type = LoM.NOMINAL;
                    break;
            }

            // If interval, get translator name
            if(lom.Split("+"[0]).Length > 1)
            {
                string[] intv = lom.Split("+"[0]);
                intervalTranslators.Add(currentVariableName, intv[1]);
                type = LoM.INTERVAL;
            }

            loms[variable] = type;
            attributeLoMs.Add(currentVariableName, type);


            switch(type)
            {
                case LoM.ORDINAL:
                    int parseResultI = 0;
                    int[] newDataSetInt = new int[grid[variable].Length - 2];
                    ordinalAtributes.Add(currentVariableName, newDataSetInt);

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
                case LoM.INTERVAL:
                    int parseResultI2 = 0;
                    int[] newDataSetInt2 = new int[grid[variable].Length - 2];
                    intervalAttributes.Add(currentVariableName, newDataSetInt2);

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
                case LoM.RATIO:
                    float parseResultF = 0;
                    float[] newDataSetFloat = new float[grid[variable].Length - 2];
                    ratioAttributes.Add(currentVariableName, newDataSetFloat);

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
                    nominalAttributes.Add(currentVariableName, newDataSetString);

                    for(int i = 2; i < grid[variable].Length; i++)
                    {
                        string value = grid[variable][i];
                        newDataSetString[i - 2] = (value == null || value.Length == 0) ? "missingValue" : value;
                    }
                    break;
            }
            
        }

        return AssembleDataSet(
            attributes, 
            nominalAttributes, 
            ordinalAtributes, 
            intervalAttributes, 
            ratioAttributes,
            ParseDictionaries(), 
            intervalTranslators);
    }

    private static DataSet AssembleDataSet(
        string[] variables, 
        IDictionary<string,string[]> nomVars,
        IDictionary<string, int[]> ordVars,
        IDictionary<string, int[]> ivlVars,
        IDictionary<string, float[]> ratioVars,
        IDictionary<string, IDictionary<int, string>> dicts,
        IDictionary<string, string> intervalTranslators)
    {
        int sampleCount = ratioVars.First().Value.Length;
        int nomVarsCount = nomVars.Count;
        int ordVarsCount = ordVars.Count;
        int ivlVarsCount = ivlVars.Count;
        int ratioVarsCount = ratioVars.Count;

        var infoObjs = new List<InfoObject>();

        for(int sample = 0; sample < sampleCount; sample++)
        {
            var nomAtts = new Attribute<string>[nomVarsCount];
            var ordAtts = new Attribute<int>[ordVarsCount];
            var ivlAtts = new Attribute<int>[ivlVarsCount];
            var ratAtts = new Attribute<float>[ratioVarsCount];

            // Fill new information object's nominal attributes
            int variable = 0;
            foreach(string attName in nomVars.Keys)
            {
                var newAttribute = new Attribute<string>(
                    attName, nomVars[attName][sample], LoM.NOMINAL);
                nomAtts[variable] = newAttribute;
                variable++;
            }

            // Fill new information object's ordinal attributes
            variable = 0;
            foreach(string attName in ordVars.Keys)
            {
                var newAttribute = new Attribute<int>(
                    attName, ordVars[attName][sample], LoM.ORDINAL);
                ordAtts[variable] = newAttribute;
                variable++;
            }

            // Fill new information object's interval attributes
            variable = 0;
            foreach(string attName in ivlVars.Keys)
            {
                var newAttribute = new Attribute<int>(
                    attName, ivlVars[attName][sample], LoM.INTERVAL);
                ivlAtts[variable] = newAttribute;
                variable++;
            }

            // Fill new information object's ratio attributes
            variable = 0;
            foreach(string attName in ratioVars.Keys)
            {
                var newFloatAttribute = new Attribute<float>(
                    attName, ratioVars[attName][sample], LoM.RATIO);
                ratAtts[variable] = newFloatAttribute;
                variable++;
            }

            // Create new information object
            var obj = new InfoObject(nomAtts, ordAtts, ivlAtts, ratAtts, dataSetIDs);
            
            infoObjs.Add(obj);
        }

        var dataSet = new DataSet("DataSet", infoObjs, dicts, intervalTranslators);

        return dataSet;
    }

    
    
}
