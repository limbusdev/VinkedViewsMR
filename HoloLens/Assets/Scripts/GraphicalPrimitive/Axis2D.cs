using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GraphicalPrimitive
{
    public class Axis2D : AAxis
    {
        public GameObject Anchor;
        public GameObject Axis;
        public GameObject Tip;
        public GameObject labelAnchor;
        public Material lineMaterial;

        public LevelOfMeasurement type;

        public void Init(NominalDataDimensionMeasures m, AxisDirection dir = AxisDirection.Y)
        {
            base.Init(m, dir);
            this.min= 0;
            this.max = m.numberOfUniqueValues;
            this.length = .15f * m.numberOfUniqueValues;
            this.tickResolution = .15f;
            this.tipped = false;
            this.ticked = true;
            type = LevelOfMeasurement.NOMINAL;
            AssembleNominalAxis(m);
        }

        public void Init(OrdinalDataDimensionMeasures m, AxisDirection dir = AxisDirection.Y)
        {
            base.Init(m, dir);
            this.min= m.min;
            this.max = m.max;
            this.length = .15f * m.numberOfUniqueValues;
            this.tickResolution = .15f;
            this.tipped = true;
            this.ticked = true;
            type = LevelOfMeasurement.ORDINAL;
            AssembleOrdinalAxis(m);
        }

        public void Init(IntervalDataDimensionMeasures m, AxisDirection dir = AxisDirection.Y)
        {
            base.Init(m, dir);
            this.min = m.min;
            this.max = m.max;
            this.length = 1f;
            this.tipped = true;
            this.ticked = true;
            CalculateTickResolution();
            type = LevelOfMeasurement.INTERVAL;
            AssembleIntervalAxis(m);
        }

        public void Init(RatioDataDimensionMeasures m, AxisDirection dir = AxisDirection.Y)
        {
            base.Init(m, dir);
            this.min = m.zeroBoundMin;
            this.max = m.zeroBoundMax;
            this.length = 1f;
            this.tipped = true;
            this.ticked = true;
            CalculateTickResolution();
            type = LevelOfMeasurement.RATIO;
            AssembleRatioAxis(m);
        }

        private void AssembleNominalAxis(NominalDataDimensionMeasures m)
        {
            DrawBaseAxis();
            GenerateNominalTicks(m);
            UpdateLabels();
        }

        private void AssembleOrdinalAxis(OrdinalDataDimensionMeasures m)
        {
            DrawBaseAxis();
            GenerateOrdinalTicks(m);
            UpdateLabels();
        }

        private void AssembleIntervalAxis(IntervalDataDimensionMeasures m)
        {
            DrawBaseAxis();
            GenerateIntervalTicks(m);
            UpdateLabels();
        }

        private void AssembleRatioAxis(RatioDataDimensionMeasures m)
        {
            DrawBaseAxis();
            GenerateRatioTicks(m);
            UpdateLabels();
        }

        private void DrawBaseAxis()
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

        private Vector3 DefineTickDirection(AxisDirection dir)
        {
            Vector3 tickDirection;
            switch(axisDirection)
            {
                case AxisDirection.Y:
                    tickDirection = Vector3.left;
                    break;
                case AxisDirection.Z:
                    tickDirection = Vector3.left;
                    break;
                default: /* case X */
                    tickDirection = Vector3.down;
                    break;
            }
            return tickDirection;
        }
        

        private void GenerateNominalTicks(NominalDataDimensionMeasures m)
        {
            var tickDir = DefineTickDirection(axisDirection);

            // Draw ticks
            for(int i=0; i<=m.max; i++)
            {
                var tick = ServiceLocator.instance.PrimitiveFactory2Dservice.CreateTick();
                tick.lr.SetPosition(1, tickDir * diameter * 4f);
                tick.SetDirection(axisDirection);
                tick.label.text = m.uniqueValues[i];

                tick.transform.parent = Anchor.transform;
                tick.transform.localPosition = direction * .15f*(i+1);
                ticks.Add(tick);
            }
        }

        private void GenerateOrdinalTicks(OrdinalDataDimensionMeasures m)
        {
            var tickDir = DefineTickDirection(axisDirection);

            // Draw ticks
            for(int i = 0; i <= m.max; i++)
            {
                var tick = ServiceLocator.instance.PrimitiveFactory2Dservice.CreateTick();
                tick.lr.SetPosition(1, tickDir * diameter * 4f);
                tick.SetDirection(axisDirection);
                tick.label.text = m.uniqueValues[i];

                tick.transform.parent = Anchor.transform;
                tick.transform.localPosition = direction * .15f * (i+1);
                ticks.Add(tick);
            }
        }

        private void GenerateIntervalTicks(IntervalDataDimensionMeasures m)
        {
            var tickDir = DefineTickDirection(axisDirection);

            // Draw ticks
            for(int i = m.min; i <= m.max; i++)
            {
                var tick = ServiceLocator.instance.PrimitiveFactory2Dservice.CreateTick();
                tick.lr.SetPosition(1, tickDir * diameter * 4f);
                tick.SetDirection(axisDirection);
                tick.label.text = IntervalValueConverters.Translate(i, m.intervalTranslator);

                tick.transform.parent = Anchor.transform;
                tick.transform.localPosition = direction * TransformFromValueToAxisSpace(i);
                ticks.Add(tick);
            }
        }

        private void GenerateRatioTicks(RatioDataDimensionMeasures m)
        {
            var tickDir = DefineTickDirection(axisDirection);

            int tickCounter = 0;
            // Draw ticks
            for(float i = min; i <= max; i += tickResolution)
            {
                var tick = ServiceLocator.instance.PrimitiveFactory2Dservice.CreateTick();
                tick.lr.SetPosition(1, tickDir * diameter * 4f);
                tick.SetDirection(axisDirection);
                tick.label.text = i.ToString(i % 1 == 0 ? "N0" : ("N" + decimals));

                tick.transform.parent = Anchor.transform;
                tick.transform.localPosition = direction * TransformFromValueToAxisSpace(tickCounter*tickResolution);
                ticks.Add(tick);
                tickCounter++;
            }
        }
        
        
        private void UpdateLabels()
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
