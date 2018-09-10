using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataDimensionMeasures
{
    public DataType type;
    public float range, zeroBoundRange;
    public float min, zeroBoundMin;
    public float max, zeroBoundMax;
    public string variableName;
    public string variableUnit;

    public DataDimensionMeasures(string variableName, float range, float zeroBoundRange, float min, float zeroBoundMin, float max, float zeroBoundMax, string variableUnit = "", DataType type = DataType.ORDINAL)
    {
        this.type = type;
        this.range = range;
        this.zeroBoundRange = zeroBoundRange;
        this.min = min;
        this.zeroBoundMin = zeroBoundMin;
        this.max = max;
        this.zeroBoundMax = zeroBoundMax;
        this.variableName = variableName;
        this.variableUnit = variableUnit;
    }
}

public class DataSet
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string[] Variables { get; set; }
    public string[] Units { get; set; }
    public IDictionary<string, InformationObject> dataObjects {get; set;}
    public IDictionary<string, DataDimensionMeasures> dataMeasures { get; set; }
    

    

    public DataSet(string title, string description, string[] variables, string[] units, IDictionary<string, InformationObject> dataObjects)
    {
        Title = title;
        Description = description;
        Variables = variables;
        Units = units;
        this.dataObjects = dataObjects;

        // Calculate Data Measures
        foreach(string variable in dataObjects.Keys)
        {
            string varName = variable;
            InformationObject dataObj = dataObjects[variable];
        }

        /*
        this.min = ordinalValues[0, 0];
        this.max = ordinalValues[0, 0];
        for(int row = 0; row < ordinalValues.Rank; row++)
        {
            for(int col = 0; col < ordinalValues.GetLength(row); col++)
            {
                float value = ordinalValues[row, col];
                if(value < this.min)
                {
                    this.min = value;
                }
                if(value > this.max)
                {
                    this.max = value;
                }
            }
        }

        this.range = this.max - this.min;
        this.zeroBoundMin = (this.min > 0) ? 0 : this.min;
        this.zeroBoundMax = (this.max < 0) ? 0 : this.max;
        this.zeroBoundRange = this.zeroBoundMax - this.zeroBoundMin;*/
    }
}
