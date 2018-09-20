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
    public class GenericAttribute<T>
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
        public GenericAttribute(string name, T value, LoM lom)
        {
            this.value = value;
            this.name = name;
            this.lom = lom;
        }
    }
}
