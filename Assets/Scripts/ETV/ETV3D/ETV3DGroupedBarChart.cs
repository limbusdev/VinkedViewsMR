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
public class ETV3DGroupedBarChart : AETV3D {

    public GameObject barGroup3D;           // Populate in Editor

    private IDictionary<string, DataObject> data;
    public GameObject Anchor;
    private IList<GameObject> barGroups;
    private List<Color> colors;
    private GameObject legend;
    
    private float yRange = 1f;
    private float xRange = 1f;

    public void Init(IDictionary<string, DataObject> data)
    {
        AGraphicalPrimitiveFactory factory3D = ServiceLocator.instance.get3DFactory();
        barGroups = new List<GameObject>();
        colors = new List<Color>();

        IEnumerator<string> keyEnum = data.Keys.GetEnumerator();
        keyEnum.MoveNext();
        float[] attributeRanges = new float[data[keyEnum.Current].attributeValues.Length];

        float scaleX, scaleY, scaleZ;
        scaleX = .015f * data.Count;
        scaleY = 1f;
        scaleZ = .01f;

        this.data = data;

        for (int i = 0; i < attributeRanges.Length; i++)
        {
            attributeRanges[i] = DataProcessor.CalculateRange(data, i);
        }

        int categoryCounter = 0;

        foreach (string category in data.Keys)
        {
            GameObject barGroup = Instantiate(barGroup3D);
            barGroups.Add(barGroup);
            BarGroup3D barGroup3Dcomp = barGroup.GetComponent<BarGroup3D>();
            int attributesCount = data[category].attributeValues.Length;
            barGroup3Dcomp.Init(attributesCount, data[category].attributeValues, attributeRanges, .1f, .1f);
            barGroup.transform.parent = Anchor.transform;
            barGroup.transform.localPosition = new Vector3((categoryCounter) * 0.15f + 0.1f, 0, 0);
            barGroup3Dcomp.SetLabelCategoryText(category);
            
            categoryCounter++;
        }


        SetUpAxis();
    }

    public override void SetUpAxis()
    {
        AGraphicalPrimitiveFactory factory2D = ServiceLocator.instance.get2DFactory();

        // x-Axis
        GameObject xAxis = factory2D.CreateAxis(Color.white, "", "", new Vector3(1, 0, 0), data.Count * 0.15f + .1f, false);
        xAxis.transform.parent = Anchor.transform;

        // y-Axis
        GameObject yAxis = factory2D.CreateAxis(Color.white, "", "", new Vector3(0, 1, 0), 1.1f, true);
        yAxis.transform.parent = Anchor.transform;

        // Grid
        GameObject grid = factory2D.CreateGrid(Color.gray, new Vector3(1, 0, 0), new Vector3(0, 1, 0),
            true, 10, 0.1f, data.Count * 0.15f, false);
        grid.transform.parent = Anchor.transform;
    }


    

    public override void ChangeColoringScheme(ETVColorSchemes scheme)
    {
        foreach(GameObject go in barGroups)
        {
            BarGroup3D comp = go.GetComponent<BarGroup3D>();
            colors = comp.ChangeColoringScheme(scheme);
        }
    }

    public void SetLegendActive(bool active)
    {
        if(active)
        {
            string[] names = new string[data.Count];
            int i = 0;
            foreach(string name in data.Keys)
            {
                names[i] = name;
                i++;
            }
            legend = ServiceLocator.instance.factoryETV3Dservice.Create3DBarChartLegend(data[names[0]].attributeNames, colors.ToArray());
            legend.transform.parent = Anchor.transform;
            legend.transform.position = new Vector3((names.Length+2)*.15f,0,0);
        } else
        {
            Destroy(legend);
        }
    }

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
