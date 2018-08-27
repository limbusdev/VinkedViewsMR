using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GraphicalPrimitive
{
    public class Axis2D : AAxis
    {
        // Use this for initialization
        void Start()
        {
            length = 2;
            diameter = 0.01f;
            labelVariableText = "Meters";
            labelUnitText = "m";
            UpdateAxis();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public GameObject Anchor;
        public GameObject Axis;
        public GameObject Tip;
        public GameObject labelAnchor;


        // Drawing methods
        public override void UpdateAxis()
        {
            // Update Base Axis
            var lr = Axis.GetComponent<LineRenderer>();

            lr.startWidth = diameter;
            lr.endWidth = diameter;
            lr.SetPosition(0, Vector3.zero);

            if (tipped)
            {
                lr.SetPosition(1, (length - diameter / length * 4) * direction);
                var lrTip = Tip.GetComponent<LineRenderer>();
                lrTip.startWidth = diameter * 3;
                lrTip.endWidth = 0;
                lrTip.SetPosition(0, lr.GetPosition(1));
                lrTip.SetPosition(1, length * direction);
            }
            else
            {
                lr.SetPosition(1, length * direction);
            }

            // Update Ticks
            ticks = new List<GameObject>();

            Vector3 tickDirection;
            switch (axisDirection)
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

            int tickCounter = 0;
            for (float i = min; i <= max; i += tickResolution)
            {
                GameObject tick = new GameObject("Tick");
                var lineRend = tick.AddComponent<LineRenderer>();
                lineRend.useWorldSpace = false;
                lineRend.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
                lineRend.startColor = Color.white;
                lineRend.endColor = Color.white;
                lineRend.startWidth = diameter;
                lineRend.endWidth = diameter;
                lineRend.SetPosition(0, Vector3.zero);
                lineRend.SetPosition(1, tickDirection * diameter * 4f);

                GameObject tickLabel = ServiceLocator.instance.factory3Dservice.CreateLabel(i.ToString("F" + decimals));
                var textMesh = tickLabel.GetComponent<TextMesh>();
                switch (axisDirection)
                {
                    case AxisDirection.X:
                        textMesh.anchor = TextAnchor.UpperCenter;
                        textMesh.alignment = TextAlignment.Center;
                        break;
                    case AxisDirection.Y:
                        textMesh.anchor = TextAnchor.MiddleRight;
                        textMesh.alignment = TextAlignment.Right;
                        break;
                    default: /* Z */
                        textMesh.anchor = TextAnchor.MiddleRight;
                        textMesh.alignment = TextAlignment.Right;
                        break;
                }
                tickLabel.transform.localPosition = tickDirection * diameter * 5;
                tickLabel.transform.parent = tick.transform;

                tick.transform.parent = Anchor.transform;
                tick.transform.localPosition = direction * length * ((tickCounter * tickResolution) / (max - min));
                ticks.Add(tick);
                tickCounter++;
            }

            // Update Labels
            TextMesh tmVariable = labelVariable.GetComponent<TextMesh>();
            tmVariable.text = labelVariableText;

            TextMesh tmUnit = labelUnit.GetComponent<TextMesh>();
            tmUnit.text = "[" + labelUnitText + "]";

            switch (axisDirection)
            {
                case AxisDirection.X:
                    tmVariable.anchor = TextAnchor.UpperCenter;
                    tmVariable.alignment = TextAlignment.Center;
                    tmUnit.anchor = TextAnchor.UpperCenter;
                    tmUnit.alignment = TextAlignment.Center;
                    break;
                case AxisDirection.Y:
                    tmVariable.anchor = TextAnchor.MiddleRight;
                    tmVariable.alignment = TextAlignment.Right;
                    tmUnit.anchor = TextAnchor.MiddleRight;
                    tmUnit.alignment = TextAlignment.Right;
                    break;
                default:
                    tmVariable.anchor = TextAnchor.MiddleRight;
                    tmVariable.alignment = TextAlignment.Right;
                    tmUnit.anchor = TextAnchor.MiddleRight;
                    tmUnit.alignment = TextAlignment.Right;
                    break;
            }

            labelAnchor.transform.localPosition = lr.GetPosition(1);
        }
    }
}
