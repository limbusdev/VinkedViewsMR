/*
Copyright 2018 Georg Eckert

(Lincensed under MIT license)

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
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE 
SOFTWARE.
*/

using GraphicalPrimitive;
using System.Collections.Generic;
using UnityEngine;
using VisBridge;
using System.Linq;
using System;

/// <summary>
/// Main class for visualization generation from databases.
/// </summary>
public class VisualizationFactory : MonoBehaviour
{
    // ........................................................................ Field to populate in editor

    // Prefabs
    public GameObject ObjectBasedVisBridgePrefab;
    public GameObject NewETVPosition;
    public GameObject ObjectCollection;
    public GameObject CubeIconVariable;

    [SerializeField]
    public DataProvider dataProvider;
    public VisFactoryInteractionReceiver interactionReceiver;
    public Material lineMaterial;


    // ........................................................................ Private properties

    private IList<GameObject> activeVisualizations;
    private IList<ObjectBasedVisBridge> activeVisBridges;


    // ........................................................................ MonoBehaviour methods

    void Awake()
    {
        activeVisualizations = new List<GameObject>();
        activeVisBridges = new List<ObjectBasedVisBridge>();
    }

    void Start()
    {
        var fact2 = ServiceLocator.instance.Factory2DETV;
        var prim2Dfactory = ServiceLocator.instance.Factory2DPrimitives;
        var a1 = prim2Dfactory.CreateAutoTickedAxis("Year", AxisDirection.X, dataProvider.dataSets[0]);
        var ae1 = fact2.PutETVOnAnchor(a1);

        var a2 = prim2Dfactory.CreateAutoTickedAxis("Population", AxisDirection.X, dataProvider.dataSets[0]);
        var ae2 = fact2.PutETVOnAnchor(a2);
        ae2.transform.position = new Vector3(0, 0, 1);

        var a3 = prim2Dfactory.CreateAutoTickedAxis("Crime", AxisDirection.X, dataProvider.dataSets[1]);
        var ae3 = fact2.PutETVOnAnchor(a3);
        ae3.transform.position = new Vector3(0, 0, 2);

        var a4 = prim2Dfactory.CreateAutoTickedAxis("Weapon", AxisDirection.X, dataProvider.dataSets[1]);
        var ae4 = fact2.PutETVOnAnchor(a4);
        ae4.transform.position = new Vector3(0, 0, 3);

        var fact3 = ServiceLocator.instance.Factory3DETV;
        var prim3Dfactory = ServiceLocator.instance.Factory3DPrimitives;
        var a3d1 = prim3Dfactory.CreateAutoTickedAxis("Year", AxisDirection.X, dataProvider.dataSets[0]);
        var a3de1 = fact3.PutETVOnAnchor(a3d1);
        a3d1.transform.position = new Vector3(0, 0, -1);

        var a3d2 = prim3Dfactory.CreateAutoTickedAxis("Population", AxisDirection.X, dataProvider.dataSets[0]);
        var a3de2 = fact3.PutETVOnAnchor(a3d2);
        a3de2.transform.position = new Vector3(0, 0, -2);

        var a3d3 = prim3Dfactory.CreateAutoTickedAxis("Crime", AxisDirection.X, dataProvider.dataSets[1]);
        var a3de3 = fact3.PutETVOnAnchor(a3d3);
        a3de3.transform.position = new Vector3(0, 0, -3);

        var a3d4 = prim3Dfactory.CreateAutoTickedAxis("Weapon", AxisDirection.X, dataProvider.dataSets[1]);
        var a3de4 = fact3.PutETVOnAnchor(a3d4);
        a3de4.transform.position = new Vector3(0, 0, -4);


        var ae5 = GenerateBarChart2DFrom(1, "Crime");
        ae5.transform.position = new Vector3(0, 0, 3.5f);

        var ae7 = GenerateBarChart2DFrom(1, "Weapon");
        ae7.transform.position = new Vector3(4, 0, 3.5f);

        var ae6 = GenerateBarChart3DFrom(1, "Crime");
        ae6.transform.position = new Vector3(-4, 0, 3.5f);

        var ae8 = GenerateBarChart3DFrom(1, "Time");
        ae8.transform.position = new Vector3(-6, 0, 3.5f);

        var ae9 = GenerateBarMap3DFrom(1, new string[] {"Crime", "Weapon" });
        ae9.transform.position = new Vector3(-4, 0, 5f);

        var ae10 = GeneratePCP2DFrom(1, new string[] { "Crime", "Inside/Outside", "Weapon", "District", "Neighborhood"});
        ae10.transform.position = new Vector3(-8, 0, 5f);

        var ae12 = GeneratePCP2DFrom(1, new string[] { "Date", "Time" });
        ae12.transform.position = new Vector3(-8, 0, 9f);

        var ae11 = GeneratePCP2DFrom(0, new string[] { "Year", "Population", "Violent crime", "Rape (legacy)" });
        ae11.transform.position = new Vector3(-8, 0, 7f);

        var ae13 = GeneratePCP3DFrom(1, new string[] { "Crime", "Inside/Outside", "Weapon", "District", "Neighborhood" });
        ae13.transform.position = new Vector3(-10, 0, 8f);

        DrawVisBridgesBetweenAllRepresentativeGameObjectsOf(dataProvider.dataSets[1].informationObjects[0]);
        

    }
    

