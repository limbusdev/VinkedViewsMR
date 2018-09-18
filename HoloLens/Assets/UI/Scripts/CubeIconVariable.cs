using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeIconVariable : MonoBehaviour
{
    public TextMesh text;

    [SerializeField]
    public Transform IconPosition;

    [SerializeField]
    public GameObject[] IconValues;
    
    private string[] IconKeys = {"1DOrdinal", "1DNominal", "2DOrdinal", "2DNominal", "2DMixed", "3DOrdinal", "3DNominal", "3D2Ord1Nom", "3D1Ord2Nom", "MultiD" };

    public int IconType = 0;
    public string[] VariableNames;
    public string[] UnitNames;
    public LevelOfMeasurement[] VariableTypes;
    public int dimension;

    public void Init(string[] variableNames, string[] unitNames, LevelOfMeasurement[] variableTypes)
    {
        VariableNames = variableNames;
        UnitNames = unitNames;
        VariableTypes = variableTypes;

        dimension = variableNames.Length;
        int iconNum = 0;
        int counterNumericVars = 0;
        int counterCategoricalVars = 0;

        foreach(LevelOfMeasurement t in variableTypes)
        {
            if (t == LevelOfMeasurement.RATIO || t == LevelOfMeasurement.INTERVAL) counterNumericVars++;
            else counterCategoricalVars++;
        }

        bool allVarsNumeric = (counterNumericVars == dimension);
        bool allVarsCategoric = (counterCategoricalVars == dimension);

        // Chose 3D Icon
        switch (dimension)
        {
            case 1:
                iconNum = (variableTypes[0] == LevelOfMeasurement.RATIO || variableTypes[0] == LevelOfMeasurement.INTERVAL) ? 0 : 1;
                break;
            case 2:
                if      (allVarsNumeric) iconNum = 2;
                else if (allVarsCategoric) iconNum = 3;
                else                          iconNum = 4;
                break;
            case 3:
                if (allVarsNumeric) iconNum = 5;
                else if (allVarsCategoric) iconNum = 6;
                else if (counterNumericVars == 2 && counterCategoricalVars == 1) iconNum = 7;
                else if (counterNumericVars == 1 && counterCategoricalVars == 2) iconNum = 8;
                else iconNum = 0;
                break;
            default: // case n:
                iconNum = 9;
                break;
        }

        GameObject icon = Instantiate(IconValues[iconNum]);
        icon.transform.parent = IconPosition;
        icon.transform.localPosition = new Vector3();

        string name = "";

        if (variableNames.Length == 1)
        {
            name = variableNames[0];
        }
        else
        {
            foreach (string v in variableNames)
            {
                name += (v + "\n");
            }
        }

        text.text = name;
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
