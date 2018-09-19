using HoloToolkit.Unity.Buttons;
using HoloToolkit.Unity.InputModule;
using HoloToolkit.Unity.Receivers;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for cubic variable icons. Used in the visualization factory shelf.
/// </summary>
public class CubeIconVariable : InteractionReceiver
{
    // ........................................................................ Public Editor Fields
    
    [SerializeField]
    public TextMesh text;
    public Transform IconPosition;
    public Transform BasePlatform;
    public GameObject[] IconValues;
    public GameObject HoloButtonPrefab;
    public GameObject visButtonsAnchor;


    // ........................................................................ Public Properties

    public int IconType = 0;
    public string[] varNames;
    public LevelOfMeasurement[] varTypes;
    public int dimension;
    public int dataSetID { get; private set; }


    // ........................................................................ Private Properties

    private string[] IconKeys = {
        "1DOrdinal",
        "1DNominal",
        "2DOrdinal",
        "2DNominal",
        "2DMixed",
        "3DOrdinal",
        "3DNominal",
        "3D2Ord1Nom",
        "3D1Ord2Nom",
        "MultiD"
    };
    
    private bool clicked = false;       // whether the icon has been clicked
    private bool initialized = false;   // whether the sub buttons have been initialized already

    /// <summary>
    /// Initializer for ND-Visualizations
    /// </summary>
    /// <param name="variableNames">Variables to represent.</param>
    /// <param name="variableTypes">Level of Measurement of the given variables.</param>
    /// <param name="dataSetID">ID of the DataSet to use.</param>
    /// <param name="name">Name to show on the button sign.</param>
    public void InitMulti(string[] variableNames, LevelOfMeasurement[] variableTypes, int dataSetID, string name)
    {
        Init(variableNames, variableTypes, dataSetID);
        text.text = name;
    }

    /// <summary>
    /// Initializer for 1D, 2D and 3D-Visualizations
    /// </summary>
    /// <param name="variableNames">Variables to represent.</param>
    /// <param name="variableTypes">Level of Measurement of the given variables.</param>
    /// <param name="dataSetID">ID of the DataSet to use.</param>
    public void Init(string[] variableNames, LevelOfMeasurement[] variableTypes, int dataSetID)
    {
        this.gameObject.name = "CubeIconVariable";

        varNames = variableNames;
        varTypes = variableTypes;

        this.dataSetID = dataSetID;
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

    /// <summary>
    /// Initializes the sub buttons to choose possible visualizations from.
    /// </summary>
    public void ShowButtons()
    {
        if(!clicked && !initialized)
        {
            var lr = visButtonsAnchor.GetComponent<LineRenderer>();

            lr.startWidth = .01f;
            lr.endWidth = .01f;
            
            visButtonsAnchor.SetActive(true);
            string[] viss = ServiceLocator.instance.visualizationFactory.ListPossibleVisualizations(dataSetID, varNames);

            lr.positionCount = viss.Length * 2;
            var poss = new List<Vector3>();

            for(int i = 0; i < viss.Length; i++)
            {
                var key = viss[i];
                var button = Instantiate(HoloButtonPrefab);
                button.transform.parent = visButtonsAnchor.transform;
                button.name = key;
                button.GetComponent<CompoundButtonText>().Text = key;
                button.transform.localPosition = new Vector3(.15f * i, 0, 0);
                button.transform.localRotation = Quaternion.Euler(Vector3.zero);
                interactables.Add(button);

                poss.Add(button.transform.position);
                poss.Add(BasePlatform.position);
            }

            lr.SetPositions(poss.ToArray());

            clicked = true;
            initialized = true;
        } else if(!clicked && initialized)
        {
            visButtonsAnchor.SetActive(true);
        }
    }

    /// <summary>
    /// Hides the sub buttons.
    /// </summary>
    public void HideButtons()
    {
        clicked = false;
        visButtonsAnchor.SetActive(false);
    }

    /// <summary>
    /// Button interaction callback.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="eventData"></param>
    protected override void InputDown(GameObject obj, InputEventData eventData)
    {
        // SingleAxis3D, BarChart2D, BarChart3D, BarMap3D, PCP2D, PCP3D, ScatterXY2D, ScatterXYZ3D, LineXY2D
        switch(obj.name)
        {
            case "SingleAxis3D":
                ServiceLocator.instance.visualizationFactory.GenerateSingle3DAxisFrom(dataSetID, varNames[0]);
                break;
            case "BarChart2D":
                ServiceLocator.instance.visualizationFactory.GenerateBarChart2DFrom(dataSetID, varNames[0]);
                break;
            case "BarChart3D":
                ServiceLocator.instance.visualizationFactory.GenerateBarChart3DFrom(dataSetID, varNames[0]);
                break;
            case "BarMap3D":
                ServiceLocator.instance.visualizationFactory.GenerateBarMap3DFrom(dataSetID, varNames);
                break;
            case "PCP2D":
                ServiceLocator.instance.visualizationFactory.GeneratePCP2DFrom(dataSetID, varNames);
                break;
            case "PCP3D":
                ServiceLocator.instance.visualizationFactory.GeneratePCP3DFrom(dataSetID, varNames);
                break;
            case "ScatterXY2D":
                ServiceLocator.instance.visualizationFactory.GenerateScatterplot2DFrom(dataSetID, varNames);
                break;
            case "ScatterXYZ3D":
                ServiceLocator.instance.visualizationFactory.GenerateScatterplot3DFrom(dataSetID, varNames);
                break;
            case "LineXY2D":
                ServiceLocator.instance.visualizationFactory.GenerateLineplot2DFrom(dataSetID, varNames);
                break;
            default:
                break;
        }
    }
    
}
