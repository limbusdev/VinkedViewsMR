using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GraphicalPrimitive
{
    public abstract class ABar : AGraphicalPrimitive
    {
        public GameObject bar;

        public abstract void SetSize(float width, float height, float depth);

        public void SetLabelText(string newText)
        {
            label.GetComponent<TextMesh>().text = newText;
        }
    }
}
