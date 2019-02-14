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
    public abstract class ABar : AGraphicalPrimitive
    {
        public GameObject bar;
        private Vector3 unhighlightedScale;
        

        public virtual void SetSize(float width, float height, float depth)
        {
            unhighlightedScale = new Vector3(width, height, depth);
        }

        public void SetLabelText(string newText)
        {
            label.GetComponent<TextMesh>().text = newText;
        }
        

        public override void Highlight()
        {
            base.Highlight();
            bar.transform.localScale = unhighlightedScale * 1.2f;
        }

        public override void Unhighlight()
        {
            base.Unhighlight();
            bar.transform.localScale = unhighlightedScale;
        }

        public override void Unbrush()
        {
            base.Unbrush();
            bar.transform.localScale = unhighlightedScale;
        }
    }
}
