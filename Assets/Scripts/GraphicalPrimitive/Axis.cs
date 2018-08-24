using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axis : MonoBehaviour {

    public GameObject axisBody;
    public GameObject axisBodyGeometry;
    public GameObject axisTip;
    public GameObject labelAnchor;
    public GameObject label;
    private float diameter = 1;
    private float length = 1;
    private bool tipped = true;

    public void ChangeColor(Color newColor)
    {
        axisBodyGeometry.GetComponent<Renderer>().material.color = newColor;
        axisTip.GetComponent<Renderer>().material.color = newColor;
    }

    public void SetDiameter(float diameter)
    {
        SetSize(diameter, length);
    }

    public void SetLength(float length)
    {
        SetSize(diameter, length);
    }

    public void SetSize(float diameter, float length)
    {
        this.diameter = diameter;
        this.length = length;

        if (tipped)
        {
            axisBody.transform.localScale = new Vector3(diameter, length - diameter * 4f, diameter);
            axisTip.transform.localScale = new Vector3(diameter * 3f, diameter * 4f, diameter * 3f);
            axisTip.transform.localPosition = new Vector3(0, length - diameter * 4f, 0);
            label.transform.localPosition = labelAnchor.transform.position;
        } else
        {
            axisBody.transform.localScale = new Vector3(diameter, length, diameter);
        }
    }

    public void SetTipped(bool tipped)
    {
        this.tipped = tipped;
        axisTip.SetActive(tipped);
        SetSize(diameter, length);
    }

    public void SetLabelText(string newText)
    {
        label.GetComponent<TextMesh>().text = newText;
    }

    // Use this for initialization
    void Start ()
    {

	}
	
	// Update is called once per frame
	void Update () {
        
    }
}
