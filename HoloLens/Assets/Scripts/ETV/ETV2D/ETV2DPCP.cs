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
        var lineGen = new PCP2DLineGenerator();

        int counter = 0;
        allAxesGO = new GameObject("Axes-Set");
        FastAxisLR.transform.parent = allAxesGO.transform;
        var offset = Vector3.zero;

        // Setup nominal axes
        foreach(int attID in nominalIDs)
        {
            string attributeName = data.nomAttribNames[attID];
            var xAxis = factory2D.CreateFixedLengthAutoTickedAxis(attributeName, 1f, AxisDirection.Y, data);
            xAxis.transform.parent = allAxesGO.transform;
            offset.x = .5f*counter;

            xAxis.transform.localPosition = new Vector3(.5f * counter, 0, 0);
            PCPAxes.Add(counter, xAxis.GetComponent<Axis2D>());
            
        
            var axisComp = xAxis.GetComponent<Axis2D>();
            lineGen.CreatePureAxis(FastAxisLR, Color.white, axisComp.GetAxisBaseLocal(), axisComp.GetAxisTipLocal(), offset);

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

            offset.x = .5f * counter;
            var axisComp = xAxis.GetComponent<Axis2D>();
            lineGen.CreatePureAxis(FastAxisLR, Color.white, axisComp.GetAxisBaseLocal(), axisComp.GetAxisTipLocal(), offset);

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

            offset.x = .5f * counter;
            var axisComp = xAxis.GetComponent<Axis2D>();
            lineGen.CreatePureAxis(FastAxisLR, Color.white, axisComp.GetAxisBaseLocal(), axisComp.GetAxisTipLocal(), offset);

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

            offset.x = .5f * counter;
            var axisComp = xAxis.GetComponent<Axis2D>();
            lineGen.CreatePureAxis(FastAxisLR, Color.white, axisComp.GetAxisBaseLocal(), axisComp.GetAxisTipLocal(), offset);

            counter++;
        }

        allAxesGO.transform.localPosition = new Vector3(0, 0, -.002f);
        allAxesGO.transform.parent = Anchor.transform;

        FastAxisLR.Apply();
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
                data.colorTable[infoO], 
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

        FastStaticLR.Apply();
        lines = notNaNPrimitives.ToArray();
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
