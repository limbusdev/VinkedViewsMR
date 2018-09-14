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
public class ETV3DBarChart : AETV3D
{
    public GameObject Anchor;

    private DataSet data;

    private IList<Bar3D> bars;
    private IDictionary<AxisDirection, GameObject> axis;

    private int stringAttributeID;
    private int floatAttributeID;

    public void Init(DataSet data, int nominalAttributeID, int numericAttributeID)
    {
        this.data = data;
        this.stringAttributeID = nominalAttributeID;
        this.floatAttributeID = numericAttributeID;

        bars = new List<Bar3D>();
        axis = new Dictionary<AxisDirection, GameObject>();

        AGraphicalPrimitiveFactory factory3D = ServiceLocator.instance.PrimitiveFactory3Dservice;

        float attributeRange = DataProcessor.FloatAttribute.CalculateZeroBoundRange(data.informationObjects, floatAttributeID);

        int categoryCounter = 0;

        foreach(InformationObject o in data.informationObjects)
        {
            GameObject bar = CreateBar(o, stringAttributeID, floatAttributeID, attributeRange);
            bars.Add(bar.GetComponent<Bar3D>());

            bar.transform.localPosition = new Vector3((categoryCounter) * 0.15f + 0.1f, 0, 0);

            bar.transform.parent = Anchor.transform;
            string category = o.attributesString[stringAttributeID].value;
            string labelString = (category.Length > 10) ? (category.Substring(0, 9) + ".") : category;
            bar.GetComponent<Bar3D>().SetLabelCategoryText(labelString);

            categoryCounter++;
        }


        SetUpAxis();
    }

    public override void SetUpAxis()
    {
        AGraphicalPrimitiveFactory factory2D = ServiceLocator.instance.PrimitiveFactory2Dservice;

        // x-Axis
        GameObject xAxis = factory2D.CreateAxis(Color.white, "", "", AxisDirection.X, data.informationObjects.Count * 0.15f + .1f, .01f, false, false);
        xAxis.transform.parent = Anchor.transform;

        // y-Axis
        GameObject yAxis = factory2D.CreateAxis(Color.white, "", "", AxisDirection.Y, 1.0f, .01f, true, true);
        Axis2D axis2D = yAxis.GetComponent<Axis2D>();

        axis2D.ticked = true;
        axis2D.min = data.dataMeasuresFloat[data.attributesFloat[floatAttributeID]].zeroBoundMin;
        axis2D.max = data.dataMeasuresFloat[data.attributesFloat[floatAttributeID]].zeroBoundMax;

        axis2D.CalculateTickResolution();
        
        yAxis.transform.parent = Anchor.transform;

        // Grid
        GameObject grid = factory2D.CreateGrid(Color.gray, AxisDirection.X, AxisDirection.Y,
            true, 10, 0.1f, data.informationObjects.Count * 0.15f, false);
        grid.transform.parent = Anchor.transform;

        axis.Add(AxisDirection.X, xAxis);
        axis.Add(AxisDirection.Y, yAxis);

        SetAxisLabels(AxisDirection.X, data.informationObjects[0].attributesString[stringAttributeID].name, "");
        SetAxisLabels(AxisDirection.Y, data.informationObjects[0].attributesFloat[floatAttributeID].name, "");
    }

    /**
     * Creates a colored bar. 
     * @param range         maximum - minimum value of this attribute
     * @param attributeID   which attribute
     * */
    private GameObject CreateBar(InformationObject obj, int stringAttributeID, int floatAttributeID, float range)
    {
        AGraphicalPrimitiveFactory factory3D = ServiceLocator.instance.PrimitiveFactory3Dservice;

        Debug.Log("creating bar: " + obj.attributesFloat[floatAttributeID].name + " Height: " + obj.attributesFloat[floatAttributeID].value);

        float value = obj.attributesFloat[floatAttributeID].value;
        GameObject bar = factory3D.CreateBar(value, range, .1f, .1f);

        bar.GetComponent<Bar3D>().SetLabelText(obj.attributesFloat[floatAttributeID].value.ToString());

        obj.AddRepresentativeObject(obj.attributesString[stringAttributeID].name, bar);
        obj.AddRepresentativeObject(obj.attributesFloat[floatAttributeID].name, bar);

        return bar;
    }

    

    public override void ChangeColoringScheme(ETVColorSchemes scheme)
    {
        int category = 0;
        int numberOfCategories = bars.Count;
        switch (scheme)
        {
            case ETVColorSchemes.Rainbow:
                foreach(var bar in bars)
                {
                    Color color = Color.HSVToRGB(((float)category) / numberOfCategories, 1, 1);
                    bar.GetComponent<Bar3D>().SetColor(color);
                    bar.GetComponent<Bar3D>().ApplyColor(color);
                    category++;
                }
                break;
            case ETVColorSchemes.GrayZebra:
                bool even = true;
                foreach (var bar in bars)
                {
                    Color color = (even) ? Color.gray : Color.white;
                    bar.GetComponent<Bar3D>().SetColor(color);
                    bar.GetComponent<Bar3D>().ApplyColor(color);
                    even = !even;
                    category++;
                }
                break;
            case ETVColorSchemes.SplitHSV:
                foreach(var bar in bars)
                {
                    Color color = Color.HSVToRGB((((float)category) / numberOfCategories)/2f+.5f, 1, 1);
                    bar.GetComponent<Bar3D>().SetColor(color);
                    bar.GetComponent<Bar3D>().ApplyColor(color);
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
