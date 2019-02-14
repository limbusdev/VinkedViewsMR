/*
Copyright 2019 Georg Eckert (MIT License)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to
deal in the Software without restriction, including without limitation the
rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
sell copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
IN THE SOFTWARE.
*/
using UnityEngine;

namespace GraphicalPrimitive
{
    public class Bar2D : ABar
    {
        public GameObject barFront;
        public GameObject barBack;

        
        protected override void ApplyColor(Color color)
        {
            Renderer rend = barBack.GetComponent<Renderer>();
            rend.material.color = color;

            rend = barFront.GetComponent<Renderer>();
            rend.material.color = color;
        }
       
        override public void SetSize(float width, float height, float depth=1)
        {
            base.SetSize(width, height, 1);
            bar.transform.localScale = new Vector3(width, height, 1);
            var textMesh = label.GetComponent<TextMesh>();
            textMesh.anchor = (height < 0) ? TextAnchor.UpperCenter : TextAnchor.LowerCenter;
            label.transform.localPosition = new Vector3(0, height, 0);
        }
    }
}
