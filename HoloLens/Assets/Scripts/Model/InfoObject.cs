/*
Copyright 2019 Georg Eckert (MIT License)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to
deal in the Software without restriction, including without limitation the
rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
sell copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
IN THE SOFTWARE.
*/
using Model.Attributes;

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
        public int dataSetID;
        public DataSet dataSet { get; private set; }

        // .................................................................... Attribute values
        public Attribute<string>[] nomVALbyID;
        public Attribute<int>[] ordVALbyID;
        public Attribute<int>[] ivlVALbyID;
        public Attribute<float>[] ratVALbyID;
        

        // .................................................................... Constructor
        public InfoObject(
            Attribute<string>[] attributesNom,
            Attribute<int>[] attributesOrd,
            Attribute<int>[] attributesIvl,
            Attribute<float>[] attributesRat,
            int dataSetID)
        {
            this.dataSetID = dataSetID;
            this.nomVALbyID = attributesNom;
            this.ordVALbyID = attributesOrd;
            this.ivlVALbyID = attributesIvl;
            this.ratVALbyID = attributesRat;
        }

        public void Init(DataSet dataSet)
        {
            if(this.dataSet == null)
            {
                this.dataSet = dataSet;
            }
        }


        // .................................................................... Methods
        

        // .................................................................... Getters & Setters
        public int Name2ID(string attributeName)
        {
            return dataSet.attIDbyNAME[attributeName];
        }
        
        public string NomValueOf(string attName)
        {
            return nomVALbyID[dataSet.attIDbyNAME[attName]].value;
        }

        public int OrdValueOf(string attName)
        {
            return ordVALbyID[dataSet.attIDbyNAME[attName]].value;
        }

        public int IvlValueOf(string attName)
        {
            return ivlVALbyID[dataSet.attIDbyNAME[attName]].value;
        }

        public float RatValueOf(string attName)
        {
            return ratVALbyID[dataSet.attIDbyNAME[attName]].value;
        }
    }
}
