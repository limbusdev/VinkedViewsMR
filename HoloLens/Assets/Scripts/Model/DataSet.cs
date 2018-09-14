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

    public float NormalizeToRange(float value)
    {
        Debug.Log(value + " normalized by r " + range + " to " + ((value-min) / range));
        return (value-min) / range;
    }

    public float NormalizeToZeroBoundRange(float value)
    {
        Debug.Log(value + " normalized by zbr " + zeroBoundRange + " to " + ((value - zeroBoundMin) / zeroBoundRange));
        return (value-zeroBoundMin) / zeroBoundRange;
    }
}

public class DataSet
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string[] Variables { get; set; }
    public string[] Units { get; set; }
    public IList<InformationObject> informationObjects {get; set;}
    public IDictionary<string, DataDimensionMeasures> dataMeasuresFloat { get; set; }

    public string[] attributesFloat { get; set; }
    public string[] attributesInt { get; set; }
    public string[] attributesString { get; set; }
    public string[] attributesVector2 { get; set; }
    public string[] attributesVector3 { get; set; }
    
    public DataSet(string title, string description, IList<InformationObject> dataObjects)
    {
        Title = title;
        Description = description;
        this.informationObjects = dataObjects;
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

        // Fill variable names
        InformationObject infoObj = dataObjects[0];

        attributesInt = new string[infoObj.attributesInt.Length];
        for(int i = 0; i < dataObjects[0].attributesInt.Length; i++)
            attributesInt[i] = infoObj.attributesInt[i].name;

        attributesFloat = new string[infoObj.attributesFloat.Length];
        for(int i = 0; i < dataObjects[0].attributesFloat.Length; i++)
            attributesFloat[i] = infoObj.attributesFloat[i].name;

        attributesString = new string[infoObj.attributesString.Length];
        for(int i = 0; i < dataObjects[0].attributesString.Length; i++)
            attributesString[i] = infoObj.attributesString[i].name;

        attributesVector2 = new string[infoObj.attributesVector2.Length];
        for(int i = 0; i < dataObjects[0].attributesVector2.Length; i++)
            attributesVector2[i] = infoObj.attributesVector2[i].name;

        attributesVector3 = new string[infoObj.attributesVector3.Length];
        for(int i = 0; i < dataObjects[0].attributesVector3.Length; i++)
            attributesVector3[i] = infoObj.attributesVector3[i].name;

    }

    public override string ToString()
    {
        string outString = "";

        outString += "DataSet: " + Title + "\n\n";

        foreach(InformationObject o in informationObjects)
        {
            outString += o.ToString() + "\n";
        }

        return outString;
    }
}
