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

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Model.Attributes
{
    /// <summary>
    /// A generic attribute holds a value for an attribute of arbitrary
    /// type.
    /// 
    /// E.g. a nominal attribute "Cloth Size" with possible values
    /// {XS, S, M, L, XL, XXL, XXXL, 4XL, 5XL} could be instantiated by
    /// 
    /// var size = new GenericValue&lt;string&gt;("size", "XL", LoM.ORDINAL);
    /// </summary>
    /// <typeparam name="T">data type to use</typeparam>
    public class Attribute<T>
    {
        public string name { get; }
        public LoM lom { get; }
        public T value { get; }
        
        /// <summary>
        /// Instantiates an attribute of the given data type.
        /// </summary>
        /// <param name="name">Name of the attribute</param>
        /// <param name="value">Value of the attribute</param>
        /// <param name="lom">Level of Measurement</param>
        public Attribute(string name, T value, LoM lom)
        {
            this.value = value;
            this.name = name;
            this.lom = lom;
        }
    }
}
