using Model;
using Model.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataProcessor
{
    public static class StringAttribute
    {
        public static StringDataDimensionMeasures CalculateMeasures(IList<InformationObject> os, int aID)
        {
            var measures = new StringDataDimensionMeasures(
                os[0].attributesString[aID].levelOfMeasurement,
                CalculateDistribution(os, aID),
                os[0].attributesString[aID].name,
                "");

            return measures;
        }

        public static string[] FindUniqueValues(IList<InformationObject> os, int aID)
        {
            var uniqueList = new List<string>();

            foreach(var o in os)
            {
                var a = o.attributesString[aID];
                if(!uniqueList.Contains(a.value))
                {
                    uniqueList.Add(a.value);
                }
            }

            return uniqueList.ToArray();
        }

        public static IDictionary<string, int> CalculateDistribution(IList<InformationObject> os, int aID)
        {
            var distribution = new Dictionary<string, int>();

            string[] keys = FindUniqueValues(os, aID);

            foreach(var key in keys)
            {
                distribution.Add(key, 0);
            }

            foreach(var o in os)
            {
                var a = o.attributesString[aID];
                distribution[a.value] = distribution[a.value] + 1;
            }

            return distribution;
        }

        public static int CountObjectsMatchingTwoCattegories(IList<InformationObject> os, int aID1, int aID2, string value1, string value2)
        {
            int counter = 0;
            foreach(var o in os)
            {
                var v1 = o.attributesString[aID1].value;
                var v2 = o.attributesString[aID2].value;
                
                if(v1.Equals(value1) && v2.Equals(value2))
                {
                    counter++;
                }
            }
            return counter;
        }
    }

    public static class FloatAttribute
    {
        public static FloatDataDimensionMeasures CalculateMeasures(IList<InformationObject> os, int aID)
        {
            var measures = new FloatDataDimensionMeasures(
                os[0].attributesFloat[aID].name,
                CalculateRange(os, aID),
                CalculateZeroBoundRange(os, aID),
                CalculateMin(os, aID),
                CalculateZeroBoundMin(os, aID),
                CalculateMax(os, aID),
                CalculateZeroBoundMax(os, aID),
                "",
                os[0].attributesFloat[aID].levelOfMeasurement
                );

            return measures;
        }

        public static float CalculateMin(IList<InformationObject> os, int attributeID)
        {
            float minimum = float.MaxValue;
            foreach(InformationObject dataObject in os)
            {
                var attribute = dataObject.attributesFloat[attributeID];
                float attributeValue = attribute.value;
                if(attributeValue < minimum)
                {
                    minimum = attributeValue;
                }
            }
            return minimum;
        }

        public static float CalculateZeroBoundMin(IList<InformationObject> os, int attributeID)
        {
            float min = CalculateMin(os, attributeID);
            min = (min > 0) ? 0f : min;
            return min;
        }

        public static float CalculateMax(IList<InformationObject> os, int attributeID)
        {
            float max = float.MinValue;
            foreach(InformationObject dataObject in os)
            {
                var attribute = dataObject.attributesFloat[attributeID];
                float attributeValue = attribute.value;
                if(attributeValue > max)
                {
                    max = attributeValue;
                }
            }
            return max;
        }

        public static float CalculateZeroBoundMax(IList<InformationObject> os, int attributeID)
        {
            float max = CalculateMax(os, attributeID);
            max = (max < 0) ? 0f : max;
            return max;
        }

        /**
         * Calculates the range from minimum to maximum in the available attribute.
         * This is needed to scale the bars appropriately.
         * */
        public static float CalculateRange(IList<InformationObject> os, int aID)
        {
            float Maximum = CalculateMax(os, aID);
            float Minimum = CalculateMin(os, aID);

            return Maximum - Minimum;
        }

        public static float CalculateZeroBoundRange(IList<InformationObject> os, int aID)
        {
            float Maximum = CalculateZeroBoundMax(os, aID);
            float Minimum = CalculateZeroBoundMin(os, aID);

            return Maximum - Minimum;
        }
    }
    
}
