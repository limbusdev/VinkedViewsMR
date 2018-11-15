using System.Collections.Generic;
using GraphicalPrimitive;
using Model;
using UnityEngine;

namespace ETV
{
    public class ETV3DScatterPlot : AETVScatterPlot
    {
        private string attributeA, attributeB, attributeC;

        public override void Init(DataSet data, string[] attributes, bool isMetaVis = false)
        {
            base.Init(data, isMetaVis);
            attributeA = attributes[0];
            attributeB = attributes[1];
            attributeC = attributes[2];

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

                    var dot = Services.PrimFactory3D().CreateScatterDot();
                    dot.SetColor(Data.colorTable[infO], Data.colorTableBrushing[infO]);
                    dot.transform.position = pos;
                    dot.transform.parent = Anchor.transform;

                    RememberRelationOf(infO, dot);
                }
            }
        }

        public override void UpdateETV()
        {
            SetUpAxes();
            DrawGraph();
        }
    }
}