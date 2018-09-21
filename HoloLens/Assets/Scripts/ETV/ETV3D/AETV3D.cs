using System.Collections;
using System.Collections.Generic;
using GraphicalPrimitive;
using Model;
using UnityEngine;

public abstract class AETV3D : MonoBehaviour, IEuclideanTransformableView
{
    public IDictionary<AxisDirection, GameObject> axes { get; set; }

    public abstract void ChangeColoringScheme(ETVColorSchemes scheme);
    public abstract void SetUpAxis();
    public abstract void UpdateETV();


    public void SetAxisLabels(AxisDirection axisDirection, string axisVariable)
    {
        axes[axisDirection].GetComponent<AAxis>().labelVariableText = axisVariable;
        axes[axisDirection].GetComponent<AAxis>().UpdateAxis();
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
    }
}
