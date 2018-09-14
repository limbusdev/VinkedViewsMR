using GraphicalPrimitive;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETV2DBarChart : AETV2D
{
    public GameObject Anchor;

    private DataSet data;
    private int floatAttributeID=0;
    private int stringAttributeID = 0;
    private IList<GameObject> bars;
    

    /**
     * Creates a colored bar. 
     * @param range         maximum - minimum value of this attribute
     * @param attributeID   which attribute
     * */
    private GameObject CreateBar(InformationObject obj, int stringAttributeID, int floatAttributeID, float range)
    {
        AGraphicalPrimitiveFactory factory2D = ServiceLocator.instance.PrimitiveFactory2Dservice;

        Debug.Log("creating bar: " + obj.attributesFloat[floatAttributeID].name + " Height: " + obj.attributesFloat[floatAttributeID].value);
        
        float value = obj.attributesFloat[floatAttributeID].value;
        GameObject bar = factory2D.CreateBar(value, range, .1f, .1f);

        bar.GetComponent<Bar2D>().SetLabelText(obj.attributesFloat[floatAttributeID].value.ToString());

        obj.AddRepresentativeObject(obj.attributesString[stringAttributeID].name, bar);
        obj.AddRepresentativeObject(obj.attributesFloat[floatAttributeID].name, bar);

        return bar;
    }

    public void Init(DataSet dataSet, int stringAttributeID, int floatAttributeID)
    {
        this.bounds = new float[] { 0, 0, 0};
        this.data = dataSet;
        this.floatAttributeID = floatAttributeID;
        this.stringAttributeID = stringAttributeID;
        bars = new List<GameObject>();

        AGraphicalPrimitiveFactory factory2D = ServiceLocator.instance.PrimitiveFactory2Dservice;
        
        float attributeRange = DataProcessor.FloatAttribute.CalculateZeroBoundRange(data.informationObjects, floatAttributeID);

        int categoryCounter = 0;
        
        foreach (InformationObject o in data.informationObjects)
        {
            GameObject bar = CreateBar(o, stringAttributeID, floatAttributeID, attributeRange);
            bars.Add(bar);

            bar.transform.localPosition = new Vector3((categoryCounter) * 0.15f + 0.1f, 0, 0);

            bar.transform.parent = Anchor.transform;
            string category = o.attributesString[stringAttributeID].value;
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
                    bar.GetComponent<Bar2D>().SetColor(color);
                    bar.GetComponent<Bar2D>().ApplyColor(color);
                    category++;
                }
                break;
            case ETVColorSchemes.GrayZebra:
                bool even = true;
                foreach (GameObject bar in bars)
                {
                    Color color = (even) ? Color.gray : Color.white;
                    bar.GetComponent<Bar2D>().SetColor(color);
                    bar.GetComponent<Bar2D>().ApplyColor(color);
                    even = !even;
                    category++;
                }
                break;
            case ETVColorSchemes.SplitHSV:
                foreach(GameObject bar in bars)
                {
                    Color color = Color.HSVToRGB((((float)category) / numberOfCategories)/2f+.5f, 1, 1);
                    bar.GetComponent<Bar2D>().SetColor(color);
                    bar.GetComponent<Bar2D>().ApplyColor(color);
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
        GameObject xAxis = factory2D.CreateAxis(Color.white, "", "", AxisDirection.X, data.informationObjects.Count * 0.15f + .1f, .01f, false, false);
        xAxis.transform.localPosition = new Vector3(0,0,-.001f);
        xAxis.transform.parent = Anchor.transform;
        bounds[0] += data.informationObjects.Count * 0.15f + .5f;

        // y-Axis
        GameObject yAxis = factory2D.CreateAxis(Color.white, "", "", AxisDirection.Y, 1.0f, .01f, true, true);
        yAxis.transform.localPosition = new Vector3(0, 0, -.001f);
        yAxis.transform.parent = Anchor.transform;
        bounds[1] += 1 + .5f;

        // Grid
        GameObject grid = factory2D.CreateGrid(Color.gray, AxisDirection.X, AxisDirection.Y,
            true, 10, 0.1f, data.informationObjects.Count * 0.15f, false);
        grid.transform.localPosition = new Vector3(0, 0, .001f);
        grid.transform.parent = Anchor.transform;

        axis.Add(AxisDirection.X, xAxis);
        axis.Add(AxisDirection.Y, yAxis);

        SetAxisLabels(AxisDirection.X, data.informationObjects[0].attributesString[stringAttributeID].name, "");
        SetAxisLabels(AxisDirection.Y, data.informationObjects[0].attributesFloat[floatAttributeID].name, "");
    }

    public override void UpdateETV()
    {
        throw new System.NotImplementedException();
    }
}
