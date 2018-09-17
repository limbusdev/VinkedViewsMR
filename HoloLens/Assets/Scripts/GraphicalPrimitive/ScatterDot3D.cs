using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphicalPrimitive
{
    public class ScatterDot3D : AGraphicalPrimitive
    {
        public MeshRenderer[] renderers;

        public override void ApplyColor(Color color)
        {
            foreach(var rend in renderers)
                rend.material.color = color;
        }
    }
}


