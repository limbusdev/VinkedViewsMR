using Model;
using Model.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataProcessor
{
    public static class FloatAttribute
    {
        public static DataDimensionMeasures CalculateMeasures(IList<InformationObject> os, int aID)
        {
            var measures = new DataDimensionMeasures(
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
            float minimum = 0;
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
            float max = 0;
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
