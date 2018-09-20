using GraphicalPrimitive;
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
    private IList<GameObject> bars;

    public void Init(int barCount, float[] values, float[] rangesToNormalizeTo, float groupWidth, float depth)
    {
        Graphical3DPrimitiveFactory factory = ServiceLocator.instance.Factory3DPrimitives;

        bars = new List<GameObject>();
        float singleWidth = groupWidth / barCount;

        for(int i=0; i<barCount; i++)
        {
            GameObject bar = factory.CreateBar(values[i], singleWidth, depth);
            bar.transform.parent = gameObject.transform;
            bar.transform.localPosition = new Vector3(-groupWidth/2 + singleWidth/2 + singleWidth*i, 0, 0);
            bars.Add(bar);
            Bar3D bar3D = bar.GetComponent<Bar3D>();
            bar3D.SetLabelText(values[i].ToString());
        }

        labelCategory.transform.localPosition = new Vector3(0,0,-depth/2);
    }

    public void SetLabelCategoryText(string newText)
    {
        labelCategory.GetComponent<TextMesh>().text = newText;
    }

    public List<Color> ChangeColoringScheme(ETVColorSchemes scheme)
    {
        List<Color> colors = new List<Color>();
        int category = 0;
        int numberOfCategories = bars.Count;
        switch (scheme)
        {
            case ETVColorSchemes.Rainbow:
                foreach (GameObject bar in bars)
                {
                    Color color = Color.HSVToRGB(((float)category) / numberOfCategories, 1, 1);
                    bar.GetComponent<Bar3D>().SetColor(color);
                    bar.GetComponent<Bar3D>().ApplyColor(color);
                    category++;
                    colors.Add(color);
                }
                break;
            case ETVColorSchemes.GrayZebra:
                bool even = true;
                foreach (GameObject bar in bars)
                {
                    Color color = (even) ? Color.gray : Color.white;
                    bar.GetComponent<Bar3D>().SetColor(color);
                    bar.GetComponent<Bar3D>().ApplyColor(color);
                    even = !even;
                    category++;
                    colors.Add(color);
                }
                break;
            default:
                break;
        }

        return colors;
    }
}
