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

    private PCPLine2D[] linePrimitives;

    public void Init(DataSet data, int[] floatAttributeIDs, int[] stringAttributeIDs)
    {
        this.floatAttributeIDs = floatAttributeIDs;
        this.stringAttributeIDs = stringAttributeIDs;
        this.data = data;
        this.linePrimitives = new PCPLine2D[data.informationObjects.Count];
        UpdateETV();
    }

    public override void SetUpAxis()
    {
        AGraphicalPrimitiveFactory factory2D = ServiceLocator.instance.PrimitiveFactory2Dservice;

        int counter = 0;
        

        for(int i=0; i<floatAttributeIDs.Length; i++)
        {
            string attributeName = data.informationObjects[0].attributesFloat[floatAttributeIDs[i]].name;
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

    private PCPLine2D CreateLine(InformationObject o, Color color)
    {
        Graphical2DPrimitiveFactory factory = ServiceLocator.instance.PrimitiveFactory2Dservice;
        var pcpLine = factory.CreatePCPLine();
        var pcpComp = pcpLine.GetComponent<PCPLine2D>();
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
            
        }

        pcpComp.visBridgePort.transform.localPosition = polyline[0];
        pcpComp.lineRenderer.SetPositions(polyline);
        pcpLine.transform.parent = Anchor.transform;

        return pcpComp;
    }

    public void DrawGraph()
    {
        int dimension = floatAttributeIDs.Length + stringAttributeIDs.Length;

        int i = 0;
        foreach(InformationObject o in data.informationObjects)
        {
            linePrimitives[i] = CreateLine(o, Color.white);
            i++;
        }
    }

    public override void ChangeColoringScheme(ETVColorSchemes scheme)
    {
        switch(scheme)
        {
            case ETVColorSchemes.Rainbow:
                for(int i=0; i<linePrimitives.Length; i++)
                {
                    Color color = Color.HSVToRGB(((float)i) / linePrimitives.Length, 1, 1);
                    linePrimitives[i].SetColor(color);
                    linePrimitives[i].ApplyColor(color);
                }
                break;
            case ETVColorSchemes.SplitHSV:
                for(int i = 0; i < linePrimitives.Length; i++)
                {
                    Color color = Color.HSVToRGB((((float)i) / linePrimitives.Length)/2f+.5f, 1, 1);
                    linePrimitives[i].SetColor(color);
                    linePrimitives[i].ApplyColor(color);
                }
                break;
            default:
                break;
        }
    }

    public override void UpdateETV()
    {
        SetUpAxis();
        DrawGraph();
    }
}
