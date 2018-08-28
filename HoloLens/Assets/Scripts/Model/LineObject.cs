using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Model
{
    public class LineObject
    {
        public string nominalValue { get; set; }
        public Vector2[] values { get; set; }

        public LineObject(string nominalValue, Vector2[] values)
        {
            this.nominalValue = nominalValue;
            this.values = values;
        }
    }
}
