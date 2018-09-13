using GraphicalPrimitive;
using Model;
using Model.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETV2DParallelCoordinatesPlot : AETV2D {

    public GameObject Anchor;

    private DataSet data;
    private int[] floatAttributeIDs;
    private int[] stringAttributeIDs;

	// Use this for initialization
	void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void Init(DataSet data, int[] floatAttributeIDs, int[] stringAttributeIDs)
    {
        this.floatAttributeIDs = floatAttributeIDs;
        this.stringAttributeIDs = stringAttributeIDs;
        this.data = data;
        UpdateETV();
    }

    public override void ChangeColoringScheme(ETVColorSchemes scheme)
    {
        base.ChangeColoringScheme(scheme);
    }

    public override void SetUpAxis()
    {
        AGraphicalPrimitiveFactory factory2D = ServiceLocator.instance.PrimitiveFactory2Dservice;

        int counter = 0;
        

        for(int i=0; i<floatAttributeIDs.Length; i++)
        {
            string attributeName = data.dataObjects[0].attributesFloat[floatAttributeIDs[i]].name;
            string attributeUnit = "";
            GameObject axis = factory2D.CreateAxis(
                Color.white, attributeName, attributeUnit, 
                AxisDirection.Y, 1f, .01f, true, true);

            axis.transform.localPosition = new Vector3(.5f * i, 0, 0);
            axis.transform.parent = Anchor.transform;
            Axis2D axis2D = axis.GetComponent<Axis2D>();
            axis2D.ticked = true;
            axis2D.min = data.dataMeasuresFloat[attributeName].zeroBoundMin;
            axis2D.max = data.dataMeasuresFloat[attributeName].zeroBoundMax;

            axis2D.CalculateTickResolution();
            axis2D.UpdateAxis();

            counter++;
        }
    }

    private void CreateLine(InformationObject o, Color color)
    {
        Graphical2DPrimitiveFactory factory = ServiceLocator.instance.PrimitiveFactory2Dservice;
        var pcpLine = factory.CreatePCPLine();
        PCPLine2D pcpComp = pcpLine.GetComponent<PCPLine2D>();
        pcpComp.lineRenderer.startColor = color;
        pcpComp.lineRenderer.endColor = color;
        pcpComp.lineRenderer.startWidth = 0.02f;
        pcpComp.lineRenderer.endWidth = 0.02f;
        int dimension = floatAttributeIDs.Length + stringAttributeIDs.Length;
        pcpComp.lineRenderer.positionCount = dimension;

        // Assemble Polyline
        Vector3[] polyline = new Vector3[dimension];
        for(int variable = 0; variable < floatAttributeIDs.Length; variable++)
        {
            GenericAttribute<float> attribute = o.attributesFloat[variable];
            polyline[variable] = new Vector3(.5f * variable, attribute.value / data.dataMeasuresFloat[attribute.name].zeroBoundRange, 0);
            o.AddRepresentativeObject(attribute.name, pcpLine);

            if(variable == 0)
            {
                pcpComp.visBridgePort.transform.localPosition = polyline[variable];
            }
        }

        pcpComp.lineRenderer.SetPositions(polyline);
        pcpLine.transform.parent = Anchor.transform;

        
    }

    public void DrawGraph()
    {
        int dimension = floatAttributeIDs.Length + stringAttributeIDs.Length;

        int i = 0;
        foreach(InformationObject o in data.dataObjects)
        {
            Color color = Color.HSVToRGB(((float)i) / data.dataObjects.Count, 1, 1);

            CreateLine(o, color);
            i++;
        }
    }

    public override void UpdateETV()
    {
        SetUpAxis();
        DrawGraph();
    }
}
