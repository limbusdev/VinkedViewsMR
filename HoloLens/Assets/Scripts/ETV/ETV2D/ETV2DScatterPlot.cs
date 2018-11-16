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



