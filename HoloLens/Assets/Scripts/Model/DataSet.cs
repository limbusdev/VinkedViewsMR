using Model.Attributes;
using System.Collections.Generic;

namespace Model
{
    public class DataSet
    {
        public string title { get; set; }

        public IList<InfoObject> infoObjects { get; set; }
        public IDictionary<string, NominalAttributeStats> nominalAttribStats { get; set; }
        public IDictionary<string, OrdinalAttributeStats> ordinalAttribStats { get; set; }
        public IDictionary<string, IntervalAttributeStats> intervalAttribStats { get; set; }
        public IDictionary<string, RatioAttributeStats> ratioAttribStats { get; set; }

        public IDictionary<string, int> attributeIDsByName { get; set; }

        public string[] nomAttribNames { get; set; }
        public string[] ordAttribNames { get; set; }
        public string[] ivlAttribNames { get; set; }
        public string[] ratAttribNames { get; set; }

        public IDictionary<string, IDictionary<int, string>> dicts;
        public IDictionary<string, string> intervalTranslators;

        public DataSet(string title, IList<InfoObject> dataObjects, IDictionary<string, IDictionary<int, string>> dicts, IDictionary<string, string> intervalTranslators)
        {
            this.title = title;
            this.infoObjects = dataObjects;

            this.dicts = dicts;
            this.intervalTranslators = intervalTranslators;

            this.nominalAttribStats = new Dictionary<string, NominalAttributeStats>();
            this.ordinalAttribStats = new Dictionary<string, OrdinalAttributeStats>();
            this.intervalAttribStats = new Dictionary<string, IntervalAttributeStats>();
            this.ratioAttribStats = new Dictionary<string, RatioAttributeStats>();

            this.attributeIDsByName = new Dictionary<string, int>();

            // CALCULATE

            // Calculate Data Measures for nominal attributes
            int nominalCounter = 0;
            foreach(Attribute<string> attribute in dataObjects[0].nomAttribVals)
            {
                NominalAttributeStats measure;
                measure = AttributeProcessor.Nominal.CalculateStats(dataObjects, nominalCounter);
                nominalAttribStats.Add(attribute.name, measure);
                nominalCounter++;
            }

            // Calculate Data Measures for ordinal attributes
            int ordinalCounter = 0;
            foreach(Attribute<int> attribute in dataObjects[0].ordAttribVals)
            {
                OrdinalAttributeStats measure;
                measure = AttributeProcessor.Ordinal.CalculateStats(dataObjects, ordinalCounter, dicts[attribute.name]);
                ordinalAttribStats.Add(attribute.name, measure);
                nominalCounter++;
            }

            // Calculate Data Measures for interval Attributes
            int intervalCounter = 0;
            foreach(Attribute<int> attribute in dataObjects[0].ivlAttribVals)
            {
                IntervalAttributeStats measure;
                measure = AttributeProcessor.Interval.CalculateStats(dataObjects, intervalCounter, intervalTranslators);
                intervalAttribStats.Add(attribute.name, measure);
                intervalCounter++;
            }

            // Calculate Data Measures for ratio Attributes
            int ratioCounter = 0;
            foreach(Attribute<float> attribute in dataObjects[0].ratAttribVals)
            {
                RatioAttributeStats measure;
                measure = AttributeProcessor.Ratio.CalculateStats(dataObjects, ratioCounter);
                ratioAttribStats.Add(attribute.name, measure);
                ratioCounter++;
            }

            // Fill variable names
            var infoObj = dataObjects[0];

            nomAttribNames = new string[infoObj.nomAttribVals.Length];
            for(int i = 0; i < dataObjects[0].nomAttribVals.Length; i++)
            {
                nomAttribNames[i] = infoObj.nomAttribVals[i].name;
                attributeIDsByName.Add(nomAttribNames[i], i);
            }

            ordAttribNames = new string[infoObj.ordAttribVals.Length];
            for(int i = 0; i < dataObjects[0].ordAttribVals.Length; i++)
            {
                ordAttribNames[i] = infoObj.ordAttribVals[i].name;
                attributeIDsByName.Add(ordAttribNames[i], i);
            }

            ivlAttribNames = new string[infoObj.ivlAttribVals.Length];
            for(int i = 0; i < dataObjects[0].ivlAttribVals.Length; i++)
            {
                ivlAttribNames[i] = infoObj.ivlAttribVals[i].name;
                attributeIDsByName.Add(ivlAttribNames[i], i);
            }

            ratAttribNames = new string[infoObj.ratAttribVals.Length];
            for(int i = 0; i < dataObjects[0].ratAttribVals.Length; i++)
            {
                ratAttribNames[i] = infoObj.ratAttribVals[i].name;
                attributeIDsByName.Add(ratAttribNames[i], i);
            }
        }

        public LoM GetTypeOf(string varName)
        {
            if(nominalAttribStats.ContainsKey(varName))
                return LoM.NOMINAL;
            if(ordinalAttribStats.ContainsKey(varName))
                return LoM.ORDINAL;
            if(intervalAttribStats.ContainsKey(varName))
                return LoM.INTERVAL;
            else
                return LoM.RATIO;

        }

        public int GetIDOf(string varName)
        {
            return attributeIDsByName[varName];
        }

        public override string ToString()
        {
            string outString = "";

            outString += "DataSet: " + title + "\n\n";

            foreach(InfoObject o in infoObjects)
            {
                outString += o.ToString() + "\n";
            }

            return outString;
        }

        public bool IsValueMissing(InfoObject infO, string attributeName, LoM lom)
        {
            if(float.IsNaN(GetValue(infO, attributeName, lom)))
            {
                return true;
            } else
            {
                return false;
            }
        }   

        public float GetValue(InfoObject infO, string attributeName, LoM lom)
        {
            float val;
            switch(lom)
            {
                case LoM.NOMINAL:
                    var mN = nominalAttribStats[attributeName];
                    if(infO.GetNomValue(attributeName).Equals("missingValue"))
                        val = float.NaN;
                    else
                        val = mN.valueIDs[infO.GetNomValue(attributeName)];
                    break;
                case LoM.ORDINAL:
                    int valO = infO.GetOrdValue(attributeName);
                    if(valO == int.MinValue)
                        val = float.NaN;
                    else
                        val = valO;
                    break;
                case LoM.INTERVAL:
                    int valI = infO.GetIvlValue(attributeName);
                    if(valI == int.MinValue)
                        val = float.NaN;
                    else
                        val = valI;
                    break;
                default: /* RATIO */
                    val = infO.GetRatValue(attributeName);
                    break;
            }

            return val;
        }
    }
}
