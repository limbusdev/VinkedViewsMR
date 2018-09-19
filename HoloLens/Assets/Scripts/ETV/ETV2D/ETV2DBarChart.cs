using GraphicalPrimitive;
using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ETV2DBarChart : AETV2D
{
    public GameObject Anchor;

    private DataSet data;
    private int attID = 0;
    private string attributeName;
    private IDictionary<string, Bar2D> bars;
    private LevelOfMeasurement lom;

    public void Init(DataSet dataSet, string attributeName)
    {
        this.bounds = new float[] { 0, 0, 0 };
        this.data = dataSet;
        this.attID = dataSet.GetIDOf(attributeName);
        this.lom = dataSet.GetTypeOf(attributeName);
        this.attributeName = attributeName;
        bars = new Dictionary<string, Bar2D>();

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
            GameObject bar = CreateBar(measures.zBoundDistRange, measures.distribution[cat]);
            bars.Add(cat, bar.GetComponent<Bar2D>());

            bar.transform.localPosition = new Vector3((categoryCounter+1) * 0.15f, 0, .001f);

            bar.transform.parent = Anchor.transform;

            categoryCounter++;
        }

        foreach(var o in data.informationObjects)
        {
            string value = o.nominalAtt[attID].value;
            Bar2D bar = bars[value];
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
            GameObject bar = CreateBar(measures.zBoundDistRange, measures.distribution[cat]);
            bars.Add(catName, bar.GetComponent<Bar2D>());

            bar.transform.localPosition = new Vector3((categoryCounter + 1) * 0.15f, 0, .001f);

            bar.transform.parent = Anchor.transform;

            categoryCounter++;
        }

        foreach(var o in data.informationObjects)
        {
            string value = measures.orderedValueIDs[o.ordinalAtt[attID].value];
            Bar2D bar = bars[value];
            o.AddRepresentativeObject(attributeName, bar.gameObject);
        }
    }


    /**
     * Creates a colored bar. 
     * @param range         maximum - minimum value of this attribute
     * @param attributeID   which attribute
     * */
    private GameObject CreateBar(float range, float value)
    {
        var factory2D = ServiceLocator.instance.PrimitiveFactory2Dservice;
        var bar = factory2D.CreateBar(value, range, .1f, .1f);
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
        grid.transform.localPosition = new Vector3(0, 0, 0);
        grid.transform.parent = Anchor.transform;

        axis.Add(AxisDirection.X, xAxis);
        axis.Add(AxisDirection.Y, yAxis);
    }

    public override void UpdateETV()
    {
        throw new System.NotImplementedException();
    }
}
