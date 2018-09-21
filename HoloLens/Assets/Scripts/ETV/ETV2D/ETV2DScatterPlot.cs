using GraphicalPrimitive;
using Model;
using System.Collections.Generic;
using UnityEngine;

public class ETV2DScatterPlot : AETV2D
{
    public GameObject Anchor;

    private DataSet data;
    
    private ScatterDot2D[] dots;
    private string attributeA, attributeB;
    private LoM lomA, lomB;

    public void Init(DataSet data, string attributeNameA, string attributeNameB)
    {
        this.data = data;
        this.attributeA = attributeNameA;
        this.attributeB = attributeNameB;
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
        AddAxis(attributeA, lomA, AxisDirection.X, data, Anchor);
        AddAxis(attributeB, lomB, AxisDirection.Y, data, Anchor);
    }
    
    public void DrawGraph()
    {
        var dotArray = new List<ScatterDot2D>();
        
        foreach(var infO in data.infoObjects)
        {
            float valA = data.GetValue(infO, attributeA, lomA);
            float valB = data.GetValue(infO, attributeB, lomB);
            
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

                infO.AddRepresentativeObject(attributeA, dot);
                infO.AddRepresentativeObject(attributeB, dot);
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






