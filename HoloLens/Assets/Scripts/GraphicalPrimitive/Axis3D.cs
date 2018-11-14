
using Model;
using UnityEngine;

namespace GraphicalPrimitive
{
    public class Axis3D : AAxis
    {
        public GameObject Anchor;
        public GameObject axisBody;
        public GameObject axisBodyGeometry;
        public GameObject axisTip;
        public GameObject labelAnchor;
        public GameObject pivot;
        
        protected override void DrawBaseAxis()
        {
            if(tipped)
            {
                axisTip.SetActive(true);
                axisBody.transform.localScale = new Vector3(diameter, length - diameter * 4f, diameter);
                axisTip.transform.localScale = new Vector3(diameter * 3f, diameter * 4f, diameter * 3f);
                axisTip.transform.localPosition = new Vector3(0, length - diameter * 4f, 0);
            } else
            {
                axisTip.SetActive(false);
                axisBody.transform.localScale = new Vector3(diameter, length, diameter);
            }

            // Update Colors
            axisBodyGeometry.GetComponent<Renderer>().material.color = color;
            axisTip.GetComponent<Renderer>().material.color = color;

            switch(axisDirection)
            {
                case AxisDirection.Z:
                    pivot.transform.localRotation = Quaternion.Euler(90, 0, 0);
                    break;
                case AxisDirection.Y:
                    pivot.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    break;
                default: //X
                    pivot.transform.localRotation = Quaternion.Euler(0, 0, -90);
                    break;
            }
        }

        // (TEMPLATE METHOD)
        protected override void GenerateNominalTicks(NominalAttributeStats m, bool manualTickRes = false, float tickRes = .15f)
        {
            var tickDir = DefineTickDirection(axisDirection);

            // Draw ticks
            for(int i = 0; i <= m.max; i++)
            {
                var tick = Services.instance.Factory2DPrimitives.CreateTick(true);
                tick.lr.SetPosition(1, tickDir * diameter * 4f);

                tick.SetDirection(axisDirection);
                tick.label.text = m.uniqueValues[i];

                tick.transform.parent = Anchor.transform;
                if(manualTickRes)
                {
                    tick.transform.localPosition = direction * (tickRes * i);
                } else
                {
                    tick.transform.localPosition = direction * .15f * (i + 1);
                }
                ticks.Add(tick);

            }
        }

        // (TEMPLATE METHOD)
        protected override void GenerateOrdinalTicks(OrdinalAttributeStats m, bool manualTickRes = false, float tickRes = .15f)
        {
            var tickDir = DefineTickDirection(axisDirection);

            // Draw ticks
            for(int i = 0; i <= m.max; i++)
            {
                var tick = Services.instance.Factory2DPrimitives.CreateTick(true);
                tick.lr.SetPosition(1, tickDir * diameter * 4f);
                tick.SetDirection(axisDirection);
                tick.label.text = m.uniqueValues[i];

                tick.transform.parent = Anchor.transform;
                if(manualTickRes)
                {
                    tick.transform.localPosition = direction * tickRes * i;
                } else
                {
                    tick.transform.localPosition = direction * .15f * (i + 1);
                }
                ticks.Add(tick);
            }
        }

        // (TEMPLATE METHOD)
        protected override void GenerateIntervalTicks(IntervalAttributeStats m)
        {
            var tickDir = DefineTickDirection(axisDirection);

            // Draw ticks
            for(int i = m.min; i <= m.max; i++)
            {
                var tick = Services.instance.Factory2DPrimitives.CreateTick(true);
                tick.lr.SetPosition(1, tickDir * diameter * 4f);
                tick.SetDirection(axisDirection);
                tick.label.text = IntervalValueConverters.Translate(i, m.intervalTranslator);

                tick.transform.parent = Anchor.transform;
                tick.transform.localPosition = direction * TransformToAxisSpace(i);
                ticks.Add(tick);
            }
        }

        // (TEMPLATE METHOD)
        protected override void GenerateRatioTicks()
        {
            var tickDir = DefineTickDirection(axisDirection);

            int tickCounter = 0;
            // Draw ticks
            for(float i = min; i <= max; i += tickResolution)
            {
                var tick = Services.PrimFactory2D().CreateTick(true);
                tick.lr.SetPosition(1, tickDir * diameter * 4f);
                tick.SetDirection(axisDirection);
                tick.label.text = i.ToString(i % 1 == 0 ? "N0" : ("N" + decimals));

                tick.transform.parent = Anchor.transform;
                tick.transform.localPosition = direction * TransformToAxisSpace(tickCounter * tickResolution);
                ticks.Add(tick);

                tickCounter++;
            }
        }

        // (TEMPLATE METHOD)
        protected override void UpdateLabels()
        {
            TextMesh tmVariable = labelVariable.GetComponent<TextMesh>();
            tmVariable.text = labelVariableText;
            
            switch(axisDirection)
            {
                case AxisDirection.X:
                    tmVariable.anchor = TextAnchor.UpperLeft;
                    tmVariable.alignment = TextAlignment.Left;
                    break;
                case AxisDirection.Y:
                    tmVariable.anchor = TextAnchor.LowerRight;
                    tmVariable.alignment = TextAlignment.Right;
                    break;
                default:
                    tmVariable.anchor = TextAnchor.MiddleRight;
                    tmVariable.alignment = TextAlignment.Right;
                    break;
            }

            if(tipped)
            {
                labelAnchor.transform.localPosition = direction*length;
            } else
            {
                labelAnchor.transform.localPosition = direction * (length - diameter*3);
            }
        }

        // Drawing methods
        public override void UpdateAxis()
        {
            // Update Base Axis
            UpdateLabels();
        }
    }
}
