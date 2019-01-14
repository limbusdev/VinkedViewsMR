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
using System.Collections.Generic;
using UnityEngine;

namespace ETV
{
    public class ETV2DLineChart : AETVLineChart
    {
        private XYLine2D primitive;
        private string attributeA, attributeB;

        public override void Init(DataSet data, string attributeA, string attributeB, bool isMetaVis = false)
        {
            base.Init(data, isMetaVis);
            this.attributeA = attributeA;
            this.attributeB = attributeB;

            SetUpAxes();
            DrawGraph();
        }

        public override void ChangeColoringScheme(ETVColorSchemes scheme)
        {
            switch(scheme)
            {
                default:
                    Color color = Color.HSVToRGB(.5f, 1, 1);
                    primitive.SetColor(color, Color.green);
                    break;
            }
        }

        public override void UpdateETV()
        {

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
        }

        public override void DrawGraph()
        {
            var line = Services.instance.Factory2DPrimitives.CreateXYLine();
            var xyLineComp = line.GetComponent<XYLine2D>();
            Vector3[] polyline;

            xyLineComp.lineRenderer.startWidth = 0.02f;
            xyLineComp.lineRenderer.endWidth = 0.02f;

            var points = new List<Vector3>();

            for(int i = 0; i < Data.infoObjects.Count; i++)
            {
                InfoObject o = Data.infoObjects[i];

                bool valAMissing = Data.IsValueMissing(o, attributeA);
                bool valBMissing = Data.IsValueMissing(o, attributeB);

                if(!(valAMissing || valBMissing))
                {
                    var valA = Data.ValueOf(o, attributeA);
                    var x = GetAxis(AxisDirection.X).TransformToAxisSpace(valA);

                    var valB = Data.ValueOf(o, attributeB);
                    var y = GetAxis(AxisDirection.Y).TransformToAxisSpace(valB);

                    points.Add(new Vector3(x, y, 0));
                    
                    RememberRelationOf(o, line.GetComponent<XYLine2D>());
                }
            }

            polyline = points.ToArray();

            xyLineComp.visBridgePort.transform.localPosition = polyline[0];
            xyLineComp.lineRenderer.positionCount = polyline.Length;
            xyLineComp.lineRenderer.SetPositions(polyline);
            line.transform.parent = Anchor.transform;

            primitive = xyLineComp;
        }
    }
}