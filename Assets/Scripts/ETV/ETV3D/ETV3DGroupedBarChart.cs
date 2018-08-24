using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 3D Euclidean transformable View: 3D Bar Chart
 * 
 * A Bar chart which normalizes linearily to 1,
 * calculated from the maximum of the provided
 * values.
 * */
public class ETV3DGroupedBarChart : AETV3D {

    private IDictionary<string, DataObject> data;
    public GameObject Anchor;
    private IList<IList<GameObject>> bars;
    
    private float yRange = 1f;
    private float xRange = 1f;

    public void Init(IDictionary<string, DataObject> data)
    {
        /*AGraphicalPrimitiveFactory factory3D = ServiceLocator.instance.get3DFactory();
        bars = new List<IList<GameObject>>();
        IEnumerator<string> keyEnum = data.Keys.GetEnumerator();
        keyEnum.MoveNext();
        float[] attributeRanges = new float[data[keyEnum.Current].attributeValues.Length];

        float scaleX, scaleY, scaleZ;
        scaleX = .015f * data.Count;
        scaleY = 1f;
        scaleZ = .01f;

        this.data = data;

        for (int i = 0; i < attributeRanges.Length; i++)
        {
            attributeRanges[i] = DataProcessor.CalculateRange(data, i);
        }

        int categoryCounter = 0;

        foreach (string category in data.Keys)
        {
            IList<GameObject> barsOfCurrentCategory = new List<GameObject>();
            bars.Add(barsOfCurrentCategory);
            int attributesCount = data[category].attributeValues.Length;

            for (int i = 0; i < data[category].attributeValues.Length; i++)
            {
                GameObject bar = CreateBar(category, data[category], i, attributeRanges[i]);
                barsOfCurrentCategory.Add(bar);

                bar.transform.localPosition = new Vector3(
                    categoryCounter * 0.15f - AnchorPosition.z + 0.05f * (i - 1), 0, 0);

                bar.transform.parent = Anchor.transform;

                var caption = new GameObject("bar caption");
                var textMesh = caption.AddComponent<TextMesh>();
                textMesh.text = category;
                textMesh.fontSize = 24;
                caption.transform.localScale = new Vector3(TextScale, TextScale, TextScale);
                caption.transform.eulerAngles = new Vector3(90, 90, 0);
                caption.transform.localPosition = new Vector3(categoryCounter * .15f + .1f, 0, -.15f);
                caption.transform.parent = Anchor.transform;
            }

            categoryCounter++;
        }


        SetUpAxis();*/
    }

    public override void SetUpAxis()
    {
        AGraphicalPrimitiveFactory factory2D = ServiceLocator.instance.get2DFactory();

        // x-Axis
        GameObject xAxis = factory2D.CreateAxis(Color.white, "", "", new Vector3(1, 0, 0), data.Count * 0.15f + .1f, false);
        xAxis.transform.parent = Anchor.transform;

        // y-Axis
        GameObject yAxis = factory2D.CreateAxis(Color.white, "", "", new Vector3(0, 1, 0), 1.1f, true);
        yAxis.transform.parent = Anchor.transform;

        // Grid
        GameObject grid = factory2D.CreateGrid(Color.gray, new Vector3(1, 0, 0), new Vector3(0, 1, 0),
            true, 10, 0.1f, data.Count * 0.15f, false);
        grid.transform.parent = Anchor.transform;
    }

    /**
     * Creates a colored bar. 
     * @param range         maximum - minimum value of this attribute
     * @param attributeID   which attribute
     * */
    private GameObject CreateBar(string category, DataObject obj, int attributeID, float range)
    {
        AGraphicalPrimitiveFactory factory3D = ServiceLocator.instance.get3DFactory();
        
        int dimension = obj.attributeValues.Length;
        float value = data[category].attributeValues[attributeID];
        GameObject bar = factory3D.CreateBar(value, range, .1f / dimension, .1f);
       
        bar.GetComponent<GraphicalPrimitive.Bar3D>().SetLabelText(data[category].attributeValues[attributeID].ToString());


        return bar;
    }

    

    public override void ChangeColoringScheme(ETVColorSchemes scheme)
    {
        int category = 0;
        int numberOfCategories = bars.Count;
        switch (scheme)
        {
            case ETVColorSchemes.Rainbow:
                foreach(IList<GameObject> barList in bars)
                {
                    int attribute = 0;
                    foreach (GameObject bar in barList)
                    {
                        int numberOfAttributes = barList.Count;
                        Debug.Log("number of attributes: " + numberOfAttributes);
                        Color color;

                        if (numberOfAttributes == 1)
                        {
                            color = Color.HSVToRGB(((float)category) / numberOfCategories, 1, 1);
                        }
                        else
                        {
                            color = Color.HSVToRGB(((float)attribute) / numberOfAttributes, 1, 1);
                            Debug.Log("Color " + attribute + " = " + ((float)attribute) / numberOfAttributes);
                        }
                        bar.GetComponent<GraphicalPrimitive.Bar3D>().ChangeColor(color);
                        attribute++;
                    }   
                    category++;
                }
                break;
            case ETVColorSchemes.GrayZebra:
                bool even = true;
                foreach (IList<GameObject> barList in bars)
                {
                    if(barList.Count > 1)
                    {
                        even = true;
                    }
                    foreach (GameObject bar in barList)
                    {
                        int attribute = 0;
                        int numberOfAttributes = barList.Count;
                        even = !even;
                        bar.GetComponent<GraphicalPrimitive.Bar3D>().ChangeColor((even) ? Color.gray : Color.white);
                        attribute++;
                    }
                    category++;
                }
                break;
            default:
                break;
        }
    }

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
