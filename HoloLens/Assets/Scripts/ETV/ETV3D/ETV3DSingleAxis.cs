using System.Collections;
using System.Collections.Generic;
using GraphicalPrimitive;
using UnityEngine;

public class ETV3DSingleAxis : AETV3D
{
    // ........................................................................ Editor Variables
    [SerializeField]
    public GameObject Anchor;


    // ........................................................................ Private Variables
    private DataSet data;
    private LevelOfMeasurement type;
    private int attributeID;
    private GameObject axis;


    // ........................................................................ AETV3D Methods
    public void Init(DataSet data, int attributeID)
    {
        this.data = data;
        this.attributeID = attributeID;

        float attributeRange = DataProcessor.FloatAttribute.CalculateRange(data.informationObjects, attributeID);

        
        SetUpAxis();

        SetAxisLabels(AxisDirection.Y, data.Variables[attributeID], data.Units[attributeID]);

    }


    public override void ChangeColoringScheme(ETVColorSchemes scheme)
    {
        throw new System.NotImplementedException();
    }

    public override void SetAxisLabels(AxisDirection axisDirection, string axisVariable, string axisUnit)
    {
        axis.GetComponent<Axis3D>().labelUnitText = axisUnit;
        axis.GetComponent<Axis3D>().labelVariableText = axisVariable;
        axis.GetComponent<Axis3D>().UpdateAxis();
    }

    public override void SetUpAxis()
    {
        AGraphicalPrimitiveFactory factory3D = ServiceLocator.instance.PrimitiveFactory3Dservice;

        // y-Axis
        axis = factory3D.CreateAxis(Color.white, data.Variables[attributeID], data.Units[attributeID], AxisDirection.Y, 1f, .01f, true, true);
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
