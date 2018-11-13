using System.Collections.Generic;
using GraphicalPrimitive;
using Model;
using UnityEngine;

namespace ETV
{
    public class ETV3DScatterPlot : AETVScatterPlot
    {
        private AScatterDot[] dots;
        private string attributeA, attributeB, attributeC;

        public override void Init(DataSet data, string[] attributes, bool isMetaVis = false)
        {
            base.Init(data, isMetaVis);
            this.attributeA = attributes[0];
            this.attributeB = attributes[1];
            this.attributeC = attributes[2];

            SetUpAxes();
            DrawGraph();
        }

        public override void SetUpAxes()
        {
            AddAxis(attributeA, AxisDirection.X);
            AddAxis(attributeB, AxisDirection.Y);
            AddAxis(attributeC, AxisDirection.Z);
        }

        public override void DrawGraph()
        {
            var dotArray = new List<AScatterDot>();

            foreach(var infO in Data.infoObjects)
            {
                float valA = Data.ValueOf(infO, attributeA);
                float valB = Data.ValueOf(infO, attributeB);
                float valC = Data.ValueOf(infO, attributeC);

                if(!float.IsNaN(valA) && !float.IsNaN(valB) && !float.IsNaN(valC))
                {
                    var pos = new Vector3(
                        GetAxis(AxisDirection.X).TransformToAxisSpace(valA),
                        GetAxis(AxisDirection.Y).TransformToAxisSpace(valB),
                        GetAxis(AxisDirection.Z).TransformToAxisSpace(valC)
                        );

                    var dot = ServiceLocator.PrimitivePlant3D().CreateScatterDot();
                    dot.SetColor(Data.colorTable[infO]);
                    dot.transform.position = pos;
                    dot.transform.parent = Anchor.transform;
                    dotArray.Add(dot);

                    RememberRelationOf(infO, dot);
                }
            }
            dots = dotArray.ToArray();
        }

        public override void UpdateETV()
        {
            SetUpAxes();
            DrawGraph();
        }
        

        public override AGraphicalPrimitiveFactory GetGraphicalPrimitiveFactory()
        {
            return ServiceLocator.PrimitivePlant2D();
        }
    }
}