using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationObject
{
    

    public float[] attributeValues;
    public string[] attributeNames;
    
    public InformationObject() {}

    public InformationObject(string[] attributeNames, float[] attributeValues)
    {
        this.attributeNames = attributeNames;
        this.attributeValues = attributeValues;
    }
}
