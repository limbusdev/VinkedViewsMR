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

    private IDictionary<string, Bar3D> bars;
    private IDictionary<AxisDirection, GameObject> axis;

    private int stringAttributeID;

    public void Init(DataSet data, int nominalAttributeID)
    {
        this.data = data;
        this.stringAttributeID = nominalAttributeID;

        bars = new Dictionary<string, Bar3D>();
        axis = new Dictionary<AxisDirection, GameObject>();

        string attName = data.nomAttributes[stringAttributeID];
        var measures = data.dataMeasuresNominal[attName];

        AGraphicalPrimitiveFactory factory3D = ServiceLocator.instance.PrimitiveFactory3Dservice;

        float range = data.dataMeasuresNominal[data.nomAttributes[stringAttributeID]].zBoundRange;

        int categoryCounter = 0;


        foreach(var cat in measures.distribution.Keys)
        {
            GameObject bar = CreateBar(stringAttributeID, cat, measures.zBoundRange);
            bars.Add(cat, bar.GetComponent<Bar3D>());

            bar.transform.localPosition = new Vector3((categoryCounter) * 0.15f + 0.1f, 0, 0);

            bar.transform.parent = Anchor.transform;
            string labelString = (cat.Length > 10) ? (cat.Substring(0, 9) + ".") : cat;
            bar.GetComponent<Bar3D>().SetLabelCategoryText(labelString);

            categoryCounter++;
        }

        foreach(var o in data.informationObjects)
        {
            var bar = bars[o.nominalAtt[stringAttributeID].value];
            o.AddRepresentativeObject(attName, bar.gameObject);
        }
        
        
        SetUpAxis();
    }

    public override void SetUpAxis()
    {
        AGraphicalPrimitiveFactory factory2D = ServiceLocator.instance.PrimitiveFactory2Dservice;

        float xAxisLength = data.dataMeasuresNominal[data.nomAttributes[stringAttributeID]].distribution.Keys.Count * 0.15f;

        // x-Axis
        GameObject xAxis = factory2D.CreateAxis(Color.white, "", "", AxisDirection.X, xAxisLength + .1f, .01f, false, false);
        xAxis.transform.parent = Anchor.transform;

        // y-Axis
        GameObject yAxis = factory2D.CreateAxis(Color.white, "", "", AxisDirection.Y, 1.0f, .01f, true, true);
        Axis2D axis2D = yAxis.GetComponent<Axis2D>();

        axis2D.ticked = true;
        axis2D.min = data.dataMeasuresNominal[data.nomAttributes[stringAttributeID]].zBoundMin;
        axis2D.max = data.dataMeasuresNominal[data.nomAttributes[stringAttributeID]].zBoundMax;

        axis2D.CalculateTickResolution();
        
        yAxis.transform.parent = Anchor.transform;

        // Grid
        GameObject grid = factory2D.CreateGrid(
                    new Color(1, 1, 1, .7f),
                    Vector3.right,
                    Vector3.up,
                    1,
                    .005f,
                    data.dataMeasuresNominal[data.nomAttributes[stringAttributeID]].zBoundMin,
                    data.dataMeasuresNominal[data.nomAttributes[stringAttributeID]].zBoundMax);
        grid.transform.parent = Anchor.transform;

        axis.Add(AxisDirection.X, xAxis);
        axis.Add(AxisDirection.Y, yAxis);

        SetAxisLabels(AxisDirection.X, data.informationObjects[0].nominalAtt[stringAttributeID].name, "");
        SetAxisLabels(AxisDirection.Y, "Amount", "");
    }

    /**
     * Creates a colored bar. 
     * @param range         maximum - minimum value of this attribute
     * @param attributeID   which attribute
     * */
    private GameObject CreateBar(int stringAttributeID, string category, float range)
    {
        AGraphicalPrimitiveFactory factory3D = ServiceLocator.instance.PrimitiveFactory3Dservice;
        
        float value = data.dataMeasuresNominal[data.nomAttributes[stringAttributeID]].distribution[category];
        GameObject bar = factory3D.CreateBar(value, range, .1f, .1f);

        bar.GetComponent<Bar3D>().SetLabelText(value.ToString());

        return bar;
    }

    

    public override void ChangeColoringScheme(ETVColorSchemes scheme)
    {
        int category = 0;
        int numberOfCategories = bars.Count;
        switch(scheme)
        {
            case ETVColorSchemes.Rainbow:
                foreach(Bar3D bar in bars.Values)
                {
                    Color color = Color.HSVToRGB(((float)category) / numberOfCategories, 1, 1);
                    bar.SetColor(color);
                    bar.ApplyColor(color);
                    category++;
                }
                break;
            case ETVColorSchemes.GrayZebra:
                bool even = true;
                foreach(Bar3D bar in bars.Values)
                {
                    Color color = (even) ? Color.gray : Color.white;
                    bar.SetColor(color);
                    bar.ApplyColor(color);
                    even = !even;
                    category++;
                }
                break;
            case ETVColorSchemes.SplitHSV:
                foreach(Bar3D bar in bars.Values)
                {
                    Color color = Color.HSVToRGB((((float)category) / numberOfCategories) / 2f + .5f, 1, 1);
                    bar.SetColor(color);
                    bar.ApplyColor(color);
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
