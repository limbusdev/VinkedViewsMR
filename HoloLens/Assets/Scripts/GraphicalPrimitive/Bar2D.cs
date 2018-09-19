using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GraphicalPrimitive
{
    public class Bar2D : ABar
    {
        public GameObject barFront;
        public GameObject barBack;

        public override void ApplyColor(Color color)
        {
            Renderer rend = barBack.GetComponent<Renderer>();
            rend.material = new Material(Shader.Find("Specular"));
            rend.material.color = color;

            rend = barFront.GetComponent<Renderer>();
            rend.material = new Material(Shader.Find("Specular"));
            rend.material.color = color;
        }
       
        override public void SetSize(float width, float height, float depth=1)
        {
            bar.transform.localScale = new Vector3(width, height, 1);
            var textMesh = label.GetComponent<TextMesh>();
            textMesh.anchor = (height < 0) ? TextAnchor.UpperCenter : TextAnchor.LowerCenter;
            label.transform.localPosition = new Vector3(0, height, 0);
        }
    }
}