    // ........................................................................ VisBridge generation
    /// <summary>
    /// Generates VisBridge-GameObjects that connect all graphical primitives in all
    /// active visualizations that represent the given InformationObject to each other.
    /// </summary>
    /// <param name="obj">InformationObject in question</param>
    public void DrawVisBridgesBetweenAllRepresentativeGameObjectsOf(InfoObject obj)
    {
        AGraphicalPrimitive primOrigin, primTarget;


        foreach(IList<GameObject> listOrigins in obj.representativeGameObjectsByAttributeName.Values)
        {
            foreach(GameObject origin in listOrigins)
            {
                primOrigin = origin.GetComponent<AGraphicalPrimitive>();

                // For each list of GameObjects, that represent the same attribute of the given InformationObject 
                foreach(IList<GameObject> listTargets in obj.representativeGameObjectsByAttributeName.Values)
                {
                    // For every other GameObject in that list
                    foreach(GameObject target in listTargets)
                    {
                        if(origin != target)
                        {
                            primTarget = target.GetComponent<AGraphicalPrimitive>();

                            if(!(activeVisBridges.Contains(new ObjectBasedVisBridge(primOrigin, primTarget))))
                            {
                                if(primOrigin != primTarget)
                                {
                                    // Create a VisBridge between them
                                    var visBridge = CreateObjectBasedVisBridge(primOrigin, primTarget);
                                    // Add it to a list to update the bridges, when the visualizations move
                                    activeVisBridges.Add(visBridge.GetComponent<ObjectBasedVisBridge>());
                                }
                            }
                        }
                    }
                }
            }
        }


    }

    private GameObject CreateObjectBasedVisBridge(AGraphicalPrimitive origin, AGraphicalPrimitive target)
    {
        var visBridge = Instantiate(ObjectBasedVisBridgePrefab);
        visBridge.GetComponent<ObjectBasedVisBridge>().Init(origin, target);
        origin.Brush(Color.yellow);
        target.Brush(Color.yellow);

        return visBridge;
    }

    // ........................................................................ Visualization generation
    // SingleAxis3D, BarChart2D, BarChart3D, BarMap3D, PCP2D, PCP3D, ScatterXY2D, ScatterXYZ3D, LineXY2D

    /// <summary>
    /// Generates a simple axis visualization for one attribute.
    /// </summary>
    /// <param name="dataSetID"></param>
    /// <param name="variable">Attribute to be represented by the axis.</param>
    /// <returns>GameObject containing the anchored visualization.</returns>
    public GameObject GenerateSingle3DAxisFrom(int dataSetID, string variable)
    {
        try
        {
            if(!CheckIfSuitable(dataSetID, new string[] { variable }, VisualizationType.AXIS))
            {
                return new GameObject("Not Suitable");
            }

            var factory = ServiceLocator.instance.Factory3DETV;
            var ds = dataProvider.dataSets[dataSetID];
            var type = ds.GetTypeOf(variable);

            var vis = factory.CreateSingleAxis(ds, ds.GetIDOf(variable), type);
            vis = ServiceLocator.instance.Factory3DETV.PutETVOnAnchor(vis);

            vis.transform.position = NewETVPosition.transform.position;

            return vis;
        } catch(Exception e)
        {
            Debug.Log("Creation of requested Visualization for variable " + variable + " failed.");
            Debug.LogError(e.Message);
            return new GameObject("Creation Failed");
        }
    }

