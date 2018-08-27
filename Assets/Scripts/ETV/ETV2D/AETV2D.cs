using System.Collections;
using System.Collections.Generic;
using GraphicalPrimitive;
using UnityEngine;

public abstract class AETV2D : MonoBehaviour, IEuclideanTransformableView
{
    public float[] bounds { get; set; }
    public IDictionary<AxisDirection, GameObject> axis { get; set; }

    public virtual void ChangeColoringScheme(ETVColorSchemes scheme) { }
    public virtual void SetUpAxis() { }
    public virtual void UpdateETV() { }

    public void SetAxisLabels(AxisDirection axisDirection, string axisVariable, string axisUnit)
    {
        axis[axisDirection].GetComponent<Axis2D>().labelUnitText = axisUnit;
        axis[axisDirection].GetComponent<Axis2D>().labelVariableText = axisVariable;
        axis[axisDirection].GetComponent<Axis2D>().UpdateAxis();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Awake()
    {
        axis = new Dictionary<AxisDirection, GameObject>();
        bounds = new float[] { 1, 1 };
    }
}
