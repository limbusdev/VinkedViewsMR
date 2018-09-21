using System.Collections;
using System.Collections.Generic;
using GraphicalPrimitive;
using Model;
using UnityEngine;

public abstract class AETV2D : MonoBehaviour, IEuclideanTransformableView
{
    public float[] bounds { get; set; }
    public IDictionary<AxisDirection, GameObject> axes { get; set; }

    public virtual void ChangeColoringScheme(ETVColorSchemes scheme) { }
    public virtual void SetUpAxis() { }
    public virtual void UpdateETV() { }

    public void SetAxisLabels(AxisDirection axisDirection, string axisVariable)
    {
        axes[axisDirection].GetComponent<Axis2D>().labelVariableText = axisVariable;
        axes[axisDirection].GetComponent<Axis2D>().UpdateAxis();
    }

    protected void AddAxis(string attributeName, LoM lom, AxisDirection dir, DataSet data, GameObject parent)
    {
        var factory2D = ServiceLocator.instance.Factory2DPrimitives;

        GameObject axis;
        switch(lom)
        {
            case LoM.NOMINAL:
                axis = factory2D.CreateFixedLengthAutoTickedAxis(attributeName, 1f, dir, data);
                break;
            case LoM.ORDINAL:
                axis = factory2D.CreateFixedLengthAutoTickedAxis(attributeName, 1f, dir, data);
                break;
            case LoM.INTERVAL:
                axis = factory2D.CreateAutoTickedAxis(attributeName, dir, data);
                break;
            default: // RATIO
                axis = factory2D.CreateAutoTickedAxis(attributeName, dir, data);
                break;
        }
        axis.transform.parent = parent.transform;
        axes.Add(dir, axis);
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
