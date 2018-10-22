using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphicalPrimitive
{
    public class ScatterDot3D : AGraphicalPrimitive
    {
        public MeshRenderer[] renderers;
        public GameObject dot;

        public override void ApplyColor(Color color)
        {
            foreach(var rend in renderers)
                rend.material.color = color;
        }

        private Vector3 unhighlightedScale;

        public override void Highlight()
        {
            base.Highlight();
            unhighlightedScale = dot.transform.localScale;
            dot.transform.localScale = unhighlightedScale * 3f;
        }

        public override void Unhighlight()
        {
            base.Unhighlight();
            dot.transform.localScale = unhighlightedScale;
        }
    }
}


