/*
Vinked Views
Copyright(C) 2018  Georg Eckert

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    public class DataSet
    {
        public string title { get; set; }
        public int ID { get; }

        public IList<InfoObject> infoObjects { get; set; }
        public IDictionary<string, NominalAttributeStats> nominalStatistics { get; set; }
        public IDictionary<string, OrdinalAttributeStats> ordinalStatistics { get; set; }
        public IDictionary<string, IntervalAttributeStats> intervalStatistics { get; set; }
        public IDictionary<string, RatioAttributeStats> rationalStatistics { get; set; }

        // Lookup Tables
        public IDictionary<string, int> attIDbyNAME { get; set; }
        public IDictionary<string, LoM> attLOMbyNAME;

        public IDictionary<string, int> nominalAttributeIDsByName;
        public IDictionary<string, int> ordinalAttributeIDsByName;
        public IDictionary<string, int> intervalAttributeIDsByName;
        public IDictionary<string, int> rationalAttributeIDsByName;

        public string[] nomAttribNames { get; set; }
        public string[] ordAttribNames { get; set; }
        public string[] ivlAttribNames { get; set; }
        public string[] ratAttribNames { get; set; }

        public IDictionary<string, IDictionary<int, string>> dictionaries;
        public IDictionary<string, string> intervalTranslators;
        public IDictionary<InfoObject, Color> colorTable;
        public IDictionary<InfoObject, Color> colorTableBrushing;

        public DataSet(
            string title,
            int ID,
            IDictionary<string, int> nominalAttributeIDsByName,
            IDictionary<string, int> ordinalAttributeIDsByName,
            IDictionary<string, int> intervalAttributeIDsByName,
            IDictionary<string, int> rationalAttributeIDsByName,
            IDictionary<string, int> attributeIDbyName,
            IDictionary<string, LoM> attributeLoMbyName,
            IList<InfoObject> infoObjects, 
            IDictionary<string, IDictionary<int, string>> dictionaries, 
            IDictionary<string, string> intervalTranslators
            )
        {
            this.ID = ID;
            this.title = title;
            this.infoObjects = infoObjects;

            this.dictionaries = dictionaries;
            this.intervalTranslators = intervalTranslators;

            this.nominalAttributeIDsByName = nominalAttributeIDsByName;
            this.ordinalAttributeIDsByName = ordinalAttributeIDsByName;
            this.intervalAttributeIDsByName = intervalAttributeIDsByName;
            this.rationalAttributeIDsByName = rationalAttributeIDsByName;
            this.attIDbyNAME = attributeIDbyName;
            this.attLOMbyNAME = attributeLoMbyName;

            this.nominalStatistics = new Dictionary<string, NominalAttributeStats>();
            this.ordinalStatistics = new Dictionary<string, OrdinalAttributeStats>();
            this.intervalStatistics = new Dictionary<string, IntervalAttributeStats>();
            this.rationalStatistics = new Dictionary<string, RatioAttributeStats>();



            // CALCULATE
            this.nomAttribNames = new string[nominalAttributeIDsByName.Count];
            this.ordAttribNames = new string[ordinalAttributeIDsByName.Count];
            this.ivlAttribNames = new string[intervalAttributeIDsByName.Count];
            this.ratAttribNames = new string[rationalAttributeIDsByName.Count];

            // Calculate Data Measures for nominal attributes
            foreach(var key in nominalAttributeIDsByName.Keys)
            {
                var measure = AttributeProcessor.Nominal.CalculateStats(
                    infoObjects, nominalAttributeIDsByName[key]);
                nominalStatistics.Add(key, measure);
                nomAttribNames[nominalAttributeIDsByName[key]] = key;
            }

            // Calculate Data Measures for ordinal attributes
            foreach(var key in ordinalAttributeIDsByName.Keys)
            {
                var measure = AttributeProcessor.Ordinal.CalculateStats(
                    infoObjects, ordinalAttributeIDsByName[key], dictionaries[key]);
                ordinalStatistics.Add(key, measure);
                ordAttribNames[ordinalAttributeIDsByName[key]] = key;
            }

            // Calculate Data Measures for interval Attributes
            foreach(var key in intervalAttributeIDsByName.Keys)
            {
                var measure = AttributeProcessor.Interval.CalculateStats(
                    infoObjects, intervalAttributeIDsByName[key], intervalTranslators);
                intervalStatistics.Add(key, measure);
                ivlAttribNames[intervalAttributeIDsByName[key]] = key;
            }

            // Calculate Data Measures for ratio Attributes
            foreach(var key in rationalAttributeIDsByName.Keys)
            {
                var measure = AttributeProcessor.Ratio.CalculateStats(
                    infoObjects, rationalAttributeIDsByName[key]);
                rationalStatistics.Add(key, measure);
                ratAttribNames[rationalAttributeIDsByName[key]] = key;
            }

            GenerateLookupTables();

            foreach(var o in infoObjects)
            {
                o.Init(this);
            }
        }

       
        public void GenerateLookupTables()
        {
            // Color table
            colorTable = new Dictionary<InfoObject, Color>();
            colorTableBrushing = new Dictionary<InfoObject, Color>();

            int counter = 0;
            foreach(var o in infoObjects)
            {
                // Primitive colors between blue and red
                colorTable.Add(o, Color.HSVToRGB((((float)counter) / infoObjects.Count) / 2f + .5f, 1, 1));

                // brush colors
                colorTableBrushing.Add(o, Color.HSVToRGB(((float)counter) / infoObjects.Count /3f + .1f, 1, 1));

                counter++;
            }

            
        }

        public LoM TypeOf(string varName)
        {
            return attLOMbyNAME[varName];
        }

        public int IDOf(string varName)
        {
            return attIDbyNAME[varName];
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

        public bool IsValueMissing(InfoObject infO, string attributeName)
        {
            if(float.IsNaN(ValueOf(infO, attributeName)))
            {
                return true;
            } else
            {
                return false;
            }
        }   

        /// <summary>
        /// Checks whether the given information object has values for
        /// each dimension. If not, it is considered incomplete and not
        /// fit for visualization.
        /// </summary>
        /// <param name="o">Object to test on completeness.</param>
        /// <param name="attIDs">Attributes to check for completeness.</param>
        /// <returns>if information object is complete</returns>
        public bool IsInfoObjectCompleteRegarding(InfoObject o, int[] nomIDs, int[] ordIDs, int[] ivlIDs, int[] ratIDs)
        {
            bool missing = false;
            foreach(var id in nomIDs)
            {
                missing |= IsValueMissing(o, nomAttribNames[id]);
            }
            foreach(var id in ordIDs)
            {
                missing |= IsValueMissing(o, ordAttribNames[id]);
            }
            foreach(var id in ivlIDs)
            {
                missing |= IsValueMissing(o, ivlAttribNames[id]);
            }
            foreach(var id in ratIDs)
            {
                missing |= IsValueMissing(o, ratAttribNames[id]);
            }

            return missing;
        }

        public float ValueOf(InfoObject infO, string attributeName)
        {
            float val;
            var lom = TypeOf(attributeName);

            switch(lom)
            {
                case LoM.NOMINAL:
                    var mN = nominalStatistics[attributeName];
                    if(infO.NomValueOf(attributeName).Equals("missingValue"))
                        val = float.NaN;
                    else
                        val = mN.valueIDs[infO.NomValueOf(attributeName)];
                    break;
                case LoM.ORDINAL:
                    int valO = infO.OrdValueOf(attributeName);
                    if(valO == int.MinValue)
                        val = float.NaN;
                    else
                        val = valO;
                    break;
                case LoM.INTERVAL:
                    int valI = infO.IvlValueOf(attributeName);
                    if(valI == int.MinValue)
                        val = float.NaN;
                    else
                        val = valI;
                    break;
                default: /* RATIO */
                    val = infO.RatValueOf(attributeName);
                    break;
            }

            return val;
        }
    }
}
