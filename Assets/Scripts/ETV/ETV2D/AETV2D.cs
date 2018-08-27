using System.Collections;
using System.Collections.Generic;
using GraphicalPrimitive;
using UnityEngine;

public abstract class AETV2D : MonoBehaviour, IEuclideanTransformableView
{
    public GameObject screen;
    public float[] bounds { get; set; }

    public virtual void ChangeColoringScheme(ETVColorSchemes scheme) { }
    public virtual void SetAxisLabels(AxisDirection axisDirection, string axisVariable, string axisUnit) { }
    public virtual void SetUpAxis() { }
    public virtual void UpdateETV() { }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
