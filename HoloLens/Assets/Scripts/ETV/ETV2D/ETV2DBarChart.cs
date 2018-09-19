using GraphicalPrimitive;
using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ETV2DBarChart : AETV2D
{
    // ........................................................................ Populate in Editor
    public GameObject Anchor;


    // ........................................................................ Private properties
    private DataSet data;
    private string attributeName;
    private int attributeID;
    private LevelOfMeasurement lom;

    private IDictionary<string, Bar2D> bars;
    

    // ........................................................................ Initializers
    public void Init(DataSet data, string attributeName)
    {
        this.data = data;
        this.attributeName = attributeName;
        this.attributeID = data.GetIDOf(attributeName);
        this.lom = data.GetTypeOf(attributeName);

        this.bounds = new float[] { 0, 0, 0 };
        bars = new Dictionary<string, Bar2D>();

        SetUpAxis();

        // .................................................................... initialize
        switch(lom)
        {
            case LevelOfMeasurement.NOMINAL:
                InitNominal(data, attributeName);
                break;
            case LevelOfMeasurement.ORDINAL:
                InitOrdinal(data, attributeName);
                break;
            default:
                Debug.Log("Attribute level of measurement unsuitable for BarChart2D");
                break;
        }
    }

    private void InitNominal(DataSet data, string attributeName)
    {
        var measures = data.dataMeasuresNominal[attributeName];
        var factory = ServiceLocator.instance.PrimitiveFactory2Dservice;
        
        for(int i = 0; i < measures.numberOfUniqueValues; i++)
        {
            string val = measures.uniqueValues[i];
            InsertBar(val, measures.distribution[val], i);
        }

        foreach(var o in data.informationObjects)
        {
            string value = o.nominalAtt[attributeID].value;
            Bar2D bar = bars[value];
            o.AddRepresentativeObject(attributeName, bar.gameObject);
        }
    }

    private void InitOrdinal(DataSet data, string attributeName)
    {
        var measures = data.dataMeasuresOrdinal[attributeName];
        var factory = ServiceLocator.instance.PrimitiveFactory2Dservice;
        
        for(int i=0; i<measures.numberOfUniqueValues; i++)
        {
            InsertBar(measures.uniqueValues[i], measures.distribution[i], i);
        }

        foreach(var o in data.informationObjects)
        {
            int value = o.ordinalAtt[attributeID].value;
            Bar2D bar = bars[measures.uniqueValues[value]];
            o.AddRepresentativeObject(attributeName, bar.gameObject);
        }
    }

    // ........................................................................ Helper Methods
    private Bar2D InsertBar(string name, int value, int barID)
    {
        var factory2D = ServiceLocator.instance.PrimitiveFactory2Dservice;

        float normValue = GetAxis(AxisDirection.Y).TransformToAxisSpace(value);
        var bar = factory2D.CreateBar(normValue, .1f).GetComponent<Bar2D>();
        bar.GetComponent<Bar2D>().SetLabelText(value.ToString());

        bars.Add(name, bar);
        bar.gameObject.transform.localPosition = new Vector3((barID + 1) * 0.15f, 0, .001f);
        bar.gameObject.transform.parent = Anchor.transform;

        return bar;
    }
    
    public override void SetUpAxis()
    {
        var factory2D = ServiceLocator.instance.PrimitiveFactory2Dservice;

        var xAxis = factory2D.CreateAutoTickedAxis(attributeName, AxisDirection.X, data);
        xAxis.transform.parent = Anchor.transform;

        float max, length;

        switch(lom)
        {
            case LevelOfMeasurement.NOMINAL:
                var mea = data.dataMeasuresNominal[attributeName];
                length = (mea.numberOfUniqueValues + 1) * .15f;
                max = mea.distMax;
                break;
            default:
                var mea2 = data.dataMeasuresOrdinal[attributeName];
                length = (mea2.numberOfUniqueValues + 1) * .15f;
                max = mea2.distMax;
                break;
        }

        var yAxis = factory2D.CreateAutoTickedAxis("Amount", max);
        yAxis.transform.parent = Anchor.transform;

        // Grid
        GameObject grid = factory2D.CreateAutoGrid(max, Vector3.right, Vector3.up, length);
        grid.transform.localPosition = new Vector3(0, 0, .002f);
        grid.transform.parent = Anchor.transform;

        axes.Add(AxisDirection.X, xAxis);
        axes.Add(AxisDirection.Y, yAxis);
    }

    public override void ChangeColoringScheme(ETVColorSchemes scheme)
    {
        int category = 0;
        int numberOfCategories = bars.Count;
        switch(scheme)
        {
            case ETVColorSchemes.Rainbow:
                foreach(Bar2D bar in bars.Values)
                {
                    Color color = Color.HSVToRGB(((float)category) / numberOfCategories, 1, 1);
                    bar.SetColor(color);
                    bar.ApplyColor(color);
                    category++;
                }
                break;
            case ETVColorSchemes.GrayZebra:
                bool even = true;
                foreach(Bar2D bar in bars.Values)
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

    public override void UpdateETV() { }
}
