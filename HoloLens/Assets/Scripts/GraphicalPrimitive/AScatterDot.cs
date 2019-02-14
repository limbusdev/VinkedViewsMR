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
    public class AScatterDot : AGraphicalPrimitive
    {
        public MeshRenderer[] renderers;
        public GameObject dot;
        private Vector3 unhighlightedScale;

        private void Awake()
        {
            unhighlightedScale = dot.transform.localScale;
        }

        protected override void ApplyColor(Color color)
        {
            foreach(var rend in renderers)
                rend.material.color = color;
        }

        

        public override void Highlight()
        {
            base.Highlight();
            dot.transform.localScale = unhighlightedScale * 3f;
        }

        public override void Unhighlight()
        {
            base.Unhighlight();
            dot.transform.localScale = unhighlightedScale;
        }

        public override void Unbrush()
        {
            base.Unbrush();
            dot.transform.localScale = unhighlightedScale;
        }
    }
}