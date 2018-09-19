using GraphicalPrimitive;
using Model;
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
    private int attID = 0;
    private LevelOfMeasurement lom;

    private IDictionary<string, Bar3D> bars;
    private IDictionary<AxisDirection, GameObject> axis;

    private int stringAttributeID;

    public void Init(DataSet dataSet, string attributeName)
    {
        this.data = dataSet;
        this.attID = dataSet.GetIDOf(attributeName);
        this.lom = dataSet.GetTypeOf(attributeName);
        bars = new Dictionary<string, Bar3D>();

        if(lom == LevelOfMeasurement.RATIO || lom == LevelOfMeasurement.INTERVAL)
        {
            Debug.LogWarning(attributeName + " is unsuitable for BarChart2D");
            return;
        }

        string attName = attributeName;

        switch(lom)
        {
            case LevelOfMeasurement.NOMINAL:
                InitNominal(dataSet, attributeName);
                break;
            default: // ORDINAL
                InitOrdinal(dataSet, attributeName);
                break;
        }

        SetUpAxis();
    }

    private void InitNominal(DataSet data, string attributeName)
    {
        var measures = data.dataMeasuresNominal[attributeName];
        var factory2D = ServiceLocator.instance.PrimitiveFactory2Dservice;
        float range = measures.zBoundDistRange;

        int categoryCounter = 0;

        foreach(var cat in measures.distribution.Keys)
        {
            GameObject bar = CreateBar(attID, cat, measures.zBoundDistRange);
            bars.Add(cat, bar.GetComponent<Bar3D>());

            bar.transform.localPosition = new Vector3((categoryCounter + 1) * 0.15f, 0, 0);

            bar.transform.parent = Anchor.transform;

            categoryCounter++;
        }

        foreach(var o in data.informationObjects)
        {
            string value = o.nominalAtt[attID].value;
            Bar3D bar = bars[value];
            o.AddRepresentativeObject(attributeName, bar.gameObject);
        }
    }

    private void InitOrdinal(DataSet data, string attributeName)
    {
        var measures = data.dataMeasuresOrdinal[attributeName];
        var factory2D = ServiceLocator.instance.PrimitiveFactory2Dservice;
        float range = measures.zBoundDistRange;

        int categoryCounter = 0;

        foreach(var cat in measures.orderedValueIDs.Keys)
        {
            string catName = measures.orderedValueIDs[cat];
            GameObject bar = CreateBar(attID, catName, measures.zBoundDistRange);
            bars.Add(catName, bar.GetComponent<Bar3D>());

            bar.transform.localPosition = new Vector3((categoryCounter + 1) * 0.15f, 0, 0);

            bar.transform.parent = Anchor.transform;

            categoryCounter++;
        }

        foreach(var o in data.informationObjects)
        {
            string value = measures.orderedValueIDs[o.ordinalAtt[attID].value];
            Bar3D bar = bars[value];
            o.AddRepresentativeObject(attributeName, bar.gameObject);
        }
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
        axis2D.min = data.dataMeasuresNominal[data.nomAttributes[stringAttributeID]].zBoundDistMin;
        axis2D.max = data.dataMeasuresNominal[data.nomAttributes[stringAttributeID]].zBoundDistMax;

        axis2D.CalculateTickResolution();
        
        yAxis.transform.parent = Anchor.transform;

        // Grid
        GameObject grid = factory2D.CreateGrid(
                    new Color(1, 1, 1, .7f),
                    Vector3.right,
                    Vector3.up,
                    1,
                    .005f,
                    data.dataMeasuresNominal[data.nomAttributes[stringAttributeID]].zBoundDistMin,
                    data.dataMeasuresNominal[data.nomAttributes[stringAttributeID]].zBoundDistMax);
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
        axis[axisDirection].GetComponent<Axis2D>().labelVariableText = axisVariable;
        axis[axisDirection].GetComponent<Axis2D>().UpdateAxis();
    }

    public override void UpdateETV()
    {
        throw new System.NotImplementedException();
    }
}
