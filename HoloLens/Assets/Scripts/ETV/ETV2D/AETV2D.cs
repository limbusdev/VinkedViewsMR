using System.Collections;
using System.Collections.Generic;
using GraphicalPrimitive;
using UnityEngine;

public abstract class AETV2D : MonoBehaviour, IEuclideanTransformableView
{
    public float[] bounds { get; set; }
    public IDictionary<AxisDirection, GameObject> axes { get; set; }

    public virtual void ChangeColoringScheme(ETVColorSchemes scheme) { }
    public virtual void SetUpAxis() { }
    public virtual void UpdateETV() { }

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
        bounds = new float[] { 1, 1 };
    }
}
