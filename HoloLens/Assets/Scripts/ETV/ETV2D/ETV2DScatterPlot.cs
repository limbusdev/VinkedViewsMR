/*
Vinked Views
Copyright(C) 2018  Georg Eckert

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
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



