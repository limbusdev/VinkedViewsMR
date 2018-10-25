using ETV;
using GraphicalPrimitive;
using Model;
using System.Collections.Generic;
using UnityEngine;

public class ETV2DPCP : AETV2D
{
    // ........................................................................ PARAMETERS
    // Hook
    private IPCPLineGenerator pcpLineGen;
    
    private int[]
            nominalIDs,
            ordinalIDs,
            intervalIDs,
            ratioIDs;

    private GameObject allAxesGO;

    private IDictionary<int, AAxis> PCPAxes;

    private PCPLine2D[] lines;


    // ........................................................................ CONSTRUCTOR / INITIALIZER

    public void Init(DataSet data, int[] nominalIDs, int[] ordinalIDs, int[] intervalIDs, int[] ratioIDs)
    {
        base.Init(data);
        this.pcpLineGen = new PCP2DLineGenerator();
        
        this.nominalIDs = nominalIDs;
        this.ordinalIDs = ordinalIDs;
        this.intervalIDs = intervalIDs;
        this.ratioIDs = ratioIDs;

        SetUpAxes();
        DrawGraph();
    }

    public override void SetUpAxes()
    {
        PCPAxes = new Dictionary<int, AAxis>();
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

    // ........................................................................ DRAW CALLS

    public override void DrawGraph()
    {
        var notNaNPrimitives = new List<PCPLine2D>();
        
        foreach(var infoO in data.infoObjects)
        {
            var line = pcpLineGen.CreateLine(
                infoO, 
                FastStaticLR, 
                Color.white, 
                data, 
                nominalIDs, 
                ordinalIDs, 
                intervalIDs, 
                ratioIDs, 
                PCPAxes, 
                false);
            if(line != null)
            {

                line.transform.parent = Anchor.transform;
                notNaNPrimitives.Add(line);
            }
        }

        this.FastStaticLR.Apply();
        lines = notNaNPrimitives.ToArray();
    }

    public override void ChangeColoringScheme(ETVColorSchemes scheme)
    {
        switch(scheme)
        {
            case ETVColorSchemes.Rainbow:
                for(int i=0; i<lines.Length; i++)
                {
                    Color color = Color.HSVToRGB(((float)i) / lines.Length, 1, 1);
                    lines[i].SetColor(color);
                    lines[i].ApplyColor(color);
                }
                break;
            case ETVColorSchemes.SplitHSV:
                for(int i = 0; i < lines.Length; i++)
                {
                    Color color = Color.HSVToRGB((((float)i) / lines.Length)/2f+.5f, 1, 1);
                    lines[i].SetColor(color);
                    lines[i].ApplyColor(color);
                }
                break;
            default:
                break;
        }
    }
    
    public override AGraphicalPrimitiveFactory GetGraphicalPrimitiveFactory()
    {
        return ServiceLocator.PrimitivePlant2D();
    }

    public override void UpdateETV()
    {
        // Nothing
    }
}
