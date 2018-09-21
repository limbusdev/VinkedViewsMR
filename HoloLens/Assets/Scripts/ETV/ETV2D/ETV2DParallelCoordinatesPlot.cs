using ETV;
using GraphicalPrimitive;
using Model;
using System.Collections.Generic;
using UnityEngine;

public class ETV2DParallelCoordinatesPlot : AETV2D
{
    // Hook
    private IPCPLineGenerator pcpLineGenerator;
    
    private int[]
            nominalIDs,
            ordinalIDs,
            intervalIDs,
            ratioIDs;

    private GameObject allAxesGO;

    private IDictionary<int, Axis2D> PCPAxes;

    private PCPLine2D[] linePrimitives;

    public void Init(DataSet data, int[] nominalIDs, int[] ordinalIDs, int[] intervalIDs, int[] ratioIDs)
    {
        base.Init(data);
        this.pcpLineGenerator = new PCP2DLineGenerator();
        
        this.nominalIDs = nominalIDs;
        this.ordinalIDs = ordinalIDs;
        this.intervalIDs = intervalIDs;
        this.ratioIDs = ratioIDs;

        SetUpAxis();
        DrawGraph();
    }

    public override void SetUpAxis()
    {
        PCPAxes = new Dictionary<int, Axis2D>();
        AGraphicalPrimitiveFactory factory2D = ServiceLocator.instance.Factory2DPrimitives;

        int counter = 0;
        allAxesGO = new GameObject("Axes-Set");

        // Setup nominal axes
        foreach(int attID in nominalIDs)
        {
            string attributeName = data.nomAttribNames[attID];
            var xAxis = factory2D.CreateFixedLengthAutoTickedAxis(attributeName, 1f, AxisDirection.Y, data);
            xAxis.transform.parent = allAxesGO.transform;
            xAxis.transform.localPosition = new Vector3(.5f * counter, 0, 0);
            PCPAxes.Add(counter, xAxis.GetComponent<Axis2D>());

            counter++;
        }

        // Setup ordinal axes
        foreach(int attID in ordinalIDs)
        {
            string attributeName = data.ordAttribNames[attID];
            var xAxis = factory2D.CreateFixedLengthAutoTickedAxis(attributeName, 1f, AxisDirection.Y, data);
            xAxis.transform.parent = allAxesGO.transform;
            xAxis.transform.localPosition = new Vector3(.5f * counter, 0, 0);
            PCPAxes.Add(counter, xAxis.GetComponent<Axis2D>());

            counter++;
        }

        // Setup interval axes
        foreach(int attID in intervalIDs)
        {
            string attributeName = data.ivlAttribNames[attID];
            var xAxis = factory2D.CreateAutoTickedAxis(attributeName, AxisDirection.Y, data);
            xAxis.transform.parent = allAxesGO.transform;
            xAxis.transform.localPosition = new Vector3(.5f * counter, 0, 0);
            PCPAxes.Add(counter, xAxis.GetComponent<Axis2D>());

            counter++;
        }


        // Setup ratio axes
        foreach(int attID in ratioIDs)
        {
            string attributeName = data.ratAttribNames[attID];
            var xAxis = factory2D.CreateAutoTickedAxis(attributeName, AxisDirection.Y, data);
            xAxis.transform.parent = allAxesGO.transform;
            xAxis.transform.localPosition = new Vector3(.5f * counter, 0, 0);
            PCPAxes.Add(counter, xAxis.GetComponent<Axis2D>());

            counter++;
        }

        allAxesGO.transform.localPosition = new Vector3(0, 0, -.002f);
        allAxesGO.transform.parent = Anchor.transform;
    }

    public override void DrawGraph()
    {
        var notNaNPrimitives = new List<PCPLine2D>();
        
        foreach(var infoO in data.infoObjects)
        {
            var line = pcpLineGenerator.CreateLine(infoO, Color.white, data, nominalIDs, ordinalIDs, intervalIDs, ratioIDs, PCPAxes);
            if(line != null)
            {
                line.transform.parent = Anchor.transform;
                notNaNPrimitives.Add(line);
            }
        }

        linePrimitives = notNaNPrimitives.ToArray();
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

    public override AGraphicalPrimitiveFactory GetGraphicalPrimitiveFactory()
    {
        return ServiceLocator.instance.Factory2DPrimitives;
    }
}
