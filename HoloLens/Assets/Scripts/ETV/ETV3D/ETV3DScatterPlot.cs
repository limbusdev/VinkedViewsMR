using System.Collections;
using System.Collections.Generic;
using GraphicalPrimitive;
using Model;
using UnityEngine;

public class ETV3DScatterPlot : AETV3D
{
    private ScatterDot3D[] dots;
    private string attributeA, attributeB, attributeC;
    private LoM lomA, lomB, lomC;
    private bool initialized = false;
    

    public void Init(DataSet data, string attA, string attB, string attC)
    {
        base.Init(data);
        this.attributeA = attA;
        this.attributeB = attB;
        this.attributeC = attC;
        this.lomA = data.GetTypeOf(attA);
        this.lomB = data.GetTypeOf(attB);
        this.lomC = data.GetTypeOf(attC);

        SetUpAxis();
        DrawGraph();
        this.initialized = true;
    }
    
    public override void SetUpAxis()
    {
        AddAxis(attributeA, lomA, AxisDirection.X);
        AddAxis(attributeB, lomB, AxisDirection.Y);
        AddAxis(attributeC, lomC, AxisDirection.Z);
    }

    public override void DrawGraph()
    {
        var dotArray = new List<ScatterDot3D>();

        foreach(var infO in data.infoObjects)
        {
            float valA = data.GetValue(infO, attributeA, lomA);
            float valB = data.GetValue(infO, attributeB, lomB);
            float valC = data.GetValue(infO, attributeC, lomC);

            if(!float.IsNaN(valA) && !float.IsNaN(valB) && !float.IsNaN(valC))
            {
                var pos = new Vector3(
                    GetAxis(AxisDirection.X).TransformToAxisSpace(valA),
                    GetAxis(AxisDirection.Y).TransformToAxisSpace(valB),
                    GetAxis(AxisDirection.Z).TransformToAxisSpace(valC)
                    );

                GameObject dot = ServiceLocator.instance.Factory3DPrimitives.CreateScatterDot();
                dot.transform.position = pos;
                dot.transform.parent = Anchor.transform;
                dotArray.Add(dot.GetComponent<ScatterDot3D>());

                infO.AddRepresentativeObject(attributeA, dot);
                infO.AddRepresentativeObject(attributeB, dot);
                infO.AddRepresentativeObject(attributeC, dot);
            }
        }
        dots = dotArray.ToArray();
    }

    public override void ChangeColoringScheme(ETVColorSchemes scheme)
    {
        foreach(ScatterDot3D dot in dots)
        {
            Color color = Color.HSVToRGB(.5f, 1, 1);
            dot.SetColor(color);
            dot.ApplyColor(color);
        }
    }

    public override void UpdateETV()
    {
        SetUpAxis();
        DrawGraph();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(initialized)
        {
            // Let point sprites face camera
            foreach(var dot in dots)
            {
                var camera = GameObject.FindGameObjectWithTag("MainCamera");
                dot.gameObject.transform.LookAt(camera.transform);
            }
        }
    }

    public override AGraphicalPrimitiveFactory GetGraphicalPrimitiveFactory()
    {
        return ServiceLocator.instance.Factory2DPrimitives;
    }
}
