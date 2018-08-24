using GraphicalPrimitive;
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
public class ETV3DBarChart : AETV3D {

    private IDictionary<string, DataObject> data;
    public GameObject Anchor;
    private IList<GameObject> bars;
    private int attributeID = 0;
    
    private float yRange = 1f;
    private float xRange = 1f;

    public void Init(IDictionary<string, DataObject> data, int attributeID)
    {
        AGraphicalPrimitiveFactory factory3D = ServiceLocator.instance.get3DFactory();
        bars = new List<GameObject>();
        IEnumerator<string> keyEnum = data.Keys.GetEnumerator();
        keyEnum.MoveNext();

        this.attributeID = attributeID;
        float attributeRange = DataProcessor.CalculateRange(data, attributeID);

        float scaleX, scaleY, scaleZ;
        scaleX = .015f * data.Count;
        scaleY = 1f;
        scaleZ = .01f;

        this.data = data;

        int categoryCounter = 0;

        foreach (string category in data.Keys)
        {
            GameObject bar = CreateBar(category, data[category], attributeID, attributeRange);
            bars.Add(bar);

            bar.transform.localPosition = new Vector3((categoryCounter) * 0.15f + 0.1f, 0, 0);

            bar.transform.parent = Anchor.transform;
            bar.GetComponent<Bar3D>().SetLabelCategoryText(category);
            
            categoryCounter++;
        }


        SetUpAxis();
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
        
        float value = data[category].attributeValues[attributeID];
        GameObject bar = factory3D.CreateBar(value, range, .1f, .1f);
       
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
                foreach(GameObject bar in bars)
                {
                    Color color = Color.HSVToRGB(((float)category) / numberOfCategories, 1, 1);
                    bar.GetComponent<GraphicalPrimitive.Bar3D>().ChangeColor(color);
                    category++;
                }
                break;
            case ETVColorSchemes.GrayZebra:
                bool even = true;
                foreach (GameObject bar in bars)
                {
                    bar.GetComponent<GraphicalPrimitive.Bar3D>().ChangeColor((even) ? Color.gray : Color.white);
                    even = !even;
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
