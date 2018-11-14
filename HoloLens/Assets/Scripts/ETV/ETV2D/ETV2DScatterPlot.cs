using GraphicalPrimitive;
using Model;
using System.Collections.Generic;
using UnityEngine;

namespace ETV
{
    public class ETV2DScatterPlot : AETVScatterPlot
    {
        private AScatterDot[] dots;
        private string attributeA, attributeB;

        public override void Init(DataSet data, string[] attributes, bool isMetaVis = false)
        {
            base.Init(data, isMetaVis);
            this.attributeA = attributes[0];
            this.attributeB = attributes[1];

            SetUpAxes();
            DrawGraph();
        }

        public override void UpdateETV()
        {
            SetUpAxes();
            DrawGraph();
        }

        public override void SetUpAxes()
        {
            AddAxis(attributeA, AxisDirection.X);
            AddAxis(attributeB, AxisDirection.Y);
        }

        public override void DrawGraph()
        {
            var dotArray = new List<AScatterDot>();

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
                    dot.SetColor(Data.colorTable[infO]);
                    dot.transform.position = pos;
                    dot.transform.parent = Anchor.transform;
                    dotArray.Add(dot);
                    
                    RememberRelationOf(infO, dot);
                }
            }

            dots = dotArray.ToArray();
        }
    }


}



