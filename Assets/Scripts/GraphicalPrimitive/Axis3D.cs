
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GraphicalPrimitive
{
    public class Axis3D : AAxis
    {

        // Use this for initialization
        void Start()
        {
            length = 2;
            diameter = .01f;
            labelVariableText = "Meters";
            labelUnitText = "m";
            UpdateAxis();
        }

        // Update is called once per frame
        void Update()
        {

        }
        
        public GameObject axisBody;
        public GameObject axisBodyGeometry;
        public GameObject axisTip;
        public GameObject labelAnchor;
        
        
        public override void UpdateAxis()
        {
            // Update Base Axis
            if (tipped)
            {
                axisBody.transform.localScale = new Vector3(diameter, length - diameter * 4f, diameter);
                axisTip.transform.localScale = new Vector3(diameter * 3f, diameter * 4f, diameter * 3f);
                axisTip.transform.localPosition = new Vector3(0, length - diameter * 4f, 0);
                labelVariable.transform.localPosition = labelAnchor.transform.position;
            }
            else
            {
                axisBody.transform.localScale = new Vector3(diameter, length, diameter);
            }

            // Update Colors
            axisBodyGeometry.GetComponent<Renderer>().material.color = color;
            axisTip.GetComponent<Renderer>().material.color = color;

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

                tick.transform.parent = axisBody.transform;
                tick.transform.localPosition = direction * ((tickCounter * tickResolution) / (max - min));
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

            labelAnchor.transform.localPosition = axisTip.transform.localPosition;
        }
    }
}
