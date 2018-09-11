using Model.Attributes;
using UnityEngine;

public enum LevelOfMeasurement
{
    ORDINAL, NOMINAL, INTERVAL, RATIO
}

public class InformationObject
{
    public GenericAttribute<string>[] attributesString;
    public GenericAttribute<float>[] attributesFloat;
    public GenericAttribute<int>[] attributesInt;
    public GenericAttribute<Vector2>[] attributesVector2;
    public GenericAttribute<Vector3>[] attributesVector3;

    public InformationObject(
        GenericAttribute<string>[] attributesString, 
        GenericAttribute<float>[] attributesFloat, 
        GenericAttribute<int>[] attributesInt, 
        GenericAttribute<Vector2>[] attributesVector2, 
        GenericAttribute<Vector3>[] attributesVector3)
    {
        this.attributesString = attributesString;
        this.attributesFloat = attributesFloat;
        this.attributesInt = attributesInt;
        this.attributesVector2 = attributesVector2;
        this.attributesVector3 = attributesVector3;
    }

    public override string ToString()
    {
        string outString = "";

        foreach(GenericAttribute<string> a in attributesString)
        {
            outString += a.name + ": " + a.value + "\t|";
        }

        foreach(GenericAttribute<float> a in attributesFloat)
        {
            outString += a.name + ": " + a.value + "\t|";
        }

        outString += "\n";

        return outString;
    }
}
