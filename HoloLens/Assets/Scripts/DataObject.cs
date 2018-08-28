using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataObject {

    public float[] attributeValues;
    public string[] attributeNames;
    
    public DataObject() {}

    public DataObject(string[] attributeNames, float[] attributeValues)
    {
        this.attributeNames = attributeNames;
        this.attributeValues = attributeValues;
    }
}
