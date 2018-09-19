using GraphicalPrimitive;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tick : MonoBehaviour
{

    public TextMesh label;
    public LineRenderer lr;

    public void SetDirection(AxisDirection dir)
    {
        switch(dir)
        {
            case AxisDirection.X:
                label.alignment = TextAlignment.Right;
                label.anchor = TextAnchor.MiddleRight;
                label.transform.localRotation = Quaternion.Euler(0, 0, 90);
                label.transform.localPosition = Vector3.down * .05f;
                break;
            case AxisDirection.Y:
                label.alignment = TextAlignment.Right;
                label.anchor = TextAnchor.MiddleRight;
                label.transform.localRotation = Quaternion.Euler(0, 0, 0);
                label.transform.localPosition = Vector3.left * .05f;
                break;
            default:
                label.alignment = TextAlignment.Right;
                label.anchor = TextAnchor.MiddleRight;
                label.transform.localRotation = Quaternion.Euler(0, 0, 0);
                label.transform.localPosition = Vector3.left * .05f;
                break;
        }
    }
}
