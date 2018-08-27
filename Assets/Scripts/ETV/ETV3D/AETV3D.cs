using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AETV3D : MonoBehaviour, IEuclideanTransformableView {

    public abstract void ChangeColoringScheme(ETVColorSchemes scheme);

    public abstract void SetUpAxis();
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
