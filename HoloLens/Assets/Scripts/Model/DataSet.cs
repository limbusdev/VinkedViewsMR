using Model.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataDimensionMeasures
{
    public LevelOfMeasurement type;
    public float range, zeroBoundRange;
    public float min, zeroBoundMin;
    public float max, zeroBoundMax;
    public string variableName;
    public string variableUnit;

    public DataDimensionMeasures(string variableName, float range, float zeroBoundRange, float min, float zeroBoundMin, float max, float zeroBoundMax, string variableUnit = "", LevelOfMeasurement type = LevelOfMeasurement.NOMINAL)
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
    public IList<InformationObject> dataObjects {get; set;}
    public IDictionary<string, DataDimensionMeasures> dataMeasuresFloat { get; set; }
    
    public DataSet(string title, string description, IList<InformationObject> dataObjects)
    {
        Title = title;
        Description = description;
        this.dataObjects = dataObjects;
        this.dataMeasuresFloat = new Dictionary<string, DataDimensionMeasures>();

        // Calculate Data Measures for Float Attributes
        int floatAttCounter = 0;
        foreach(GenericAttribute<float> attribute in dataObjects[0].attributesFloat)
        {
            DataDimensionMeasures measure;
            measure = DataProcessor.FloatAttribute.CalculateMeasures(dataObjects, floatAttCounter);
            dataMeasuresFloat.Add(attribute.name, measure);
            floatAttCounter++;
        }

    }

    public override string ToString()
    {
        string outString = "";

        outString += "DataSet: " + Title + "\n\n";

        foreach(InformationObject o in dataObjects)
        {
            outString += o.ToString() + "\n";
        }

        return outString;
    }
}
