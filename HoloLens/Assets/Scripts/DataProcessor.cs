using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Calculates various stats. Keep in mind that missing values are coded as
/// float.NaN for ratio scaled attributes and int.MinValue for ordinal and
/// interval scaled values. Nominal values are marked with "missingValue".
/// </summary>
public class AttributeProcessor
{
    public static class Categorial
    {
        public static int CountObjectsMatchingTwoCategoriesNomOrd(IList<InfoObject> os, string a1, string a2, string v1, int v2)
        {
            int counter = 0;

            try
            {
                foreach(var o in os)
                {
                    var vNom = o.GetNomValue(a1);
                    var vOrd = o.GetOrdValue(a2);

                    if(vNom.Equals(v1) && vOrd == v2)
                    {
                        counter++;
                    }
                }
            }
            catch(Exception e)
            {
                Debug.LogWarning("Attributes must be Nominal and Ordinal" + e.Message);
            }

            return counter;
        }
    }

    public static class Nominal
    {
        public static NominalAttributeStats CalculateStats(IList<InfoObject> os, int aID)
        {
            var measures = new NominalAttributeStats(
                CalculateDistribution(os, aID),
                os[0].nomAttribVals[aID].name);

            return measures;
        }

        public static string[] FindUniqueValues(IList<InfoObject> os, int aID)
        {
            var uniqueList = new List<string>();

            foreach(var o in os)
            {
                var a = o.nomAttribVals[aID];
                if(!a.value.Equals("missingValue"))
                {
                    if(!uniqueList.Contains(a.value))
                    {
                        uniqueList.Add(a.value);
                    }
                }
            }

            return uniqueList.ToArray();
        }

        public static IDictionary<string, int> CalculateDistribution(IList<InfoObject> os, int aID)
        {
            var distribution = new Dictionary<string, int>();

            string[] keys = FindUniqueValues(os, aID);

            foreach(var key in keys)
            {
                distribution.Add(key, 0);
            }

            foreach(var o in os)
            {
                var a = o.nomAttribVals[aID];
                if(!a.value.Equals("missingValue"))
                {
                    distribution[a.value] = distribution[a.value] + 1;
                }
            }

            return distribution;
        }

        public static int CountObjectsMatchingTwoCattegories(IList<InfoObject> os, int aID1, int aID2, string value1, string value2)
        {
            int counter = 0;

            foreach(var o in os)
            {
                var v1 = o.nomAttribVals[aID1].value;
                var v2 = o.nomAttribVals[aID2].value;
                
                if(v1.Equals(value1) && v2.Equals(value2))
                {
                    counter++;
                }
            }
            return counter;
        }
    }

    public static class Ordinal
    {
        public static OrdinalAttributeStats CalculateStats(IList<InfoObject> os, int aID, IDictionary<int, string> dict)
        {
            var measures = new OrdinalAttributeStats(
                dict,
                CalculateDistribution(os, aID, dict),
                os[0].ordAttribVals[aID].name);

            return measures;
        }

        public static IDictionary<int, int> CalculateDistribution(IList<InfoObject> os, int aID, IDictionary<int, string> dict)
        {
            var distribution = new Dictionary<int, int>();

            int[] keys = dict.Keys.ToArray();

            foreach(var key in keys)
            {
                distribution.Add(key, 0);
            }

            foreach(var o in os)
            {
                var a = o.ordAttribVals[aID];
                if(a.value != int.MinValue)
                {
                    distribution[a.value] = distribution[a.value] + 1;
                }
            }

            return distribution;
        }

        public static int CountObjectsMatchingTwoCattegories(IList<InfoObject> os, int aID1, int aID2, int value1, int value2)
        {
            int counter = 0;

            foreach(var o in os)
            {
                var v1 = o.ordAttribVals[aID1].value;
                var v2 = o.ordAttribVals[aID2].value;

                if(v1 != int.MinValue && v2 != int.MinValue)
                {
                    if(v1 == value1 && v2 == value2)
                    {
                        counter++;
                    }
                }
            }
            return counter;
        }

        public static int CalculateMin(IList<InfoObject> os, int attributeID)
        {
            int minimum = int.MaxValue;
            foreach(InfoObject dataObject in os)
            {
                var a = dataObject.ordAttribVals[attributeID];
                if(a.value != int.MinValue)
                {
                    if(a.value < minimum)
                    {
                        minimum = a.value;
                    }
                }
            }
            return minimum;
        }

        public static int CalculateMax(IList<InfoObject> os, int attributeID)
        {
            int maximum = int.MinValue;
            foreach(InfoObject dataObject in os)
            {
                var a = dataObject.ordAttribVals[attributeID];
                if(a.value != int.MinValue)
                {
                    if(a.value > maximum)
                    {
                        maximum = a.value;
                    }
                }
            }
            return maximum;
        }
    }

