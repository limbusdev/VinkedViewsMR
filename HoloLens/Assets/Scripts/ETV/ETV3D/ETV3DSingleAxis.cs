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
using GraphicalPrimitive;
using Model;
using UnityEngine;

namespace ETV
{
    public class ETV3DSingleAxis : AETVSingleAxis
    {
        // .................................................................... Private Variables
        private GameObject axis;
        private string attributeName;


        // .................................................................... AETV3D Methods
        public override void Init(DataSet data, string attributeName, bool isMetaVis = false)
        {
            base.Init(data, isMetaVis);
            this.attributeName = attributeName;

            SetUpAxes();

            SetAxisLabels(AxisDirection.Y, attributeName);
        }
        

        public override void SetUpAxes()
        {
            AddAxis(attributeName, AxisDirection.Y);
        }

        public override void UpdateETV()
        {
            // Nothing yet
        }

        public override void DrawGraph()
        {
            // Nothing yet
        }


        public override AGraphicalPrimitiveFactory AxisFactory()
        {
            return Services.PrimFactory3D();
        }

    }
}