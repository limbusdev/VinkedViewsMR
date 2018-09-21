using System.Collections;
using System.Collections.Generic;
using GraphicalPrimitive;
using Model;
using UnityEngine;

public class ETV3DSingleAxis : AETV3D
{
    // ........................................................................ Editor Variables
    [SerializeField]
    public GameObject Anchor;


    // ........................................................................ Private Variables
    private DataSet data;
    private LoM type;
    private int attributeID;
    private GameObject axis;
    private string attributeName;


    // ........................................................................ AETV3D Methods
    public void Init(DataSet data, int attributeID, LoM type)
    {
        this.data = data;
        this.attributeID = attributeID;
        this.type = type;
        
        

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
        AGraphicalPrimitiveFactory factory3D = ServiceLocator.instance.Factory3DPrimitives;

        // y-Axis
        axis = factory3D.CreateAutoTickedAxis(attributeName, AxisDirection.Y, data);
        axis.transform.parent = Anchor.transform;
        axis.transform.localPosition = new Vector3();

        axes.Add(AxisDirection.Y, axis);
    }

    public override void UpdateETV()
    {
        throw new System.NotImplementedException();
    }


    // ........................................................................ MonoBehaviour Methods
    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
