using Model.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class IntervalDataDimensionMeasures
{
    public LevelOfMeasurement type = LevelOfMeasurement.INTERVAL;
    public float range;
    public float min;
    public float max;
    public string variableName;

    public IntervalDataDimensionMeasures(float min, float max, string variableName)
    {
        this.range = max - min;
        this.min = min;
        this.max = max;
        this.variableName = variableName;
    }

    public float NormalizeToRange(int value)
    {
        return (((float)value) / range);
    }
}

public class OrdinalDataDimensionMeasures
{
    public LevelOfMeasurement type = LevelOfMeasurement.ORDINAL;
    public float range;
    public float min;
    public float max;
    public string variableName;

    public OrdinalDataDimensionMeasures(float min, float max, string variableName)
    {
        this.range = max-min;
        this.min = min;
        this.max = max;
        this.variableName = variableName;
    }

    public float NormalizeToRange(int value)
    {
        return (((float)value) / range);
    }
}

public class RatioDataDimensionMeasures
{
    public LevelOfMeasurement type;
    public float range, zeroBoundRange;
    public float min, zeroBoundMin;
    public float max, zeroBoundMax;
    public string variableName;
    public string variableUnit;

    public RatioDataDimensionMeasures(string variableName, float range, float zeroBoundRange, float min, float zeroBoundMin, float max, float zeroBoundMax, string variableUnit = "", LevelOfMeasurement type = LevelOfMeasurement.NOMINAL)
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

public class NominalDataDimensionMeasures
{
    public LevelOfMeasurement type;
    public IDictionary<string, int> distribution;
    public string[] uniqueValues;
    public string variableName;
    public string variableUnit;
    public int minimum, zBoundMin;
    public int maximum, zBoundMax;
    public int range, zBoundRange;
    public int numberOfUniqueValues;

    public NominalDataDimensionMeasures(LevelOfMeasurement type, IDictionary<string,int> distribution, string variableName, string variableUnit)
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
        this.numberOfUniqueValues = distribution.Keys.Count;
        this.uniqueValues = new string[distribution.Keys.Count];

        int counter = 0;
        foreach(string key in distribution.Keys)
        {
            uniqueValues[counter] = key;
            counter++;
        }
    }
}

public class DataSet
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string[] Variables { get; set; }
    public string[] Units { get; set; }

    public IList<InformationObject> informationObjects {get; set;}
    public IDictionary<string, NominalDataDimensionMeasures> dataMeasuresNominal { get; set; }
    public IDictionary<string, OrdinalDataDimensionMeasures> dataMeasuresOrdinal { get; set; }
    public IDictionary<string, IntervalDataDimensionMeasures> dataMeasuresInterval { get; set; }
    public IDictionary<string, RatioDataDimensionMeasures> dataMeasuresRatio { get; set; }

    public string[] nomAttributes { get; set; }
    public string[] ordAttributes { get; set; }
    public string[] ivlAttributes { get; set; }
    public string[] ratAttributes { get; set; }
    public string[] ratioQuadAttributes { get; set; }
    public string[] ratioCubeAttributes { get; set; }
    
    public DataSet(string title, string description, IList<InformationObject> dataObjects)
    {
        Title = title;
        Description = description;
        this.informationObjects = dataObjects;

        this.dataMeasuresNominal = new Dictionary<string, NominalDataDimensionMeasures>();
        this.dataMeasuresOrdinal = new Dictionary<string, OrdinalDataDimensionMeasures>();
        this.dataMeasuresInterval = new Dictionary<string, IntervalDataDimensionMeasures>();
        this.dataMeasuresRatio = new Dictionary<string, RatioDataDimensionMeasures>();
        
        // CALCULATE

        // Calculate Data Measures for nominal attributes
        int nominalCounter = 0;
        foreach(GenericAttribute<string> attribute in dataObjects[0].nominalAtt)
        {
            NominalDataDimensionMeasures measure;
            measure = DataProcessor.NominalAttribute.CalculateMeasures(dataObjects, nominalCounter);
            dataMeasuresNominal.Add(attribute.name, measure);
            nominalCounter++;
        }

        // Calculate Data Measures for ordinal attributes
        int ordinalCounter = 0;
        foreach(GenericAttribute<int> attribute in dataObjects[0].ordinalAtt)
        {
            OrdinalDataDimensionMeasures measure;
            measure = DataProcessor.OrdinalAttribute.CalculateMeasures(dataObjects, ordinalCounter);
            dataMeasuresOrdinal.Add(attribute.name, measure);
            nominalCounter++;
        }



        // Calculate Data Measures for interval Attributes
        int intervalCounter = 0;
        foreach(GenericAttribute<int> attribute in dataObjects[0].intervalAtt)
        {
            IntervalDataDimensionMeasures measure;
            measure = DataProcessor.IntervalAttribute.CalculateMeasures(dataObjects, intervalCounter);
            dataMeasuresInterval.Add(attribute.name, measure);
            intervalCounter++;
        }

        // Calculate Data Measures for ratio Attributes
        int ratioCounter = 0;
        foreach(GenericAttribute<float> attribute in dataObjects[0].ratioAtt)
        {
            RatioDataDimensionMeasures measure;
            measure = DataProcessor.RatioAttribute.CalculateMeasures(dataObjects, ratioCounter);
            dataMeasuresRatio.Add(attribute.name, measure);
            ratioCounter++;
        }

        // Fill variable names
        InformationObject infoObj = dataObjects[0];

        ordAttributes = new string[infoObj.ordinalAtt.Length];
        for(int i = 0; i < dataObjects[0].ordinalAtt.Length; i++)
            ordAttributes[i] = infoObj.ordinalAtt[i].name;

        ratAttributes = new string[infoObj.ratioAtt.Length];
        for(int i = 0; i < dataObjects[0].ratioAtt.Length; i++)
            ratAttributes[i] = infoObj.ratioAtt[i].name;

        nomAttributes = new string[infoObj.nominalAtt.Length];
        for(int i = 0; i < dataObjects[0].nominalAtt.Length; i++)
            nomAttributes[i] = infoObj.nominalAtt[i].name;

        ivlAttributes = new string[infoObj.intervalAtt.Length];
        for(int i = 0; i < dataObjects[0].intervalAtt.Length; i++)
            ivlAttributes[i] = infoObj.intervalAtt[i].name;

        ratioQuadAttributes = new string[infoObj.attributesVector2.Length];
        for(int i = 0; i < dataObjects[0].attributesVector2.Length; i++)
            ratioQuadAttributes[i] = infoObj.attributesVector2[i].name;

        ratioCubeAttributes = new string[infoObj.attributesVector3.Length];
        for(int i = 0; i < dataObjects[0].attributesVector3.Length; i++)
            ratioCubeAttributes[i] = infoObj.attributesVector3[i].name;

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
