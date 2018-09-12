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

        }

        // Update is called once per frame
        void Update()
        {

        }

        private void Awake()
        {
            if(ticks == null) ticks = new List<GameObject>();
        }

        public GameObject Anchor;
        public GameObject Axis;
        public GameObject Tip;
        public GameObject labelAnchor;


        // Drawing methods
        public override void UpdateAxis()
        {
            foreach (GameObject go in ticks)
            {
                Destroy(go);
            }
            ticks.Clear();

            // Update Base Axis
            var lr = Axis.GetComponent<LineRenderer>();
            if(axisDirection == AxisDirection.Z)
            {
                lr.alignment = LineAlignment.View;
            }

            lr.startWidth = diameter;
            lr.endWidth = diameter;
            lr.SetPosition(0, Vector3.zero);

            if (tipped)
            {
                lr.SetPosition(1, (length - diameter / length * 4) * direction);
                Tip.SetActive(true);
                var lrTip = Tip.GetComponent<LineRenderer>();
                lrTip.startWidth = diameter * 3;
                lrTip.endWidth = 0;
                lrTip.SetPosition(0, lr.GetPosition(1));
                lrTip.SetPosition(1, length * direction);

                if (axisDirection == AxisDirection.Z)
                {
                    lrTip.alignment = LineAlignment.View;
                }
            }
            else
            {
                lr.SetPosition(1, length * direction);
                Tip.SetActive(false);
            }

            // Update Ticks
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

            if (ticked)
            {
                int tickCounter = 0;
                for (float i = min; i <= max; i += tickResolution)
                {
                    GameObject tick = new GameObject("Tick");
                    var lineRend = tick.AddComponent<LineRenderer>();
                    lineRend.useWorldSpace = false;
                    lineRend.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
                    lineRend.startColor = color;
                    lineRend.endColor = color;
                    lineRend.startWidth = diameter;
                    lineRend.endWidth = diameter;
                    lineRend.SetPosition(0, Vector3.zero);
                    lineRend.SetPosition(1, tickDirection * diameter * 4f);

                    GameObject tickLabel = ServiceLocator.instance.PrimitiveFactory2Dservice.CreateLabel(i.ToString("F" + decimals));
                    var textMesh = tickLabel.GetComponent<TextMesh>();
                    textMesh.color = color;
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
            }

            // Update Labels
            TextMesh tmVariable = labelVariable.GetComponent<TextMesh>();
            tmVariable.text = labelVariableText;

            TextMesh tmUnit = labelUnit.GetComponent<TextMesh>();
            if (labelUnitText.Length == 0)
            {
                tmUnit.text = "";
            }
            else
            {
                tmUnit.text = "[" + labelUnitText + "]";
            }

            switch (axisDirection)
            {
                case AxisDirection.X:
                    tmVariable.anchor = TextAnchor.MiddleLeft;
                    tmVariable.alignment = TextAlignment.Left;
                    tmUnit.anchor = TextAnchor.MiddleLeft;
                    tmUnit.alignment = TextAlignment.Left;
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
