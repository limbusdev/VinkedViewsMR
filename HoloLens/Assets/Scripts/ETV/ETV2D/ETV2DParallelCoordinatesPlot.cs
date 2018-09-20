using GraphicalPrimitive;
using Model;
using Model.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ETV2DParallelCoordinatesPlot : AETV2D {

    public GameObject Anchor;

    private DataSet data;
    private int[]
            nominalIDs,
            ordinalIDs,
            intervalIDs,
            ratioIDs;

    private GameObject allAxesGO;

    private PCPLine2D[] linePrimitives;

    public void Init(DataSet data, int[] nominalIDs, int[] ordinalIDs, int[] intervalIDs, int[] ratioIDs)
    {
        this.data = data;
        this.nominalIDs = nominalIDs;
        this.ordinalIDs = ordinalIDs;
        this.intervalIDs = intervalIDs;
        this.ratioIDs = ratioIDs;

        this.linePrimitives = new PCPLine2D[data.informationObjects.Count];

        SetUpAxis();
        DrawGraph();
    }

    public override void SetUpAxis()
    {
        AGraphicalPrimitiveFactory factory2D = ServiceLocator.instance.Factory2DPrimitives;

        int counter = 0;
        allAxesGO = new GameObject("Axes-Set");

        // Setup nominal axes
        for(int i=0; i<nominalIDs.Length; i++)
        {
            string attributeName = data.informationObjects[0].nominalAtt[nominalIDs[i]].name;
            GameObject axis = factory2D.CreateAxis(
                Color.white,attributeName,"",AxisDirection.Y,1f,.01f,true,true);
            axis.transform.localPosition = new Vector3(.5f * counter, 0, 0);
            axis.transform.parent = allAxesGO.transform;
            Axis2D axis2D = axis.GetComponent<Axis2D>();
            axis2D.ticked = true;
            axis2D.min = 0;
            axis2D.max = data.dataMeasuresNominal[attributeName].numberOfUniqueValues;

            axis2D.CalculateTickResolution();
            axis2D.UpdateAxis();

            counter++;
        }

        // Setup ordinal axes
        for(int i = 0; i < ordinalIDs.Length; i++)
        {
            string attributeName = data.informationObjects[0].ordinalAtt[ordinalIDs[i]].name;
            GameObject axis = factory2D.CreateAxis(
                Color.white, attributeName, "", AxisDirection.Y, 1f, .01f, true, true);
            axis.transform.localPosition = new Vector3(.5f * counter, 0, 0);
            axis.transform.parent = allAxesGO.transform;
            Axis2D axis2D = axis.GetComponent<Axis2D>();
            axis2D.ticked = true;
            axis2D.min = data.dataMeasuresOrdinal[attributeName].min;
            axis2D.max = data.dataMeasuresOrdinal[attributeName].max;

            axis2D.CalculateTickResolution();
            axis2D.UpdateAxis();

            counter++;
        }

        // Setup interval axes
        for(int i = 0; i < intervalIDs.Length; i++)
        {
            string attributeName = data.informationObjects[0].intervalAtt[intervalIDs[i]].name;
            GameObject axis = factory2D.CreateAxis(
                Color.white, attributeName, "", AxisDirection.Y, 1f, .01f, true, true);
            axis.transform.localPosition = new Vector3(.5f * counter, 0, 0);
            axis.transform.parent = allAxesGO.transform;
            Axis2D axis2D = axis.GetComponent<Axis2D>();
            axis2D.ticked = true;
            axis2D.min = data.dataMeasuresInterval[attributeName].min;
            axis2D.max = data.dataMeasuresInterval[attributeName].max;

            axis2D.CalculateTickResolution();
            axis2D.UpdateAxis();

            counter++;
        }


        // Setup ratio axes
        for(int i=0; i<ratioIDs.Length; i++)
        {
            string attributeName = data.informationObjects[0].ratioAtt[ratioIDs[i]].name;
            string attributeUnit = "";
            GameObject axis = factory2D.CreateAxis(
                Color.white, attributeName, attributeUnit, 
                AxisDirection.Y, 1f, .01f, true, true);

            axis.transform.localPosition = new Vector3(.5f * counter, 0, 0);
            axis.transform.parent = allAxesGO.transform;
            Axis2D axis2D = axis.GetComponent<Axis2D>();
            axis2D.ticked = true;
            axis2D.min = data.dataMeasuresRatio[attributeName].zeroBoundMin;
            axis2D.max = data.dataMeasuresRatio[attributeName].zeroBoundMax;

            axis2D.CalculateTickResolution();
            axis2D.UpdateAxis();

            counter++;
        }

        allAxesGO.transform.localPosition = new Vector3(0, 0, -.002f);
        allAxesGO.transform.parent = Anchor.transform;
    }

    private PCPLine2D CreateLine(InfoObject o, Color color)
    {
        Graphical2DPrimitiveFactory factory = ServiceLocator.instance.Factory2DPrimitives;
        var pcpLine = factory.CreatePCPLine();
        var pcpComp = pcpLine.GetComponent<PCPLine2D>();
        pcpComp.lineRenderer.startColor = color;
        pcpComp.lineRenderer.endColor = color;
        pcpComp.lineRenderer.startWidth = 0.02f;
        pcpComp.lineRenderer.endWidth = 0.02f;
        int dimension = ratioIDs.Length + nominalIDs.Length + ordinalIDs.Length + intervalIDs.Length;
        pcpComp.lineRenderer.positionCount = dimension;

        // Assemble Polyline
        Vector3[] polyline = new Vector3[dimension];

        int counter = 0;
        for(int variable = 0; variable < nominalIDs.Length; variable++)
        {
            GenericAttribute<string> attribute = o.nominalAtt[variable];
            polyline[counter] = new Vector3(.5f * counter, 
                ((float)data.dataMeasuresNominal[attribute.name].valueIDs[attribute.value]) / 
                data.dataMeasuresNominal[attribute.name].numberOfUniqueValues, 0);
            o.AddRepresentativeObject(attribute.name, pcpLine);
            counter++;
        }

        for(int variable = 0; variable < ordinalIDs.Length; variable++)
        {
            GenericAttribute<int> attribute = o.ordinalAtt[variable];
            polyline[counter] = new Vector3(.5f * counter, ((float)attribute.value - data.dataMeasuresInterval[attribute.name].min) / data.dataMeasuresOrdinal[attribute.name].range, 0);
            o.AddRepresentativeObject(attribute.name, pcpLine);
            counter++;
        }

        for(int variable = 0; variable < intervalIDs.Length; variable++)
        {
            GenericAttribute<int> attribute = o.intervalAtt[variable];
            polyline[counter] = new Vector3(.5f * counter, ((float)attribute.value - data.dataMeasuresInterval[attribute.name].min) / data.dataMeasuresInterval[attribute.name].range, 0);
            o.AddRepresentativeObject(attribute.name, pcpLine);
            counter++;
        }

        for(int variable = 0; variable < ratioIDs.Length; variable++)
        {
            GenericAttribute<float> attribute = o.ratioAtt[variable];
            polyline[counter] = new Vector3(.5f * counter, attribute.value / data.dataMeasuresRatio[attribute.name].zeroBoundRange, 0);
            o.AddRepresentativeObject(attribute.name, pcpLine);
            counter++;
        }

        pcpComp.visBridgePort.transform.localPosition = polyline[0];
        pcpComp.lineRenderer.SetPositions(polyline);
        pcpLine.transform.parent = Anchor.transform;

        return pcpComp;
    }

    public void DrawGraph()
    {
        int dimension = ratioIDs.Length + nominalIDs.Length;

        int i = 0;
        foreach(InfoObject o in data.informationObjects)
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
