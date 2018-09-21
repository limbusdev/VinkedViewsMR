using Model.Attributes;
using System.Collections.Generic;
using UnityEngine;

namespace Model
{
    /// <summary>
    /// Level of Measurement
    /// 
    /// Nominal: Dresden, Berlin, Hamburg, ...
    /// Ordinal: very small, small, medium, big, very big
    /// Interval: Monday, Tuesday, Wednesday, Thursday, ...
    /// Ratio: 0.01, 0.02, 0.035, 0.24441, ...
    /// </summary>
    public enum LoM
    {
        NOMINAL, ORDINAL, INTERVAL, RATIO
    }

    /// <summary>
    /// Instances of this class represent an information object. An information
    /// object can be a measurement containing several values, connected to this
    /// measurement. 
    /// E.g. 
    ///     {
    ///         "Humidity": .9,
    ///         "Temperature [K]": 304.2,
    ///         "x": 1.0,
    ///         "y": 1.0,
    ///         "z": 1.0,
    ///         "Pressure [Pa]": 1001.3 
    ///     }
    ///         
    /// Could be an information object describing the current state of the air
    /// in a room at position (1,1,1).
    /// </summary>
    public class InfoObject
    {
        // .................................................................... Lookup tables
        public IDictionary<string, int> attributeIDsByName;
        public IDictionary<string, LoM> attributeLoMs;

        // .................................................................... Attribute values
        public Attribute<string>[] nomAttribVals;
        public Attribute<int>[] ordAttribVals;
        public Attribute<int>[] ivlAttribVals;
        public Attribute<float>[] ratAttribVals;

        // .................................................................... Observed representatives
        public IDictionary<string, IList<GameObject>> 
            representativeGameObjectsByAttributeName { get; }


        // .................................................................... Constructor
        public InfoObject(
            Attribute<string>[] attributesNom,
            Attribute<int>[] attributesOrd,
            Attribute<int>[] attributesIvl,
            Attribute<float>[] attributesRat)
        {
            this.representativeGameObjectsByAttributeName 
                = new Dictionary<string, IList<GameObject>>();
            this.nomAttribVals = attributesNom;
            this.ordAttribVals = attributesOrd;
            this.ivlAttribVals = attributesIvl;
            this.ratAttribVals = attributesRat;

            GenerateLookupTables();
        }

        // .................................................................... Initializer methods
        private void GenerateLookupTables()
        {
            attributeIDsByName = new Dictionary<string, int>();
            attributeLoMs = new Dictionary<string, LoM>();

            for(int i = 0; i < nomAttribVals.Length; i++)
            {
                var a = nomAttribVals[i];
                attributeIDsByName.Add(a.name, i);
                attributeLoMs.Add(a.name, a.lom);
            }

            for(int i = 0; i < ordAttribVals.Length; i++)
            {
                var a = ordAttribVals[i];
                attributeIDsByName.Add(a.name, i);
                attributeLoMs.Add(a.name, a.lom);
            }

            for(int i = 0; i < ivlAttribVals.Length; i++)
            {
                var a = ivlAttribVals[i];
                attributeIDsByName.Add(a.name, i);
                attributeLoMs.Add(a.name, a.lom);
            }

            for(int i = 0; i < ratAttribVals.Length; i++)
            {
                var a = ratAttribVals[i];
                attributeIDsByName.Add(a.name, i);
                attributeLoMs.Add(a.name, a.lom);
            }
        }


        // .................................................................... Methods
        public void AddRepresentativeObject(string attributeName, GameObject o)
        {
            if(!representativeGameObjectsByAttributeName.ContainsKey(attributeName))
            {
                representativeGameObjectsByAttributeName.Add(
                    attributeName, new List<GameObject>());
            }
            representativeGameObjectsByAttributeName[attributeName].Add(o);
        }

        // .................................................................... Getters & Setters

        public string GetNomValue(string attributeName)
        {
            return nomAttribVals[attributeIDsByName[attributeName]].value;
        }

        public int GetOrdValue(string attributeName)
        {
            return ordAttribVals[attributeIDsByName[attributeName]].value;
        }

        public int GetIvlValue(string attributeName)
        {
            return ivlAttribVals[attributeIDsByName[attributeName]].value;
        }

        public float GetRatValue(string attributeName)
        {
            return ratAttribVals[attributeIDsByName[attributeName]].value;
        }
    }
}
