using System.Collections;
using System.Collections.Generic;
using GraphicalPrimitive;
using UnityEngine;

public abstract class AETV2D : MonoBehaviour, IEuclideanTransformableView
{
    public abstract void ChangeColoringScheme(ETVColorSchemes scheme);
    public abstract void SetAxisLabels(AxisDirection axisDirection, string axisVariable, string axisUnit);
    public abstract void SetUpAxis();
    public abstract void UpdateETV();

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
