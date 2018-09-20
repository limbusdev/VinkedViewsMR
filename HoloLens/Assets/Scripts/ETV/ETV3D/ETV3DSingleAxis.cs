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
    private string name;


    // ........................................................................ AETV3D Methods
    public void Init(DataSet data, int attributeID, LoM type)
    {
        this.data = data;
        this.attributeID = attributeID;
        this.type = type;

        float attributeRange = DataProcessor.RatioAttribute.CalculateRange(data.informationObjects, attributeID);

        
        SetUpAxis();

        switch(type)
        {
            case LoM.NOMINAL:
                this.name = data.nomAttributes[attributeID];
                break;
            case LoM.ORDINAL:
                this.name = data.ordAttributes[attributeID];
                break;
            case LoM.INTERVAL:
                this.name = data.ivlAttributes[attributeID];
                break;
            default: //case LevelOfMeasurement.RATIO:
                this.name = data.ratAttributes[attributeID];
                break;
        }

        SetAxisLabels(AxisDirection.Y, name, "");


    }


    public override void ChangeColoringScheme(ETVColorSchemes scheme)
    {
        throw new System.NotImplementedException();
    }
    
    public override void SetUpAxis()
    {
        AGraphicalPrimitiveFactory factory3D = ServiceLocator.instance.Factory3DPrimitives;

        // y-Axis
        axis = factory3D.CreateAxis(Color.white, this.name, "", AxisDirection.Y, 1f, .01f, true, true);
        Axis3D axis3D = axis.GetComponent<Axis3D>();
        axis3D.max = 0;
        axis3D.min = 0;
        axis3D.CalculateTickResolution();
        
        axis.transform.localPosition = new Vector3();
        axis.transform.parent = Anchor.transform;
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
