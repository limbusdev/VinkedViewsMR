using GraphicalPrimitive;
using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETV2DLineChart : AETV2D {

    public GameObject Anchor;
    private DataSetLines data;
    private float rangeX, rangeY;
    private float minX = 0;
    private float maxX = 1;
    private float minY = 0;
    private float maxY = 1;
    private float ticksX = 1;
    private float ticksY = 1;

	// Use this for initialization
	void Start ()
    {
        Vector2[] kaninchen = new Vector2[] {
           new Vector2(1,2),
           new Vector2(2,4),
           new Vector2(3,8),
           new Vector2(4,16),
           new Vector2(5,32)
           };
        LineObject lo = new LineObject("Kaninchen", kaninchen);
        

        Vector2[] wallabees = new Vector2[]
        {
           new Vector2(1,30),
           new Vector2(2,20),
           new Vector2(3,25),
           new Vector2(4,10),
           new Vector2(5,3)
        };
        LineObject lo2 = new LineObject("Wallabies", wallabees);

        DataSetLines dsl = new DataSetLines(new LineObject[] { lo, lo2 }, "Zeit", "a", "Tiere", "tausend");

        Init(dsl,0,6,0,40,1,5);
        UpdateETV();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init(DataSetLines data, float minX, float maxX, float minY, float maxY, float ticksX, float ticksY)
    {
        this.data = data;
        this.minX = minX;
        this.maxX = maxX;
        this.minY = minY;
        this.maxY = maxY;
        rangeX = maxX - minX;
        rangeY = maxY - minY;
        this.ticksX = ticksX;
        this.ticksY = ticksY;
    }

    public override void ChangeColoringScheme(ETVColorSchemes scheme) { }

    public override void UpdateETV()
    {
        SetUpAxis();
        DrawGraph();
    }

    public override void SetUpAxis()
    {
        AGraphicalPrimitiveFactory factory2D = ServiceLocator.instance.PrimitiveFactory2Dservice;

        // x-Axis
        GameObject xAxis = factory2D.CreateAxis(Color.white, data.variableX, data.unitX, AxisDirection.X, 1f, .01f, true, true);
        xAxis.transform.localPosition = new Vector3(0, 0, -.001f);
        xAxis.transform.parent = Anchor.transform;
        Axis2D xAxis2D = xAxis.GetComponent<Axis2D>();
        xAxis2D.ticked = true;
        xAxis2D.tickResolution = ticksX;
        xAxis2D.min = minX;
        xAxis2D.max = maxX;
        xAxis2D.UpdateAxis();
        bounds[0] += 1f + .5f;

        // y-Axis
        GameObject yAxis = factory2D.CreateAxis(Color.white, data.variableY, data.unitY, AxisDirection.Y, 1f, .01f, true, true);
        yAxis.transform.localPosition = new Vector3(0, 0, -.001f);
        yAxis.transform.parent = Anchor.transform;
        Axis2D yAxis2D = yAxis.GetComponent<Axis2D>();
        yAxis2D.ticked = true;
        yAxis2D.tickResolution = ticksY;
        yAxis2D.min = minY;
        yAxis2D.max = maxY;
        yAxis2D.UpdateAxis();


        bounds[1] += 1f + .5f;
        
    }

    public void DrawGraph()
    {
        int counter = 0;
        foreach (LineObject lo in data.lineObjects)
        {
            Color color = Color.HSVToRGB(((float)counter) / data.lineObjects.Length, 1, 1);

            GameObject renderedLine = new GameObject(lo.nominalValue);
            var lineRend = renderedLine.AddComponent<LineRenderer>();
            lineRend.useWorldSpace = false;
            lineRend.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
            lineRend.startColor = color;
            lineRend.endColor = color;
            lineRend.startWidth = .02f;
            lineRend.endWidth = .02f;
            lineRend.positionCount = lo.values.Length;
            lineRend.alignment = LineAlignment.TransformZ;

            // Assemble Polyline
            Vector3[] polyline = new Vector3[lo.values.Length];
            for(int i=0; i<lo.values.Length; i++)
            {
                polyline[i] = new Vector3(lo.values[i].x/rangeX, lo.values[i].y/rangeY, 0);
            }

            lineRend.SetPositions(polyline);

            renderedLine.transform.parent = Anchor.transform;

            GameObject label = ServiceLocator.instance.PrimitiveFactory2Dservice.CreateLabel(lo.nominalValue);
            label.GetComponent<TextMesh>().color = color;
            label.transform.localPosition = polyline[0];
            label.transform.parent = Anchor.transform;

            counter++;
        }
    }
}
