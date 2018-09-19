using System.Collections;
using System.Collections.Generic;
using GraphicalPrimitive;
using UnityEngine;

public abstract class AETV3D : MonoBehaviour, IEuclideanTransformableView
{
    public IDictionary<AxisDirection, GameObject> axes { get; set; }

    public abstract void ChangeColoringScheme(ETVColorSchemes scheme);
    public abstract void SetUpAxis();
    public abstract void UpdateETV();


    public void SetAxisLabels(AxisDirection axisDirection, string axisVariable, string axisUnit)
    {
        axes[axisDirection].GetComponent<Axis2D>().labelVariableText = axisVariable;
        axes[axisDirection].GetComponent<Axis2D>().UpdateAxis();
    }

    public Axis2D GetAxis(AxisDirection dir)
    {
        return axes[dir].GetComponent<Axis2D>();
    }

    void Awake()
    {
        axes = new Dictionary<AxisDirection, GameObject>();
    }
}
