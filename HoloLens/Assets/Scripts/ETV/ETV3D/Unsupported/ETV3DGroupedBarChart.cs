using System;
using System.Collections.Generic;
using GraphicalPrimitive;
using Model;
using UnityEngine;

[Obsolete("ETV3DGroupedBarChart is not supported right now. It might get support at some point in time.")]
public class ETV3DGroupedBarChart : AETV3D
{
    public GameObject Anchor;
    public GameObject barGroup3D;           // Populate in Editor

    private DataSet data;
    private int[] stringAttributeIDs;

    private IDictionary<int, IDictionary<string, Bar3D>> bars;
    private List<Color> colors;
    private GameObject legend;

    private IDictionary<AxisDirection, GameObject> axis;

    
    public void Init(DataSet data, int[] stringAttributeIDs)
    {
        this.data = data;
        this.stringAttributeIDs = stringAttributeIDs;

        AGraphicalPrimitiveFactory factory3D = ServiceLocator.instance.PrimitiveFactory3Dservice;
        bars = new Dictionary<int, IDictionary<string, Bar3D>>();
        colors = new List<Color>();

        float max = 0;
        for(int i=0; i<stringAttributeIDs.Length; i++)
        {
            int key = stringAttributeIDs[i];
            string sKey = data.nomAttributes[key];
            float newMax = data.dataMeasuresNominal[sKey].zBoundDistMax;
            if(max < newMax)
            {
                max = newMax; // Get value for scaling
            }
        }

        SetUpAxis();
    }

    public override void SetUpAxis()
    {
        /*
        AGraphicalPrimitiveFactory factory2D = ServiceLocator.instance.PrimitiveFactory2Dservice;

        // x-Axis
        GameObject xAxis = factory2D.CreateAxis(Color.white, "", "", AxisDirection.X, data.Count * 0.15f + .1f, .01f, false, false);
        xAxis.transform.parent = Anchor.transform;

        // y-Axis
        GameObject yAxis = factory2D.CreateAxis(Color.white, "", "", AxisDirection.Y, 1.1f, .01f, true, true);
        yAxis.transform.parent = Anchor.transform;

        // Grid
        GameObject grid = factory2D.CreateGrid(Color.gray, AxisDirection.X, AxisDirection.Y,
            true, 10, 0.1f, data.Count * 0.15f, false);
        grid.transform.parent = Anchor.transform;
        */
    }


    

    public override void ChangeColoringScheme(ETVColorSchemes scheme)
    {
        /*
        foreach(GameObject go in barGroups)
        {
            BarGroup3D comp = go.GetComponent<BarGroup3D>();
            colors = comp.ChangeColoringScheme(scheme);
        }
        */
    }

    public void SetLegendActive(bool active)
    {/*
        if(active)
        {
            string[] names = new string[data.Count];
            int i = 0;
            foreach(string name in data.Keys)
            {
                names[i] = name;
                i++;
            }
            legend = ServiceLocator.instance.ETV3DFactoryService.Create3DBarChartLegend(data[names[0]].attributeNames, colors.ToArray());
            legend.transform.parent = Anchor.transform;
            legend.transform.position = new Vector3((names.Length+2)*.15f,0,0);
        } else
        {
            Destroy(legend);
        }*/
    }

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void SetAxisLabels(AxisDirection axisDirection, string axisVariable, string axisUnit)
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateETV()
    {
        throw new System.NotImplementedException();
    }
}
