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