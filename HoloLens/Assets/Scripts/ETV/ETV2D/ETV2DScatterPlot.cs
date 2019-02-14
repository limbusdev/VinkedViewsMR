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
    public class ETV2DScatterPlot : AETVScatterPlot
    {
        private string attributeA, attributeB;

        public override void Init(DataSet data, string[] attributes, bool isMetaVis = false)
        {
            base.Init(data, isMetaVis);
            attributeA = attributes[0];
            attributeB = attributes[1];


            SetUpAxes();
            DrawGraph();
        }

        public override void SetUpAxes()
        {
            AddAxis(attributeA, AxisDirection.X);
            AddAxis(attributeB, AxisDirection.Y);

            var secondXAxis = AddAxis(attributeA, AxisDirection.X);
            var secondYAxis = AddAxis(attributeB, AxisDirection.Y);

            secondXAxis.transform.localPosition = new Vector3(0, 1, 0);
            secondYAxis.transform.localPosition = new Vector3(1, 0, 0);
            
            secondXAxis.transform.localRotation = Quaternion.Euler(180, 0, 0);
            secondYAxis.transform.localRotation = Quaternion.Euler(0, 180, 0);

            secondXAxis.GetComponent<AAxis>().Clean();
            secondYAxis.GetComponent<AAxis>().Clean();
        }

        public override void DrawGraph()
        {
            foreach(var infO in Data.infoObjects)
            {
                float valA = Data.ValueOf(infO, attributeA);
                float valB = Data.ValueOf(infO, attributeB);


                if(!float.IsNaN(valA) && !float.IsNaN(valB))
                {
                    var pos = new Vector3(
                        GetAxis(AxisDirection.X).TransformToAxisSpace(valA),
                        GetAxis(AxisDirection.Y).TransformToAxisSpace(valB),
                        0
                        );

                    var dot = Services.PrimFactory2D().CreateScatterDot();

                    dot.SetColor(Data.colorTable[infO], Data.colorTableBrushing[infO]);
                    dot.transform.position = pos;
                    dot.transform.parent = Anchor.transform;
                    
                    RememberRelationOf(infO, dot);
                }
            }
        }

        public override void UpdateETV()
        {

        }
    }


}



