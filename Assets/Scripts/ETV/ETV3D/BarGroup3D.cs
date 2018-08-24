using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarGroup3D : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // .................................................................................... NON-Unity-Methods

    public GameObject labelCategory;

    public void Init(int barCount, float[] values, float[] rangesToNormalizeTo, float groupWidth, float depth)
    {
        Graphical3DPrimitiveFactory factory = ServiceLocator.instance.factory3Dservice;
        float singleWidth = groupWidth / barCount;

        for(int i=0; i<barCount; i++)
        {
            GameObject bar = factory.CreateBar(values[i], rangesToNormalizeTo[i], singleWidth, depth);
            bar.transform.localPosition = new Vector3(-groupWidth/2 + singleWidth/2 + singleWidth*i, 0, 0);
            bar.transform.parent = gameObject.transform;
        }
    }

    public void SetLabelCategoryText(string newText)
    {
        labelCategory.GetComponent<TextMesh>().text = newText;
    }
}