    /// <summary>
    /// Generates a 2D bar chart for one categorical attribute.
    /// </summary>
    /// <param name="dataSetID"></param>
    /// <param name="variable">Attribute to generate the histogram from.</param>
    /// <returns>GameObject containing the anchored visualization.</returns>
    public GameObject GenerateBarChart2DFrom(int dataSetID, string variable)
    {
        try
        {
            if(!CheckIfSuitable(dataSetID, new string[] { variable }, VisualizationType.BAR_CHART))
            {
                return new GameObject("Not Suitable");
            }

            var ds = dataProvider.dataSets[dataSetID];
            var lom = ds.GetTypeOf(variable);

            var factory = ServiceLocator.instance.Factory2DETV;
            var vis = factory.CreateETVBarChart(ds, variable);
            vis = factory.PutETVOnAnchor(vis);

            vis.transform.position = NewETVPosition.transform.position;

            return vis;
        } catch(Exception e)
        {
            Debug.Log("Creation of requested Visualization for variable " + variable + " failed.");
            Debug.LogError(e.Message);
            return new GameObject("Creation Failed");
        }
    }

    /// <summary>
    /// Generates a 3D bar chart for one categorical attribute.
    /// </summary>
    /// <param name="dataSetID"></param>
    /// <param name="variable">Attribute to generate the histogram from.</param>
    /// <returns>GameObject containing the anchored visualization.</returns>
    public GameObject GenerateBarChart3DFrom(int dataSetID, string variable)
    {
        try
        {
            if(!CheckIfSuitable(dataSetID, new string[] { variable }, VisualizationType.BAR_CHART))
            {
                return new GameObject("Not Suitable");
            }

            var ds = dataProvider.dataSets[dataSetID];
            var lom = ds.GetTypeOf(variable);

            var factory = ServiceLocator.instance.Factory3DETV;
            var vis = factory.CreateETVBarChart(ds, variable);
            vis = factory.PutETVOnAnchor(vis);

            vis.transform.position = NewETVPosition.transform.position;

            return vis;
        } catch(Exception e)
        {
            Debug.Log("Creation of requested Visualization for variable " + variable + " failed.");
            Debug.LogError(e.Message);
            return new GameObject("Creation Failed");
        }
    }

    /// <summary>
    /// Generates a 3D bar map for two categorical attributes.
    /// </summary>
    /// <param name="dataSetID"></param>
    /// <param name="variables">Attributes to generate the histogram from.</param>
    /// <returns>GameObject containing the anchored visualization.</returns>
    public GameObject GenerateBarMap3DFrom(int dataSetID, string[] variables)
    {
        try
        {
            if(!CheckIfSuitable(dataSetID, variables, VisualizationType.BAR_MAP))
            {
                return new GameObject("Not Suitable");
            }

            var ds = dataProvider.dataSets[dataSetID];
            var type1 = ds.GetTypeOf(variables[0]);
            var type2 = ds.GetTypeOf(variables[1]);

            var factory = ServiceLocator.instance.Factory3DETV;
            var vis = factory.CreateETVBarMap(ds, variables[0], variables[1]);
            vis = factory.PutETVOnAnchor(vis);

            vis.transform.position = NewETVPosition.transform.position;

            return vis;
        } catch(Exception e)
        {
            Debug.Log("Creation of requested Visualization for variable " + variables + " failed.");
            Debug.LogError(e.Message);
            return new GameObject("Creation Failed");
        }
    }

    /// <summary>
    /// Generates a 2D Parallel Coordinates Plot for n attributes.
    /// </summary>
    /// <param name="dataSetID"></param>
    /// <param name="variables">Attributes to be present in the PCP.</param>
    /// <returns>GameObject containing the anchored visualization.</returns>
    public GameObject GeneratePCP2DFrom(int dataSetID, string[] variables)
    {
        try
        {
            if(!CheckIfSuitable(dataSetID, variables, VisualizationType.PCP))
            {
                return new GameObject("Not Suitable");
            }

            var ds = dataProvider.dataSets[dataSetID];

            var factory = ServiceLocator.instance.Factory2DETV;
            var vis = factory.CreateETVParallelCoordinatesPlot(ds, variables);

            vis = factory.PutETVOnAnchor(vis);

            vis.transform.position = NewETVPosition.transform.position;

            return vis;
        } catch(Exception e)
        {
            Debug.Log("Creation of requested Visualization for variable " + variables + " failed.");
            Debug.LogError(e.Message);
            return new GameObject("Creation Failed");
        }
    }

