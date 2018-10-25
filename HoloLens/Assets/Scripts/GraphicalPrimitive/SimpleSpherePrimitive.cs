using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphicalPrimitive
{
    public class SimpleSpherePrimitive : AGraphicalPrimitive
    {
        public MeshRenderer rend;

        public override void ApplyColor(Color color)
        {
            rend.material.color = color;
        }
    }
}