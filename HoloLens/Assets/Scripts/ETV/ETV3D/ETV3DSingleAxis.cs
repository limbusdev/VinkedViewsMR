using System.Collections;
using System.Collections.Generic;
using GraphicalPrimitive;
using Model;
using UnityEngine;

public class ETV3DSingleAxis : AETV3D
{
    // ........................................................................ Editor Variables

    // ........................................................................ Private Variables

    private LoM lom;
    private int attributeID;
    private GameObject axis;
    private string attributeName;


    // ........................................................................ AETV3D Methods
    public void Init(DataSet data, int attributeID, LoM type)
    {
        base.Init(data);
        this.attributeID = attributeID;
        this.lom = type;
        
        switch(type)
        {
            case LoM.NOMINAL:
                this.attributeName = data.nomAttribNames[attributeID];
                break;
            case LoM.ORDINAL:
                this.attributeName = data.ordAttribNames[attributeID];
                break;
            case LoM.INTERVAL:
                this.attributeName = data.ivlAttribNames[attributeID];
                break;
            default: //case LevelOfMeasurement.RATIO:
                this.attributeName = data.ratAttribNames[attributeID];
                break;
        }

        SetUpAxis();

        SetAxisLabels(AxisDirection.Y, attributeName);
    }


    public override void ChangeColoringScheme(ETVColorSchemes scheme)
    {
        throw new System.NotImplementedException();
    }
    
    public override void SetUpAxis()
    {
        AddAxis(attributeName, lom, AxisDirection.Y);
    }

    public override void UpdateETV()
    {
        throw new System.NotImplementedException();
    }

    public override void DrawGraph()
    {
        throw new System.NotImplementedException();
    }

    public override AGraphicalPrimitiveFactory GetGraphicalPrimitiveFactory()
    {
        return ServiceLocator.instance.Factory3DPrimitives;
    }


    // ........................................................................ MonoBehaviour Methods

}
