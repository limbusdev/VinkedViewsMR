using ETV;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphicalPrimitive
{
    public class PCPLine2D : AGraphicalPrimitive
    {
        [SerializeField]
        public LineRenderer lineRenderer;

        public override void ApplyColor(Color color)
        {
            if(lineRenderer != null)
            {
                lineRenderer.startColor = color;
                lineRenderer.endColor = color;
            }
        }
    }
}