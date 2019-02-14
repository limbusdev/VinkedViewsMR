/*
Copyright 2019 Georg Eckert (MIT License)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to
deal in the Software without restriction, including without limitation the
rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
sell copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
IN THE SOFTWARE.
*/
using Model;
using UnityEngine;

/// <summary>
/// Script for cubic variable icons. Used in the visualization factory shelf.
/// </summary>
public class CubeIconVariable : MonoBehaviour
{
    // ........................................................................ Inner Classes
    public class VisTypeButton : MonoBehaviour
    {
        public VisType visType = VisType.SingleAxis3D;
    }

    // ........................................................................ Public Editor Fields

    [SerializeField]
    public TextMesh text;
    public Transform IconPosition;
    public Transform BasePlatform;
    public GameObject[] IconValues;
    public GameObject HoloButtonPrefab;
    public GameObject visButtonsAnchor;
    public Material highlightMaterial;
    public Material defaultMaterial;


    // ........................................................................ Public Properties

    public bool selected = false;
    public int IconType = 0;
    public string[] varNames;
    public LoM[] varTypes;
    public int dimension;
    public int dataSetID { get; private set; }


    // ........................................................................ Private Properties

    /// <summary>
    /// Initializer for ND-Visualizations
    /// </summary>
    /// <param name="variableNames">Variables to represent.</param>
    /// <param name="variableTypes">Level of Measurement of the given variables.</param>
    /// <param name="dataSetID">ID of the DataSet to use.</param>
    /// <param name="name">Name to show on the button sign.</param>
    public void InitMulti(string[] variableNames, LoM[] variableTypes, int dataSetID, string name)
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
    public void Init(string[] variableNames, LoM[] variableTypes, int dataSetID)
    {
        this.gameObject.name = "CubeIconVariable";

        varNames = variableNames;
        varTypes = variableTypes;

        this.dataSetID = dataSetID;
        dimension = variableNames.Length;
        int iconNum = 0;
        int counterNumericVars = 0;
        int counterCategoricalVars = 0;

        foreach(LoM t in variableTypes)
        {
            if (t == LoM.RATIO || t == LoM.INTERVAL) counterNumericVars++;
            else counterCategoricalVars++;
        }

        bool allVarsNumeric = (counterNumericVars == dimension);
        bool allVarsCategoric = (counterCategoricalVars == dimension);

        // Chose 3D Icon
        switch (dimension)
        {
            case 1:
                iconNum = (variableTypes[0] == LoM.RATIO || variableTypes[0] == LoM.INTERVAL) ? 0 : 1;
                break;
            case 2:
                if      (allVarsNumeric)   iconNum = 2;
                else if (allVarsCategoric) iconNum = 3;
                else                       iconNum = 4;
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
            name = (((name.Length > 12 ? name.Substring(0, 11) : name)) + "\n");
        }
        else
        {
            foreach (string v in variableNames)
            {
                name += (((v.Length > 12 ? v.Substring(0,11) : v)) + "\n");
            }
        }

        text.text = name;
    }

    
    public void Select()
    {
        selected = !selected;

        if(selected)
        {
            text.text = varNames[0] + "\n(Selected)";
            Highlight();
        } else
        {
            text.text = varNames[0];
            Unhighlight();
        }
    }

    public void Highlight()
    {
        BasePlatform.GetComponent<MeshRenderer>().material = highlightMaterial;
    }

    public void Unhighlight()
    {
        BasePlatform.GetComponent<MeshRenderer>().material = defaultMaterial;
    }
}
