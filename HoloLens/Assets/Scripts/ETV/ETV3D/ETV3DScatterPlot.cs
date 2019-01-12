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

            var secondXAxis = AddAxis(attributeA, AxisDirection.X);
            var secondYAxis = AddAxis(attributeB, AxisDirection.Y);
            var secondZAxis = AddAxis(attributeC, AxisDirection.Z);

            secondXAxis.transform.localPosition = new Vector3(0, 1, 0);
            secondYAxis.transform.localPosition = new Vector3(1, 0, 0);
            secondZAxis.transform.localPosition = new Vector3(1, 0, 0);

            secondXAxis.transform.localRotation = Quaternion.Euler(180, 0, 0);
            secondYAxis.transform.localRotation = Quaternion.Euler(0, 180, 0);
            secondZAxis.transform.localRotation = Quaternion.Euler(0, 0, 180);

            var thirdXAxis = AddAxis(attributeA, AxisDirection.X);
            var thirdYAxis = AddAxis(attributeB, AxisDirection.Y);
            var thirdZAxis = AddAxis(attributeC, AxisDirection.Z);

            thirdXAxis.transform.localPosition = new Vector3(0, 0, 1);
            thirdYAxis.transform.localPosition = new Vector3(0, 0, 1);
            thirdZAxis.transform.localPosition = new Vector3(0, 1, 0);

            var fourthXAxis = AddAxis(attributeA, AxisDirection.X);
            var fourthYAxis = AddAxis(attributeB, AxisDirection.Y);
            var fourthZAxis = AddAxis(attributeC, AxisDirection.Z);

            fourthXAxis.transform.localPosition = new Vector3(0, 1, 1);
            fourthYAxis.transform.localPosition = new Vector3(1, 0, 1);
            fourthZAxis.transform.localPosition = new Vector3(1, 1, 0);

            fourthXAxis.transform.localRotation = Quaternion.Euler(180, 0, 0);
            fourthYAxis.transform.localRotation = Quaternion.Euler(0, 180, 0);
            fourthZAxis.transform.localRotation = Quaternion.Euler(0, 0, 180);

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

        }
    }
}