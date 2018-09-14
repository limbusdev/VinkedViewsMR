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

    private DataSet dataSet;
    private IDictionary<string, InformationObject> data;
    public GameObject Anchor;
    private IList<GameObject> bars;
    private int attributeID = 0;
    private IDictionary<AxisDirection, GameObject> axis;
    
    private float yRange = 1f;
    private float xRange = 1f;

    public void Init(DataSet data, int nominalAttributeID, int numericAttributeID)
    {/*
        this.data = dataSet.dataObjects;
        this.dataSet = dataSet;

        AGraphicalPrimitiveFactory factory3D = ServiceLocator.instance.PrimitiveFactory3Dservice;
        bars = new List<GameObject>();
        IEnumerator<string> keyEnum = data.Keys.GetEnumerator();
        keyEnum.MoveNext();
        axis = new Dictionary<AxisDirection, GameObject>();

        this.attributeID = attributeID;
        float attributeRange = DataProcessor.CalculateRange(data, attributeID);

        float scaleX, scaleY, scaleZ;
        scaleX = .015f * data.Count;
        scaleY = 1f;
        scaleZ = .01f;

        

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


        SetUpAxis();*/
    }

    public override void SetUpAxis()
    {
        AGraphicalPrimitiveFactory factory2D = ServiceLocator.instance.PrimitiveFactory2Dservice;

        // x-Axis
        GameObject xAxis = factory2D.CreateAxis(Color.white, "", "", AxisDirection.X, data.Count * 0.15f + .1f, .01f, false, false);
        xAxis.transform.parent = Anchor.transform;

        // y-Axis
        GameObject yAxis = factory2D.CreateAxis(Color.white, "", "", AxisDirection.Y, 1.0f, .01f, true, true);
        yAxis.transform.parent = Anchor.transform;

        // Grid
        GameObject grid = factory2D.CreateGrid(Color.gray, AxisDirection.X, AxisDirection.Y,
            true, 10, 0.1f, data.Count * 0.15f, false);
        grid.transform.parent = Anchor.transform;

        axis.Add(AxisDirection.X, xAxis);
        axis.Add(AxisDirection.Y, yAxis);

        SetAxisLabels(AxisDirection.X, "Category", "");
        SetAxisLabels(AxisDirection.Y, dataSet.Variables[attributeID], dataSet.Units[attributeID]);
    }

    /**
     * Creates a colored bar. 
     * @param range         maximum - minimum value of this attribute
     * @param attributeID   which attribute
     * */
    private GameObject CreateBar(string category, InformationObject obj, int attributeID, float range)
    {
        AGraphicalPrimitiveFactory factory3D = ServiceLocator.instance.PrimitiveFactory3Dservice;

        //float value = data[category].attributeValues[attributeID];
        float value = 0;
        GameObject bar = factory3D.CreateBar(value, range, .1f, .1f);
       
        //bar.GetComponent<Bar3D>().SetLabelText(data[category].attributeValues[attributeID].ToString());


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
                    bar.GetComponent<Bar3D>().SetColor(color);
                    bar.GetComponent<Bar3D>().ApplyColor(color);
                    category++;
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

    public override void SetAxisLabels(AxisDirection axisDirection, string axisVariable, string axisUnit)
    {
        axis[axisDirection].GetComponent<Axis2D>().labelUnitText = axisUnit;
        axis[axisDirection].GetComponent<Axis2D>().labelVariableText = axisVariable;
        axis[axisDirection].GetComponent<Axis2D>().UpdateAxis();
    }

    public override void UpdateETV()
    {
        throw new System.NotImplementedException();
    }
}
