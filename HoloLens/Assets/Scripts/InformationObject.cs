using Model;
using Model.Attributes;
using System.Collections.Generic;
using UnityEngine;

public enum LevelOfMeasurement
{
     NOMINAL, ORDINAL, INTERVAL, RATIO
}

public class InformationObject
{
    public IDictionary<string, IList<GameObject>> representativeGameObjectsByAttributeName { get; }

    public GenericAttribute<string>[] nominalAtt;
    public GenericAttribute<int>[] ordinalAtt;
    public GenericAttribute<int>[] intervalAtt;
    public GenericAttribute<float>[] ratioAtt;
    
    public GenericAttribute<Vector2>[] attributesVector2;
    public GenericAttribute<Vector3>[] attributesVector3;

    public InformationObject(
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
    }

    public string GetNomValue(string attributeName, DataSet data)
    {
        return nominalAtt[data.GetIDOf(attributeName)].value;
    }

    public int GetOrdValue(string attributeName, DataSet data)
    {
        return ordinalAtt[data.GetIDOf(attributeName)].value;
    }

    public int GetIvlValue(string attributeName, DataSet data)
    {
        return intervalAtt[data.GetIDOf(attributeName)].value;
    }

    public float GetRatValue(string attributeName, DataSet data)
    {
        return ratioAtt[data.GetIDOf(attributeName)].value;
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

    public void AddRepresentativeObject(string attributeName, GameObject o)
    {
        if(!representativeGameObjectsByAttributeName.ContainsKey(attributeName))
        {
            representativeGameObjectsByAttributeName.Add(attributeName, new List<GameObject>());
        }
        representativeGameObjectsByAttributeName[attributeName].Add(o);
    }
}
