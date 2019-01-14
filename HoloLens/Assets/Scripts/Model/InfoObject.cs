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
