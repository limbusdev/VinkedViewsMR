using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataProcessor
{
    /**
     * Calculates the range from minimum to maximum in the available attribute.
     * This is needed to scale the bars appropriately.
     * */
    public static float CalculateRange(IDictionary<string, DataObject> data, int attributeID)
    {
        float Maximum = 0;
        float Minimum = 0;

        foreach (DataObject dataObject in data.Values)
        {
            float attributeValue = dataObject.attributeValues[attributeID];
            if (attributeValue > Maximum)
            {
                Maximum = attributeValue;
            }
            if (attributeValue < Minimum)
            {
                Minimum = attributeValue;
            }
        }

        return Maximum - Minimum;
    }
}
