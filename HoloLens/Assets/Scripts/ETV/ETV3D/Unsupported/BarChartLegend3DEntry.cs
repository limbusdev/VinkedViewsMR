using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarChartLegend3DEntry : MonoBehaviour {

    public TextMesh label;
    public Renderer cubeRenderer;

    public void Init(string category, Color color)
    {
        label.text = category;
        cubeRenderer.material.color = color;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