    /// <summary>
    /// Generates a 3D Parallel Coordinates Plot for n attributes.
    /// </summary>
    /// <param name="dataSetID"></param>
    /// <param name="variables">Attributes to be present in the PCP.</param>
    /// <returns>GameObject containing the anchored visualization.</returns>
    public GameObject GeneratePCP3DFrom(int dataSetID, string[] variables)
    {
        try
        { 
            if(!CheckIfSuitable(dataSetID, variables, VisualizationType.PCP))
            {
                return new GameObject("Not Suitable");
            }

            var ds = dataProvider.dataSets[dataSetID];

            var factory = ServiceLocator.instance.Factory3DETV;
            var vis = factory.CreateETVParallelCoordinatesPlot(ds, variables);

            vis = factory.PutETVOnAnchor(vis);

            vis.transform.position = NewETVPosition.transform.position;

            return vis;
        } catch(Exception e)
        {
            Debug.Log("Creation of requested Visualization for variable " + variables + " failed.");
            Debug.LogError(e.Message);
            return new GameObject("Creation Failed");
        }
    }

    /// <summary>
    /// Generates a 2D Scatterplot for 2 attributes.
    /// </summary>
    /// <param name="dataSetID"></param>
    /// <param name="variables">Attributes to be present in the scatterplot.</param>
    /// <returns>GameObject containing the anchored visualization.</returns>
    public GameObject GenerateScatterplot2DFrom(int dataSetID, string[] variables)
    {
        try
        {
            if(!CheckIfSuitable(dataSetID, variables, VisualizationType.SCATTER_PLOT_2D))
            {
                return new GameObject("Not Suitable");
            }

            var ds = dataProvider.dataSets[dataSetID];

            var factory = ServiceLocator.instance.Factory2DETV;
            var vis = factory.CreateETVScatterPlot(ds, variables);

            vis = factory.PutETVOnAnchor(vis);

            vis.transform.position = NewETVPosition.transform.position;

            return vis;
        } catch(Exception e)
        {
            Debug.Log("Creation of requested Visualization for variable " + variables + " failed.");
            Debug.LogError(e.Message);
            return new GameObject("Creation Failed");
        }
    }

    /// <summary>
    /// Generates a 3D Scatterplot for 3 attributes.
    /// </summary>
    /// <param name="dataSetID"></param>
    /// <param name="variables">Attributes to be present in the scatterplot.</param>
    /// <returns>GameObject containing the anchored visualization.</returns>
    public GameObject GenerateScatterplot3DFrom(int dataSetID, string[] variables)
    {
        try
        {
            if(!CheckIfSuitable(dataSetID, variables, VisualizationType.SCATTER_PLOT_2D))
            {
                return new GameObject("Not Suitable");
            }

            return new GameObject();
        } catch(Exception e)
        {
            Debug.Log("Creation of requested Visualization for variable " + variables + " failed.");
            Debug.LogError(e.Message);
            return new GameObject("Creation Failed");
        }
    }

    /// <summary>
    /// Generates a 2D line plot for 2 attributes.
    /// </summary>
    /// <param name="dataSetID"></param>
    /// <param name="variables">Attributes to be present in the line plot.</param>
    /// <returns>GameObject containing the anchored visualization.</returns>
    public GameObject GenerateLineplot2DFrom(int dataSetID, string[] variables)
    {
        try
        {
            if(!CheckIfSuitable(dataSetID, variables, VisualizationType.LINE_CHART))
            {
                return new GameObject("Not Suitable");
            }

            return new GameObject();
        } catch(Exception e)
        {
            Debug.Log("Creation of requested Visualization for variable " + variables + " failed.");
            Debug.LogError(e.Message);
            return new GameObject("Creation Failed");
        }
    }


    // ........................................................................ Helper methods

