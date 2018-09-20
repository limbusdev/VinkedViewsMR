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
        public Material lineMaterial;

        public LoM type;

        public void Init(string name, float max, AxisDirection dir = AxisDirection.Y)
        {
            base.Init(name, dir);
            this.min = 0;
            this.max = max;
            this.length = 1f;
            this.tipped = true;
            this.ticked = true;
            type = LoM.RATIO;
            CalculateTickResolution();
            AssembleRatioAxis();
        }
       
        public void Init(NominalDataDimensionMeasures m, AxisDirection dir = AxisDirection.Y, bool manualLength=false, float length=1)
        {
            base.Init(m, dir);
            this.min = 0;
            this.max = m.max;

            if(manualLength)
            {
                this.length = length;
                this.tickResolution = 1f / (m.numberOfUniqueValues-1);
            } else
            {
                this.length = .15f * m.numberOfUniqueValues + .15f;
                this.tickResolution = .15f;
            }
            
            this.tipped = false;
            this.ticked = true;
            type = LoM.NOMINAL;
            AssembleNominalAxis(m, manualLength, this.tickResolution);
        }

        public void Init(OrdinalDataDimensionMeasures m, AxisDirection dir = AxisDirection.Y, bool manualLength = false, float length = 1)
        {
            base.Init(m, dir);
            this.min= m.min;
            this.max = m.max;

            if(manualLength)
            {
                this.length = length;
                this.tickResolution = 1f / (m.numberOfUniqueValues-1);
            } else
            {
                this.length = .15f * m.numberOfUniqueValues + .15f;
                this.tickResolution = .15f;
            }

            this.tipped = true;
            this.ticked = true;
            type = LoM.ORDINAL;
            AssembleOrdinalAxis(m, manualLength, this.tickResolution);
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
            type = LoM.INTERVAL;
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
            type = LoM.RATIO;
            AssembleRatioAxis();
        }

        private void AssembleNominalAxis(NominalDataDimensionMeasures m, bool manualLength=false, float tickRes=.15f)
        {
            DrawBaseAxis();
            GenerateNominalTicks(m, manualLength, tickRes);
            UpdateLabels();
        }

        private void AssembleOrdinalAxis(OrdinalDataDimensionMeasures m, bool manualLength = false, float tickRes = .15f)
        {
            DrawBaseAxis();
            GenerateOrdinalTicks(m, manualLength, tickRes);
            UpdateLabels();
        }

        private void AssembleIntervalAxis(IntervalDataDimensionMeasures m)
        {
            DrawBaseAxis();
            GenerateIntervalTicks(m);
            UpdateLabels();
        }

        private void AssembleRatioAxis()
        {
            DrawBaseAxis();
            GenerateRatioTicks();
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
        
        private void GenerateNominalTicks(NominalDataDimensionMeasures m, bool manualTickRes=false, float tickRes=.15f)
        {
            var tickDir = DefineTickDirection(axisDirection);

            // Draw ticks
            for(int i=0; i<=m.max; i++)
            {
                var tick = ServiceLocator.instance.Factory2DPrimitives.CreateTick();

                
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

        private void GenerateOrdinalTicks(OrdinalDataDimensionMeasures m, bool manualTickRes = false, float tickRes = .15f)
        {
            var tickDir = DefineTickDirection(axisDirection);

            // Draw ticks
            for(int i = 0; i <= m.max; i++)
            {
                var tick = ServiceLocator.instance.Factory2DPrimitives.CreateTick();
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

        private void GenerateIntervalTicks(IntervalDataDimensionMeasures m)
        {
            var tickDir = DefineTickDirection(axisDirection);

            // Draw ticks
            for(int i = m.min; i <= m.max; i++)
            {
                var tick = ServiceLocator.instance.Factory2DPrimitives.CreateTick();
                tick.lr.SetPosition(1, tickDir * diameter * 4f);
                tick.SetDirection(axisDirection);
                tick.label.text = IntervalValueConverters.Translate(i, m.intervalTranslator);

                tick.transform.parent = Anchor.transform;
                tick.transform.localPosition = direction * TransformToAxisSpace(i);
                ticks.Add(tick);
            }
        }

        private void GenerateRatioTicks()
        {
            var tickDir = DefineTickDirection(axisDirection);

            int tickCounter = 0;
            // Draw ticks
            for(float i = min; i <= max; i += tickResolution)
            {
                var tick = ServiceLocator.instance.Factory2DPrimitives.CreateTick();
                tick.lr.SetPosition(1, tickDir * diameter * 4f);
                tick.SetDirection(axisDirection);
                tick.label.text = i.ToString(i % 1 == 0 ? "N0" : ("N" + decimals));

                tick.transform.parent = Anchor.transform;
                tick.transform.localPosition = direction * TransformToAxisSpace(tickCounter*tickResolution);
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
