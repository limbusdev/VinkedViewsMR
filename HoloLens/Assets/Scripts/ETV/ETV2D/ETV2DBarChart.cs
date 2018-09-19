using GraphicalPrimitive;
using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETV2DBarChart : AETV2D
{
    public GameObject Anchor;

    private DataSet data;
    private int stringAttributeID = 0;
    private IDictionary<string, Bar2D> bars;

    public void Init(DataSet dataSet, int stringAttributeID)
    {
        this.bounds = new float[] { 0, 0, 0 };
        this.data = dataSet;
        this.stringAttributeID = stringAttributeID;
        bars = new Dictionary<string, Bar2D>();

        string attName = dataSet.nomAttributes[stringAttributeID];
        var measures = data.dataMeasuresNominal[attName];
        
        AGraphicalPrimitiveFactory factory2D = ServiceLocator.instance.PrimitiveFactory2Dservice;

        float range = measures.zBoundDistRange;

        int categoryCounter = 0;

        foreach(var cat in measures.distribution.Keys)
        {
            GameObject bar = CreateBar(stringAttributeID, cat, measures.zBoundDistRange);
            bars.Add(cat, bar.GetComponent<Bar2D>());

            bar.transform.localPosition = new Vector3((categoryCounter) * 0.15f + 0.1f, 0, 0);

            bar.transform.parent = Anchor.transform;
            string labelString = (cat.Length > 10) ? (cat.Substring(0, 9) + ".") : cat;
            bar.GetComponent<Bar2D>().SetLabelCategoryText(labelString);

            categoryCounter++;
        }

        foreach(var o in dataSet.informationObjects)
        {
            string value = o.nominalAtt[stringAttributeID].value;
            Bar2D bar = bars[value];
            o.AddRepresentativeObject(attName, bar.gameObject);
        }


        SetUpAxis();
    }


    /**
     * Creates a colored bar. 
     * @param range         maximum - minimum value of this attribute
     * @param attributeID   which attribute
     * */
    private GameObject CreateBar(int stringAttributeID, string category, float range)
    {
        AGraphicalPrimitiveFactory factory2D = ServiceLocator.instance.PrimitiveFactory2Dservice;
        
        float value = data.dataMeasuresNominal[data.nomAttributes[stringAttributeID]].distribution[category];
        GameObject bar = factory2D.CreateBar(value, range, .1f, .1f);

        bar.GetComponent<Bar2D>().SetLabelText(value.ToString());

        return bar;
    }

    public override void ChangeColoringScheme(ETVColorSchemes scheme)
    {
        int category = 0;
        int numberOfCategories = bars.Count;
        switch (scheme)
        {
            case ETVColorSchemes.Rainbow:
                foreach (Bar2D bar in bars.Values)
                {
                    Color color = Color.HSVToRGB(((float)category) / numberOfCategories, 1, 1);
                    bar.SetColor(color);
                    bar.ApplyColor(color);
                    category++;
                }
                break;
            case ETVColorSchemes.GrayZebra:
                bool even = true;
                foreach (Bar2D bar in bars.Values)
                {
                    Color color = (even) ? Color.gray : Color.white;
                    bar.SetColor(color);
                    bar.ApplyColor(color);
                    even = !even;
                    category++;
                }
                break;
            case ETVColorSchemes.SplitHSV:
                foreach(Bar2D bar in bars.Values)
                {
                    Color color = Color.HSVToRGB((((float)category) / numberOfCategories)/2f+.5f, 1, 1);
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

    

    public override void SetUpAxis()
    {
        AGraphicalPrimitiveFactory factory2D = ServiceLocator.instance.PrimitiveFactory2Dservice;

        float xAxisLength = data.dataMeasuresNominal[data.nomAttributes[stringAttributeID]].distribution.Keys.Count * 0.15f;

        // x-Axis
        GameObject xAxis = factory2D.CreateAxis(Color.white, "", "", AxisDirection.X, xAxisLength + .1f, .01f, false, false);
        xAxis.transform.localPosition = new Vector3(0, 0, -.001f);
        xAxis.transform.parent = Anchor.transform;
        
        bounds[0] += data.informationObjects.Count * 0.15f + .5f;

        // y-Axis
        GameObject yAxis = factory2D.CreateAxis(Color.white, "", "", AxisDirection.Y, 1.0f, .01f, true, true);
        Axis2D axis2D = yAxis.GetComponent<Axis2D>();

        axis2D.ticked = true;
        axis2D.min = data.dataMeasuresNominal[data.nomAttributes[stringAttributeID]].zBoundDistMin;
        axis2D.max = data.dataMeasuresNominal[data.nomAttributes[stringAttributeID]].zBoundDistMax;

        axis2D.CalculateTickResolution();

        yAxis.transform.localPosition = new Vector3(0, 0, -.001f);
        yAxis.transform.parent = Anchor.transform;
        bounds[1] += 1 + .5f;

        // Grid
        GameObject grid = factory2D.CreateGrid(
                    new Color(1, 1, 1, .7f),
                    Vector3.right,
                    Vector3.up,
                    1,
                    .005f,
                    data.dataMeasuresNominal[data.nomAttributes[stringAttributeID]].zBoundDistMin,
                    data.dataMeasuresNominal[data.nomAttributes[stringAttributeID]].zBoundDistMax);
        grid.transform.localPosition = new Vector3(0, 0, .001f);
        grid.transform.parent = Anchor.transform;

        axis.Add(AxisDirection.X, xAxis);
        axis.Add(AxisDirection.Y, yAxis);

        SetAxisLabels(AxisDirection.X, data.informationObjects[0].nominalAtt[stringAttributeID].name, "");
        SetAxisLabels(AxisDirection.Y, "Amount", "");
    }

    public override void UpdateETV()
    {
        throw new System.NotImplementedException();
    }
}
