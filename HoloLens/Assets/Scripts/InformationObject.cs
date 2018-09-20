using Model;
using Model.Attributes;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Level of Measurement
/// 
/// Nominal: Dresden, Berlin, Hamburg, ...
/// Ordinal: very small, small, medium, big, very big
/// Interval: Monday, Tuesday, Wednesday, Thursday, ...
/// Ratio: 0.01, 0.02, 0.035, 0.24441, ...
/// </summary>
public enum LoM
{
     NOMINAL, ORDINAL, INTERVAL, RATIO
}

/// <summary>
/// Instances of this class represent an information object. An information
/// object can be a measurement containing several values, connected to this
/// measurement. 
/// E.g. 
///     {
///         "Humidity": .9,
///         "Temperature [K]": 304.2,
///         "x": 1.0,
///         "y": 1.0,
///         "z": 1.0,
///         "Pressure [Pa]": 1001.3 
///     }
///         
/// Could be an information object describing the current state of the air
/// in a room at position (1,1,1).
/// </summary>
public class InfoObject
{
    // ........................................................................ Lookup tables
    public IDictionary<string, int> attributeNames;
    public IDictionary<string, LoM> attributeLoMs;

    // ........................................................................ Attribute values
    public GenericAttribute<string>[] nominalAtt;
    public GenericAttribute<int>[] ordinalAtt;
    public GenericAttribute<int>[] intervalAtt;
    public GenericAttribute<float>[] ratioAtt;

    public GenericAttribute<Vector2>[] attributesVector2;
    public GenericAttribute<Vector3>[] attributesVector3;

    // ........................................................................ Observed representatives
    public IDictionary<string, IList<GameObject>> representativeGameObjectsByAttributeName { get; }

    
    // ........................................................................ Constructor
    public InfoObject(
        GenericAttribute<string>[] attributesNom,
        GenericAttribute<int>[] attributesOrd,
        GenericAttribute<int>[] attributesIvl,
        GenericAttribute<float>[] attributesRat,
        GenericAttribute<Vector2>[] attributesVector2, 
        GenericAttribute<Vector3>[] attributesVector3)
    {
        this.representativeGameObjectsByAttributeName = new Dictionary<string, IList<GameObject>>();
        this.nominalAtt = attributesNom;
        this.ordinalAtt = attributesOrd;
        this.intervalAtt = attributesIvl;
        this.ratioAtt = attributesRat;
        this.attributesVector2 = attributesVector2;
        this.attributesVector3 = attributesVector3;

        GenerateLookupTables();
    }

    // ........................................................................ Initializer methods
    private void GenerateLookupTables()
    {
        attributeNames = new Dictionary<string, int>();
        attributeLoMs = new Dictionary<string, LoM>();
        
        for(int i = 0; i < nominalAtt.Length; i++)
        {
            var a = nominalAtt[i];
            attributeNames.Add(a.name, i);
            attributeLoMs.Add(a.name, a.lom);
        }

        for(int i = 0; i < ordinalAtt.Length; i++)
        {
            var a = ordinalAtt[i];
            attributeNames.Add(a.name, i);
            attributeLoMs.Add(a.name, a.lom);
        }

        for(int i = 0; i < intervalAtt.Length; i++)
        {
            var a = intervalAtt[i];
            attributeNames.Add(a.name, i);
            attributeLoMs.Add(a.name, a.lom);
        }

        for(int i = 0; i < ratioAtt.Length; i++)
        {
            var a = ratioAtt[i];
            attributeNames.Add(a.name, i);
            attributeLoMs.Add(a.name, a.lom);
        }
    }


    // ........................................................................ Methods
    public void AddRepresentativeObject(string attributeName, GameObject o)
    {
        if(!representativeGameObjectsByAttributeName.ContainsKey(attributeName))
        {
            representativeGameObjectsByAttributeName.Add(attributeName, new List<GameObject>());
        }
        representativeGameObjectsByAttributeName[attributeName].Add(o);
    }

    // ........................................................................ Getters & Setters
    public string GetNomValue(string attributeName)
    {
        return nominalAtt[attributeNames[attributeName]].value;
    }

    public int GetOrdValue(string attributeName)
    {
        return ordinalAtt[attributeNames[attributeName]].value;
    }

    public int GetIvlValue(string attributeName)
    {
        return intervalAtt[attributeNames[attributeName]].value;
    }

    public float GetRatValue(string attributeName)
    {
        return ratioAtt[attributeNames[attributeName]].value;
    }

    public override string ToString()
    {
        string outString = "";

        foreach(GenericAttribute<string> a in nominalAtt)
        {
            outString += a.name + ": " + a.value + "\t|";
        }

        foreach(GenericAttribute<float> a in ratioAtt)
        {
            outString += a.name + ": " + a.value + "\t|";
        }

        outString += "\n";

        return outString;
    }

    
}