    /// <summary>
    /// List which visualizations are suitable for the combination of provided attributes.
    /// </summary>
    /// <param name="dataSetID">ID of DataSet to use.</param>
    /// <param name="attNames">Names of the attributes to visualize.</param>
    /// <returns>List of suitable visualizations.</returns>
    public string[] ListPossibleVisualizations(int dataSetID, string[] attNames)
    {
        int[] nomIDs, ordIDs, ivlIDs, ratIDs;

        DataProcessor.ExtractAttributeIDs(dataProvider.dataSets[dataSetID], attNames, out nomIDs, out ordIDs, out ivlIDs, out ratIDs);

        var count = new Vector4(nomIDs.Length, ordIDs.Length, ivlIDs.Length, ratIDs.Length);

        if(count == new Vector4(1, 0, 0, 0))
        {
            return new string[] { "SingleAxis3D", "BarChart2D", "BarChart3D" };
        } else if(count == new Vector4(2, 0, 0, 0))
        {
            return new string[] { "BarMap3D", "PCP2D", "PCP3D" };
        } else if(count == new Vector4(0, 1, 0, 0))
        {
            return new string[] { "SingleAxis3D", "BarChart2D", "BarChart3D" };
        } else if(count == new Vector4(0, 2, 0, 0))
        {
            return new string[] { "BarMap3D", "PCP2D", "PCP3D" };
        } else if(count == new Vector4(0, 0, 1, 0))
        {
            return new string[] { "SingleAxis3D" };
        } else if(count == new Vector4(0, 0, 2, 0))
        {
            return new string[] { "PCP2D", "PCP3D" };
        } else if(count == new Vector4(0, 0, 0, 1))
        {
            return new string[] { "SingleAxis3D" };
        } else if(count == new Vector4(0, 0, 0, 2))
        {
            return new string[] { "PCP2D", "PCP3D", "LineXY2D", "ScatterXY2D" };
        } else if(count == new Vector4(0, 0, 0, 3))
        {
            return new string[] { "PCP2D", "PCP3D", "ScatterXYZ3D" };
        } else if(count == new Vector4(1, 1, 0, 0)) {
            return new string[] { "BarMap3D", "PCP2D", "PCP3D", "ScatterXYZ2D" };
        } else
        {
            return new string[] { "PCP2D", "PCP3D" };
        }
    }

    /// <summary>
    /// Checks whether the given attributes of the given DataSet are suitable for
    /// the visualization type in question
    /// </summary>
    /// <param name="dataSetID">ID of the data set</param>
    /// <param name="attributes">attributes in question</param>
    /// <param name="visType">planned visualization type</param>
    /// <returns>if suitable</returns>
    public bool CheckIfSuitable(int dataSetID, string[] attributes, VisualizationType visType)
    {
        // SingleAxis3D, BarChart2D, BarChart3D, BarMap3D, PCP2D, PCP3D, ScatterXY2D, ScatterXYZ3D, LineXY2D
        var suitables = ListPossibleVisualizations(dataSetID, attributes);

        bool suitable;

        switch(visType)
        {
            case VisualizationType.AXIS:
                suitable = (suitables.Contains("SingleAxis3D"));
                break;
            case VisualizationType.BAR_CHART:
                suitable = (suitables.Contains("BarChart2D") || suitables.Contains("BarChart3D"));
                break;
            case VisualizationType.BAR_MAP:
                suitable = (suitables.Contains("BarMap3D"));
                break;
            case VisualizationType.LINE_CHART:
                suitable = (suitables.Contains("LineXY2D"));
                break;
            case VisualizationType.SCATTER_PLOT_2D:
                suitable = (suitables.Contains("ScatterXY2D"));
                break;
            case VisualizationType.SCATTER_PLOT_3D:
                suitable = (suitables.Contains("ScatterXYZ3D"));
                break;
            case VisualizationType.PCP:
                suitable = (suitables.Contains("PCP2D") || suitables.Contains("PCP3D"));
                break;
            default:
                suitable = false;
                break;
        }

        return suitable;
    }

    public void AddNewVisualization(GameObject visualization)
    {
        if(visualization.GetComponent<ETVAnchor>() != null)
        {
            activeVisualizations.Add(visualization);
        } else
        {
            Debug.LogWarning("Given GameObject ist not an anchored visualization!");
        }
    }

    
}

public enum VisualizationType
{
    AXIS, BAR_CHART, BAR_MAP, LINE_CHART, SCATTER_PLOT_2D, SCATTER_PLOT_3D, PCP
}
