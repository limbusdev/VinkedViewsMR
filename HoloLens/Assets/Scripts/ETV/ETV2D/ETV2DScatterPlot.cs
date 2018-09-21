using GraphicalPrimitive;
using Model;
using System.Collections.Generic;
using UnityEngine;

public class ETV2DScatterPlot : AETV2D
{
    public GameObject Anchor;

    private DataSet data;
    
    private ScatterDot2D[] dots;
    private string attributeNameA, attributeNameB;
    private LoM lomA, lomB;

    public void Init(DataSet data, string attributeNameA, string attributeNameB)
    {
        this.data = data;
        this.attributeNameA = attributeNameA;
        this.attributeNameB = attributeNameB;
        this.lomA = data.GetTypeOf(attributeNameA);
        this.lomB = data.GetTypeOf(attributeNameB);

        SetUpAxis();
        DrawGraph();
    }

    public override void UpdateETV()
    {
        SetUpAxis();
        DrawGraph();
    }

    public override void SetUpAxis()
    {
        AddAxis(attributeNameA, lomA, AxisDirection.X, data, Anchor);
        AddAxis(attributeNameB, lomB, AxisDirection.Y, data, Anchor);
    }

    private float GetValue(InfoObject infO, string attributeName, LoM lom, DataSet data)
    {
        float val;
        switch(lom)
        {
            case LoM.NOMINAL:
                var mN = data.dataMeasuresNominal[attributeName];
                int valN = mN.valueIDs[infO.GetNomValue(attributeName)];
                if(valN == int.MinValue)
                    val = float.NaN;
                else
                    val = valN;
                break;
            case LoM.ORDINAL:
                int valO = infO.GetOrdValue(attributeName);
                if(valO == int.MinValue)
                    val = float.NaN;
                else
                    val = valO;
                break;
            case LoM.INTERVAL:
                int valI = infO.GetOrdValue(attributeName);
                if(valI == int.MinValue)
                    val = float.NaN;
                else
                    val = valI;
                break;
            default: /* RATIO */
                val = infO.GetRatValue(attributeName);
                break;
        }

        return val;
    }

    public void DrawGraph()
    {
        var dotArray = new List<ScatterDot2D>();
        
        foreach(var infO in data.informationObjects)
        {
            float valA = GetValue(infO, attributeNameA, lomA, data);
            float valB = GetValue(infO, attributeNameB, lomB, data);
            
            if(!float.IsNaN(valA) && !float.IsNaN(valB))
            {
                var pos = new Vector3(
                    GetAxis(AxisDirection.X).TransformToAxisSpace(valA),
                    GetAxis(AxisDirection.Y).TransformToAxisSpace(valB),
                    0
                    );

                GameObject dot = ServiceLocator.instance.Factory2DPrimitives.CreateScatterDot();
                dot.transform.position = pos;
                dot.transform.parent = Anchor.transform;
                dotArray.Add(dot.GetComponent<ScatterDot2D>());

                infO.AddRepresentativeObject(attributeNameA, dot);
                infO.AddRepresentativeObject(attributeNameB, dot);
            }
        }

        dots = dotArray.ToArray();
    }

    public override void ChangeColoringScheme(ETVColorSchemes scheme)
    {
        foreach(ScatterDot2D dot in dots)
        {
            Color color = Color.HSVToRGB(.5f, 1, 1);
            dot.SetColor(color);
            dot.ApplyColor(color);
        }
    }
}