    public static class Interval
    {
        public static IntervalAttributeStats CalculateStats(IList<InfoObject> os, int aID, IDictionary<string, string> intervalTranslators)
        {
            var measures = new IntervalAttributeStats(
                CalculateDistribution(os, aID),
                os[0].ivlAttribVals[aID].name,
                intervalTranslators[os[0].ivlAttribVals[aID].name],
                CalculateMin(os, aID),
                CalculateMax(os, aID)
                );

            return measures;
        }

        public static IDictionary<int, int> CalculateDistribution(IList<InfoObject> os, int aID)
        {
            var distribution = new Dictionary<int, int>();

            foreach(var o in os)
            {
                var a = o.ivlAttribVals[aID];
                if(a.value != int.MinValue)
                {
                    if(!distribution.ContainsKey(a.value))
                    {
                        distribution.Add(a.value, 0);
                    }
                    distribution[a.value] = distribution[a.value] + 1;
                }
            }

            return distribution;
        }

        public static int CalculateMin(IList<InfoObject> os, int attributeID)
        {
            int minimum = int.MaxValue;
            foreach(InfoObject dataObject in os)
            {
                var a = dataObject.ivlAttribVals[attributeID];
                if(a.value != int.MinValue)
                {
                    if(a.value < minimum)
                    {
                        minimum = a.value;
                    }
                }
            }
            return minimum;
        }

        public static int CalculateMax(IList<InfoObject> os, int attributeID)
        {
            int maximum = int.MinValue;
            foreach(InfoObject dataObject in os)
            {
                var a = dataObject.ivlAttribVals[attributeID];
                if(a.value != int.MinValue)
                {
                    if(a.value > maximum)
                    {
                        maximum = a.value;
                    }
                }
            }
            return maximum;
        }
    }
    
    public static class Ratio
    {
        public static RatioAttributeStats CalculateStats(IList<InfoObject> os, int aID)
        {
            var measures = new RatioAttributeStats(
                os[0].ratAttribVals[aID].name,
                CalculateRange(os, aID),
                CalculateZeroBoundRange(os, aID),
                CalculateMin(os, aID),
                CalculateZeroBoundMin(os, aID),
                CalculateMax(os, aID),
                CalculateZeroBoundMax(os, aID)
                );

            return measures;
        }

        public static float CalculateMin(IList<InfoObject> os, int attributeID)
        {
            float minimum = float.MaxValue;
            foreach(InfoObject dataObject in os)
            {
                var a = dataObject.ratAttribVals[attributeID];
                if(!float.IsNaN(a.value))
                {
                    if(a.value < minimum)
                    {
                        minimum = a.value;
                    }
                }
            }
            return minimum;
        }

        public static float CalculateZeroBoundMin(IList<InfoObject> os, int attributeID)
        {
            float min = CalculateMin(os, attributeID);
            min = (min > 0) ? 0f : min;
            return min;
        }

        public static float CalculateMax(IList<InfoObject> os, int attributeID)
        {
            float maximum = float.MinValue;
            foreach(InfoObject dataObject in os)
            {
                var a = dataObject.ratAttribVals[attributeID];
                if(!float.IsNaN(a.value))
                {
                    if(a.value > maximum)
                    {
                        maximum = a.value;
                    }
                }
            }
            return maximum;
        }

        public static float CalculateZeroBoundMax(IList<InfoObject> os, int attributeID)
        {
            float max = CalculateMax(os, attributeID);
            max = (max < 0) ? 0f : max;
            return max;
        }

        /**
         * Calculates the range from minimum to maximum in the available attribute.
         * This is needed to scale the bars appropriately.
         * */
        public static float CalculateRange(IList<InfoObject> os, int aID)
        {
            float Maximum = CalculateMax(os, aID);
            float Minimum = CalculateMin(os, aID);

            return Maximum - Minimum;
        }

        public static float CalculateZeroBoundRange(IList<InfoObject> os, int aID)
        {
            float Maximum = CalculateZeroBoundMax(os, aID);
            float Minimum = CalculateZeroBoundMin(os, aID);

            return Maximum - Minimum;
        }
    }

    public static void ExtractAttributeIDs(DataSet data, string[] attIDs, out int[] nomIDs, out int[] ordIDs, out int[] ivlIDs, out int[] ratIDs)
    {
        var nomIDsL = new List<int>();
        var ordIDsL = new List<int>();
        var ivlIDsL = new List<int>();
        var ratIDsL = new List<int>();

        foreach(string key in attIDs)
        {
            var type = data.GetTypeOf(key);
            switch(type)
            {
                case LoM.NOMINAL:
                    nomIDsL.Add(data.GetIDOf(key));
                    break;
                case LoM.ORDINAL:
                    ordIDsL.Add(data.GetIDOf(key));
                    break;
                case LoM.INTERVAL:
                    ivlIDsL.Add(data.GetIDOf(key));
                    break;
                case LoM.RATIO:
                    ratIDsL.Add(data.GetIDOf(key));
                    break;
                default:
                    break;
            }
        }

        nomIDs = nomIDsL.ToArray();
        ordIDs = ordIDsL.ToArray();
        ivlIDs = ivlIDsL.ToArray();
        ratIDs = ratIDsL.ToArray();
    }

}
