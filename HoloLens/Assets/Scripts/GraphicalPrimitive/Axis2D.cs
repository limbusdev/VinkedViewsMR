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
using Model;
using UnityEngine;

namespace GraphicalPrimitive
{
    public class Axis2D : AAxis
    {
        public GameObject Anchor;
        public GameObject Axis;
        public GameObject Tip;
        public GameObject labelAnchor;

        // (TEMPLATE METHOD)
        protected override void DrawBaseAxis()
        {
            var lr = Axis.GetComponent<LineRenderer>();
            if(axisDirection == AxisDirection.Z)
            {
                lr.alignment = LineAlignment.View;
            }

            lr.startWidth = diameter;
            lr.endWidth = diameter;
            lr.SetPosition(0, Vector3.zero);

            if(tipped)
            {
                lr.SetPosition(1, (length - diameter / length * 4) * direction);
                Tip.SetActive(true);
                var lrTip = Tip.GetComponent<LineRenderer>();
                lrTip.startWidth = diameter * 3;
                lrTip.endWidth = 0;
                lrTip.SetPosition(0, lr.GetPosition(1));
                lrTip.SetPosition(1, length * direction);

                if(axisDirection == AxisDirection.Z)
                {
                    lrTip.alignment = LineAlignment.View;
                }
            } else
            {
                lr.SetPosition(1, length * direction);
                Tip.SetActive(false);
            }
        }

        // (TEMPLATE METHOD)
        protected override void GenerateNominalTicks(NominalAttributeStats m, bool manualTickRes=false, float tickRes=.15f)
        {
            float barGap = .01f;
            float dim = m.uniqueValues.Length;
            float gapWidth = (dim - 1) * barGap;
            float barWidth = (length - gapWidth - .1f) / (dim);
            float margin = .05f + barWidth / 2;

            var tickDir = DefineTickDirection(axisDirection);

            // Draw ticks
            for(int i = 0; i < dim; i++)
            {
                var tick = Services.instance.Factory2DPrimitives.CreateTick(true);

                tick.lr.SetPosition(1, tickDir * diameter * 4f);

                tick.SetDirection(axisDirection);
                tick.label.text = m.uniqueValues[i];

                tick.transform.parent = Anchor.transform;

                float offset = margin + i * (barWidth + barGap);
                tick.transform.localPosition = direction * offset;
                
                ticks.Add(tick);
            }

            base.endOffset = margin;
        }

        // (TEMPLATE METHOD)
        protected override void GenerateOrdinalTicks(OrdinalAttributeStats m, bool manualTickRes = false, float tickRes = .15f)
        {
            float barGap = .01f;
            float dim = m.uniqueValues.Length;
            float gapWidth = (dim - 1) * barGap;
            float barWidth = (length - gapWidth - .1f) / (dim);
            float margin = .05f + barWidth / 2;

            var tickDir = DefineTickDirection(axisDirection);

            // Draw ticks
            for(int i = 0; i <= m.max; i++)
            {
                var tick = Services.instance.Factory2DPrimitives.CreateTick(true);

                tick.lr.SetPosition(1, tickDir * diameter * 4f);

                tick.SetDirection(axisDirection);
                tick.label.text = m.uniqueValues[i];

                tick.transform.parent = Anchor.transform;

                float offset = margin + i * (barWidth + barGap);
                tick.transform.localPosition = direction * offset;

                ticks.Add(tick);
            }

            base.endOffset = margin;
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
                    tmVariable.anchor = TextAnchor.MiddleLeft;
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
                var lrTip = Tip.GetComponent<LineRenderer>();
                labelAnchor.transform.localPosition = lrTip.GetPosition(1);
            } else
            {
                var lr = Axis.GetComponent<LineRenderer>();
                labelAnchor.transform.localPosition = lr.GetPosition(1);
            }
        }

        // Drawing methods
        public override void UpdateAxis()
        {
            // Update Base Axis
            DrawBaseAxis();
            UpdateLabels();
        }
    }
}
