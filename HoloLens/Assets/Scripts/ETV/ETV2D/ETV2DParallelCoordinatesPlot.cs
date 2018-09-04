using GraphicalPrimitive;
using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETV2DParallelCoordinatesPlot : AETV2D {

    public GameObject Anchor;

    private DataSetMultiDimensionalPoints data;
    private float[] ticks;

	// Use this for initialization
	void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void Init(DataSetMultiDimensionalPoints data, float[] ticks)
    {
        this.data = data;
        this.ticks = ticks;

        UpdateETV();
    }

    public override void ChangeColoringScheme(ETVColorSchemes scheme)
    {
        base.ChangeColoringScheme(scheme);
    }

    public override void SetUpAxis()
    {
        AGraphicalPrimitiveFactory factory2D = ServiceLocator.instance.PrimitiveFactory2Dservice;

        for(int i=0; i<data.dimension; i++)
        {
            GameObject axis = factory2D.CreateAxis(
                Color.white, data.variables[i], data.units[i], 
                AxisDirection.Y, 1f, .01f, true, true);
            axis.transform.localPosition = new Vector3(.5f * i, 0, 0);
            axis.transform.parent = Anchor.transform;
            Axis2D axis2D = axis.GetComponent<Axis2D>();
            axis2D.ticked = true;
            axis2D.tickResolution = ticks[i];
            axis2D.min = data.zeroBoundMins[i];
            axis2D.max = data.zeroBoundMaxs[i];
            axis2D.UpdateAxis();
        }
    }

    public void DrawGraph()
    {
        for(int dataObject=0; dataObject < data.dataPointCount; dataObject++)
        {
            Color color = Color.HSVToRGB(((float)dataObject) / data.dataPointCount, 1, 1);

            GameObject renderedLine = new GameObject("LineObject " + dataObject);
            var lineRend = renderedLine.AddComponent<LineRenderer>();
            lineRend.useWorldSpace = false;
            lineRend.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
            lineRend.startColor = color;
            lineRend.endColor = color;
            lineRend.startWidth = .02f;
            lineRend.endWidth = .02f;
            lineRend.positionCount = data.dimension;
            lineRend.alignment = LineAlignment.TransformZ;

            // Assemble Polyline
            Vector3[] polyline = new Vector3[data.dimension];
            for (int variable = 0; variable < data.dimension; variable++)
            {
                polyline[variable] = new Vector3(.5f*variable, data.nDpoints[variable][dataObject]/data.zeroBoundRanges[variable], 0);
            }

            lineRend.SetPositions(polyline);

            renderedLine.transform.parent = Anchor.transform;

        }
    }

    public override void UpdateETV()
    {
        SetUpAxis();
        DrawGraph();
    }
}
