using GraphicalPrimitive;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETV2DBarChart : AETV2D
{
    public GameObject Anchor;

    private DataSet data;
    private int attributeID=0;
    private IList<GameObject> bars;
    

    /**
     * Creates a colored bar. 
     * @param range         maximum - minimum value of this attribute
     * @param attributeID   which attribute
     * */
    private GameObject CreateBar(string category, DataObject obj, int attributeID, float range)
    {
        AGraphicalPrimitiveFactory factory2D = ServiceLocator.instance.PrimitiveFactory2Dservice;

        Debug.Log("creating bar: " + data.dataObjects[category].attributeValues[attributeID] + " Height: ");
        
        float value = data.dataObjects[category].attributeValues[attributeID];
        GameObject bar = factory2D.CreateBar(value, range, .1f, .1f);

        bar.GetComponent<Bar2D>().SetLabelText(data.dataObjects[category].attributeValues[attributeID].ToString());

        return bar;
    }

    public void Init(DataSet dataSet, int attributeID)
    {
        this.bounds = new float[] { 0, 0, 0};
        this.data = dataSet;
        this.attributeID = attributeID;
        bars = new List<GameObject>();

        AGraphicalPrimitiveFactory factory2D = ServiceLocator.instance.PrimitiveFactory2Dservice;
        

        

        this.attributeID = attributeID;
        float attributeRange = DataProcessor.CalculateRange(data.dataObjects, attributeID);

        int categoryCounter = 0;

        foreach (string category in data.dataObjects.Keys)
        {
            GameObject bar = CreateBar(category, data.dataObjects[category], attributeID, attributeRange);
            bars.Add(bar);

            bar.transform.localPosition = new Vector3((categoryCounter) * 0.15f + 0.1f, 0, 0);

            bar.transform.parent = Anchor.transform;
            string labelString = (category.Length > 10) ? (category.Substring(0, 9) + ".") : category;
            bar.GetComponent<Bar2D>().SetLabelCategoryText(labelString);

            categoryCounter++;
        }


        SetUpAxis();
    }

    public override void ChangeColoringScheme(ETVColorSchemes scheme)
    {
        int category = 0;
        int numberOfCategories = bars.Count;
        switch (scheme)
        {
            case ETVColorSchemes.Rainbow:
                foreach (GameObject bar in bars)
                {
                    Color color = Color.HSVToRGB(((float)category) / numberOfCategories, 1, 1);
                    bar.GetComponent<Bar2D>().ChangeColor(color);
                    category++;
                }
                break;
            case ETVColorSchemes.GrayZebra:
                bool even = true;
                foreach (GameObject bar in bars)
                {
                    bar.GetComponent<Bar2D>().ChangeColor((even) ? Color.gray : Color.white);
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

    

    public override void SetUpAxis()
    {
        AGraphicalPrimitiveFactory factory2D = ServiceLocator.instance.PrimitiveFactory2Dservice;

        // x-Axis
        GameObject xAxis = factory2D.CreateAxis(Color.white, "", "", AxisDirection.X, data.dataObjects.Count * 0.15f + .1f, .01f, false, false);
        xAxis.transform.localPosition = new Vector3(0,0,-.001f);
        xAxis.transform.parent = Anchor.transform;
        bounds[0] += data.dataObjects.Count * 0.15f + .5f;

        // y-Axis
        GameObject yAxis = factory2D.CreateAxis(Color.white, "", "", AxisDirection.Y, 1.0f, .01f, true, true);
        yAxis.transform.localPosition = new Vector3(0, 0, -.001f);
        yAxis.transform.parent = Anchor.transform;
        bounds[1] += 1 + .5f;

        // Grid
        GameObject grid = factory2D.CreateGrid(Color.gray, AxisDirection.X, AxisDirection.Y,
            true, 10, 0.1f, data.dataObjects.Count * 0.15f, false);
        grid.transform.localPosition = new Vector3(0, 0, .001f);
        grid.transform.parent = Anchor.transform;

        axis.Add(AxisDirection.X, xAxis);
        axis.Add(AxisDirection.Y, yAxis);

        SetAxisLabels(AxisDirection.X, "Category", "");
        SetAxisLabels(AxisDirection.Y, data.Variables[attributeID], data.Units[attributeID]);
    }

    public override void UpdateETV()
    {
        throw new System.NotImplementedException();
    }
}
