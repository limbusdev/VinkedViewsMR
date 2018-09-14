using Model.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FloatDataDimensionMeasures
{
    public LevelOfMeasurement type;
    public float range, zeroBoundRange;
    public float min, zeroBoundMin;
    public float max, zeroBoundMax;
    public string variableName;
    public string variableUnit;

    public FloatDataDimensionMeasures(string variableName, float range, float zeroBoundRange, float min, float zeroBoundMin, float max, float zeroBoundMax, string variableUnit = "", LevelOfMeasurement type = LevelOfMeasurement.NOMINAL)
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
        return (value-min) / range;
    }

    public float NormalizeToZeroBoundRange(float value)
    {
        return (value-zeroBoundMin) / zeroBoundRange;
    }
}

public class StringDataDimensionMeasures
{
    public LevelOfMeasurement type;
    public IDictionary<string, int> distribution;
    public string variableName;
    public string variableUnit;
    public int minimum, zBoundMin;
    public int maximum, zBoundMax;
    public int range, zBoundRange;

    public StringDataDimensionMeasures(LevelOfMeasurement type, IDictionary<string,int> distribution, string variableName, string variableUnit)
    {
        this.type = type;
        this.variableName = variableName;
        this.variableUnit = variableUnit;
        this.distribution = distribution;

        this.minimum = distribution.Values.Min();
        this.maximum = distribution.Values.Max();
        this.zBoundMin = 0;
        this.zBoundMax = (maximum == 0) ? 0 : maximum;
        this.range = maximum - minimum;
        this.zBoundRange = zBoundMax - zBoundMin;
    }
}

public class DataSet
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string[] Variables { get; set; }
    public string[] Units { get; set; }
    public IList<InformationObject> informationObjects {get; set;}
    public IDictionary<string, FloatDataDimensionMeasures> dataMeasuresFloat { get; set; }
    public IDictionary<string, StringDataDimensionMeasures> dataMeasuresString { get; set; }

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
        this.dataMeasuresFloat = new Dictionary<string, FloatDataDimensionMeasures>();
        this.dataMeasuresString = new Dictionary<string, StringDataDimensionMeasures>();

        // Calculate Data Measures for Float Attributes
        int floatAttCounter = 0;
        foreach(GenericAttribute<float> attribute in dataObjects[0].attributesFloat)
        {
            FloatDataDimensionMeasures measure;
            measure = DataProcessor.FloatAttribute.CalculateMeasures(dataObjects, floatAttCounter);
            dataMeasuresFloat.Add(attribute.name, measure);
            floatAttCounter++;
        }

        // Calculate Data Measures for String Attributes
        int stringAttCounter = 0;
        foreach(GenericAttribute<string> attribute in dataObjects[0].attributesString)
        {
            StringDataDimensionMeasures measure;
            measure = DataProcessor.StringAttribute.CalculateMeasures(dataObjects, stringAttCounter);
            dataMeasuresString.Add(attribute.name, measure);
            stringAttCounter++;
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
