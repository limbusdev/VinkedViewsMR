using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GraphicalPrimitive
{
    class Bar3D : ABar
    {
        public GameObject geometry;

        public override void ChangeColor(Color newColor)
        {
            geometry.GetComponent<Renderer>().material.color = newColor;
        }

        public override void SetSize(float width, float height, float depth)
        {
            bar.transform.localScale = new Vector3(width, height, depth);
            var textMesh = label.GetComponent<TextMesh>();
            textMesh.anchor = (height < 0) ? TextAnchor.UpperCenter : TextAnchor.LowerCenter;
            label.transform.localPosition = new Vector3(0, height, 0);
            labelCategory.transform.localPosition = new Vector3(0, 0, -depth / 2);
        }
        
    }
}
