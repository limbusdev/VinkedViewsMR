using Model.Attributes;
using System.Collections.Generic;

namespace Model
{
    public class DataSet
    {
        public string Title { get; set; }

        public IList<InfoObject> informationObjects { get; set; }
        public IDictionary<string, NominalDataDimensionMeasures> dataMeasuresNominal { get; set; }
        public IDictionary<string, OrdinalDataDimensionMeasures> dataMeasuresOrdinal { get; set; }
        public IDictionary<string, IntervalDataDimensionMeasures> dataMeasuresInterval { get; set; }
        public IDictionary<string, RatioDataDimensionMeasures> dataMeasuresRatio { get; set; }

        public IDictionary<string, int> attributeIDs { get; set; }

        public string[] nomAttributes { get; set; }
        public string[] ordAttributes { get; set; }
        public string[] ivlAttributes { get; set; }
        public string[] ratAttributes { get; set; }
        public string[] ratioQuadAttributes { get; set; }
        public string[] ratioCubeAttributes { get; set; }

        public IDictionary<string, IDictionary<int, string>> dicts;
        public IDictionary<string, string> intervalTranslators;

        public DataSet(string title, IList<InfoObject> dataObjects, IDictionary<string, IDictionary<int, string>> dicts, IDictionary<string, string> intervalTranslators)
        {
            Title = title;
            this.informationObjects = dataObjects;

            this.dicts = dicts;
            this.intervalTranslators = intervalTranslators;

            this.dataMeasuresNominal = new Dictionary<string, NominalDataDimensionMeasures>();
            this.dataMeasuresOrdinal = new Dictionary<string, OrdinalDataDimensionMeasures>();
            this.dataMeasuresInterval = new Dictionary<string, IntervalDataDimensionMeasures>();
            this.dataMeasuresRatio = new Dictionary<string, RatioDataDimensionMeasures>();

            this.attributeIDs = new Dictionary<string, int>();

            // CALCULATE

            // Calculate Data Measures for nominal attributes
            int nominalCounter = 0;
            foreach(GenericAttribute<string> attribute in dataObjects[0].nominalAtt)
            {
                NominalDataDimensionMeasures measure;
                measure = DataProcessor.NominalAttribute.CalculateMeasures(dataObjects, nominalCounter);
                dataMeasuresNominal.Add(attribute.name, measure);
                nominalCounter++;
            }

            // Calculate Data Measures for ordinal attributes
            int ordinalCounter = 0;
            foreach(GenericAttribute<int> attribute in dataObjects[0].ordinalAtt)
            {
                OrdinalDataDimensionMeasures measure;
                measure = DataProcessor.OrdinalAttribute.CalculateMeasures(dataObjects, ordinalCounter, dicts[attribute.name]);
                dataMeasuresOrdinal.Add(attribute.name, measure);
                nominalCounter++;
            }

            // Calculate Data Measures for interval Attributes
            int intervalCounter = 0;
            foreach(GenericAttribute<int> attribute in dataObjects[0].intervalAtt)
            {
                IntervalDataDimensionMeasures measure;
                measure = DataProcessor.IntervalAttribute.CalculateMeasures(dataObjects, intervalCounter, intervalTranslators);
                dataMeasuresInterval.Add(attribute.name, measure);
                intervalCounter++;
            }

            // Calculate Data Measures for ratio Attributes
            int ratioCounter = 0;
            foreach(GenericAttribute<float> attribute in dataObjects[0].ratioAtt)
            {
                RatioDataDimensionMeasures measure;
                measure = DataProcessor.RatioAttribute.CalculateMeasures(dataObjects, ratioCounter);
                dataMeasuresRatio.Add(attribute.name, measure);
                ratioCounter++;
            }

            // Fill variable names
            InfoObject infoObj = dataObjects[0];

            nomAttributes = new string[infoObj.nominalAtt.Length];
            for(int i = 0; i < dataObjects[0].nominalAtt.Length; i++)
            {
                nomAttributes[i] = infoObj.nominalAtt[i].name;
                attributeIDs.Add(nomAttributes[i], i);
            }

            ordAttributes = new string[infoObj.ordinalAtt.Length];
            for(int i = 0; i < dataObjects[0].ordinalAtt.Length; i++)
            {
                ordAttributes[i] = infoObj.ordinalAtt[i].name;
                attributeIDs.Add(ordAttributes[i], i);
            }

            ivlAttributes = new string[infoObj.intervalAtt.Length];
            for(int i = 0; i < dataObjects[0].intervalAtt.Length; i++)
            {
                ivlAttributes[i] = infoObj.intervalAtt[i].name;
                attributeIDs.Add(ivlAttributes[i], i);
            }

            ratAttributes = new string[infoObj.ratioAtt.Length];
            for(int i = 0; i < dataObjects[0].ratioAtt.Length; i++)
            {
                ratAttributes[i] = infoObj.ratioAtt[i].name;
                attributeIDs.Add(ratAttributes[i], i);
            }





            ratioQuadAttributes = new string[infoObj.attributesVector2.Length];
            for(int i = 0; i < dataObjects[0].attributesVector2.Length; i++)
                ratioQuadAttributes[i] = infoObj.attributesVector2[i].name;

            ratioCubeAttributes = new string[infoObj.attributesVector3.Length];
            for(int i = 0; i < dataObjects[0].attributesVector3.Length; i++)
                ratioCubeAttributes[i] = infoObj.attributesVector3[i].name;

        }

        public LoM GetTypeOf(string varName)
        {
            if(dataMeasuresNominal.ContainsKey(varName))
                return LoM.NOMINAL;
            if(dataMeasuresOrdinal.ContainsKey(varName))
                return LoM.ORDINAL;
            if(dataMeasuresInterval.ContainsKey(varName))
                return LoM.INTERVAL;
            else
                return LoM.RATIO;

        }

        public int GetIDOf(string varName)
        {
            return attributeIDs[varName];
        }

        public override string ToString()
        {
            string outString = "";

            outString += "DataSet: " + Title + "\n\n";

            foreach(InfoObject o in informationObjects)
            {
                outString += o.ToString() + "\n";
            }

            return outString;
        }

        public float GetValue(InfoObject infO, string attributeName, LoM lom)
        {
            float val;
            switch(lom)
            {
                case LoM.NOMINAL:
                    var mN = dataMeasuresNominal[attributeName];
                    int valN = mN.valueIDs[infO.GetNomValue(attributeName)];
                    if(valN == int.MinValue)
                        val = float.NaN;
                    else
                        val = valN;
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
