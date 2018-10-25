
using Model;
using System.Collections;
using System.Collections.Generic;
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

        public void Init(NominalAttributeStats m, AxisDirection dir = AxisDirection.Y, bool manualLength = false, float length = 1)
        {
            base.Init(m, dir);
            this.min = 0;
            this.max = m.max;

            if(manualLength)
            {
                this.length = length;
                this.tickResolution = 1f / (m.numberOfUniqueValues - 1);
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

        public void Init(OrdinalAttributeStats m, AxisDirection dir = AxisDirection.Y, bool manualLength = false, float length = 1)
        {
            base.Init(m, dir);
            this.min = m.min;
            this.max = m.max;

            if(manualLength)
            {
                this.length = length;
                this.tickResolution = 1f / (m.numberOfUniqueValues - 1);
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

        public void Init(IntervalAttributeStats m, AxisDirection dir = AxisDirection.Y)
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

        public void Init(RatioAttributeStats m, AxisDirection dir = AxisDirection.Y)
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

        private void AssembleNominalAxis(NominalAttributeStats m, bool manualLength = false, float tickRes = .15f)
        {
            DrawBaseAxis();
            GenerateNominalTicks(m, manualLength, tickRes);
            UpdateLabels();
        }

        private void AssembleOrdinalAxis(OrdinalAttributeStats m, bool manualLength = false, float tickRes = .15f)
        {
            DrawBaseAxis();
            GenerateOrdinalTicks(m, manualLength, tickRes);
            UpdateLabels();
        }

        private void AssembleIntervalAxis(IntervalAttributeStats m)
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

        private void GenerateNominalTicks(NominalAttributeStats m, bool manualTickRes = false, float tickRes = .15f)
        {
            var tickDir = DefineTickDirection(axisDirection);

            // Draw ticks
            for(int i = 0; i <= m.max; i++)
            {
                if(!GlobalSettings.onHoloLens || (i == m.min || i == m.max))
                {
                    var tick = ServiceLocator.instance.Factory2DPrimitives.CreateTick(true);
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
        }

        private void GenerateOrdinalTicks(OrdinalAttributeStats m, bool manualTickRes = false, float tickRes = .15f)
        {
            var tickDir = DefineTickDirection(axisDirection);

            // Draw ticks
            for(int i = 0; i <= m.max; i++)
            {
                if(!GlobalSettings.onHoloLens || (i == m.min || i == m.max))
                {
                    var tick = ServiceLocator.instance.Factory2DPrimitives.CreateTick(true);
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
        }

        private void GenerateIntervalTicks(IntervalAttributeStats m)
        {
            var tickDir = DefineTickDirection(axisDirection);

            // Draw ticks
            for(int i = m.min; i <= m.max; i++)
            {
                if(!GlobalSettings.onHoloLens || (i == m.min || i == m.max))
                {
                    var tick = ServiceLocator.instance.Factory2DPrimitives.CreateTick(true);
                    tick.lr.SetPosition(1, tickDir * diameter * 4f);
                    tick.SetDirection(axisDirection);
                    tick.label.text = IntervalValueConverters.Translate(i, m.intervalTranslator);

                    tick.transform.parent = Anchor.transform;
                    tick.transform.localPosition = direction * TransformToAxisSpace(i);
                    ticks.Add(tick);
                }
            }
        }

        private void GenerateRatioTicks()
        {
            var tickDir = DefineTickDirection(axisDirection);

            int tickCounter = 0;
            // Draw ticks
            for(float i = min; i <= max; i += tickResolution)
            {
                if(!GlobalSettings.onHoloLens || (tickCounter == 0 || (i + tickResolution) >= max))
                {
                    var tick = ServiceLocator.instance.Factory2DPrimitives.CreateTick(true);
                    tick.lr.SetPosition(1, tickDir * diameter * 4f);
                    tick.SetDirection(axisDirection);
                    tick.label.text = i.ToString(i % 1 == 0 ? "N0" : ("N" + decimals));

                    tick.transform.parent = Anchor.transform;
                    tick.transform.localPosition = direction * TransformToAxisSpace(tickCounter * tickResolution);
                    ticks.Add(tick);
                }
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
