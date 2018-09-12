using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Model.Attributes
{
    public class GenericAttribute<T>
    {
        public string name { get; }
        public LevelOfMeasurement levelOfMeasurement { get; }
        public T value { get; }
        

        public GenericAttribute(string name, T value, LevelOfMeasurement lom)
        {
            this.value = value;
            this.name = name;
            this.levelOfMeasurement = lom;
        }
    }
}
