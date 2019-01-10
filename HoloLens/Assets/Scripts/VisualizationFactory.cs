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

using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Networking;

/// <summary>
/// Main class for visualization generation from databases.
/// </summary>
public class VisualizationFactory : NetworkBehaviour
{
    // ........................................................................ Fields to populate in editor

    // Prefabs
    public Transform NewETVPlaceHolder;
    public GameObject ObjectCollection;
    public GameObject CubeIconVariable;
    public GameObject NetworkAnchorPrefab;

    [SerializeField]
    public DataProvider dataProvider;
    public Material lineMaterial;
    public VisFactoryInteractionReceiver interactionReceiver;



    // ........................................................................ Private properties

    private IList<GameObject> activeVisualizations;
    


    // ........................................................................ MonoBehaviour methods

    void Awake()
    {
        activeVisualizations = new List<GameObject>();
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
            if(!CheckIfSuitable(dataSetID, new string[] { variable }, VisType.SingleAxis))
            {
                return new GameObject(VisType.SingleAxis + "not suitable for " + variable);
            }

            var factory = Services.ETVFactory3D();
            var ds = dataProvider.dataSets[dataSetID];
            var vis = factory.CreateSingleAxis(ds, variable).gameObject;
            
            vis = factory.PutETVOnAnchor(vis);

            vis.transform.position = NewETVPlaceHolder.position;
            AddNetworkAnchor(vis, dataSetID, new string[] { variable }, VisType.SingleAxis);
            
            return vis;
        } 
        catch(Exception e)
        {
            Debug.LogError("Creation of requested Visualization for variable " + variable + " failed.");
            Debug.LogException(e);
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
            if(!CheckIfSuitable(dataSetID, new string[] { variable }, VisType.BarChart2D))
            {
                return new GameObject(VisType.BarChart2D + "not suitable for " + variable);
            }

            var ds = dataProvider.dataSets[dataSetID];

            var factory = Services.instance.Factory2DETV;
            var vis = factory.CreateBarChart(ds, variable).gameObject;
            vis = factory.PutETVOnAnchor(vis);

            vis.transform.position = NewETVPlaceHolder.position;
            AddNetworkAnchor(vis, dataSetID, new string[] { variable }, VisType.BarChart2D);

            return vis;
        } catch(Exception e)
        {
            Debug.LogError("Creation of requested Visualization for variable " + variable + " failed.");
            Debug.LogException(e);
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
            if(!CheckIfSuitable(dataSetID, new string[] { variable }, VisType.BarChart3D))
            {
                return new GameObject(VisType.BarChart3D + "not suitable for " + variable);
            }

            var ds = dataProvider.dataSets[dataSetID];

            var factory = Services.instance.Factory3DETV;
            var vis = factory.CreateBarChart(ds, variable).gameObject;
            vis = factory.PutETVOnAnchor(vis);

            vis.transform.position = NewETVPlaceHolder.position;
            AddNetworkAnchor(vis, dataSetID, new string[] { variable }, VisType.BarChart3D);

            return vis;
        } catch(Exception e)
        {
            Debug.LogError("Creation of requested Visualization for variable " + variable + " failed.");
            Debug.LogException(e);
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
            if(!CheckIfSuitable(dataSetID, variables, VisType.BarMap3D))
            {
                return new GameObject(VisType.BarMap3D + "not suitable for " +  variables[0] + " and " + variables[1]);
            }

            var ds = dataProvider.dataSets[dataSetID];

            var factory = Services.instance.Factory3DETV;
            var vis = factory.CreateETVBarMap(ds, variables[0], variables[1]).gameObject;
            vis = factory.PutETVOnAnchor(vis);

            vis.transform.position = NewETVPlaceHolder.position;
            AddNetworkAnchor(vis, dataSetID, variables, VisType.BarMap3D);

            return vis;
        } catch(Exception e)
        {
            Debug.LogError("Creation of requested Visualization for variable " + variables + " failed.");
            Debug.LogException(e);
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
            if(!CheckIfSuitable(dataSetID, variables, VisType.PCP2D))
            {
                return new GameObject(VisType.PCP2D + "not suitable for " + variables);
            }

            var ds = dataProvider.dataSets[dataSetID];

            var factory = Services.instance.Factory2DETV;
            var vis = factory.CreatePCP(ds, variables).gameObject;

            vis = factory.PutETVOnAnchor(vis);

            vis.transform.position = NewETVPlaceHolder.position;
            AddNetworkAnchor(vis, dataSetID, variables, VisType.PCP2D);

            return vis;
        } catch(Exception e)
        {
            Debug.LogError("Creation of requested Visualization for variable " + variables + " failed.");
            Debug.LogException(e);
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
            if(!CheckIfSuitable(dataSetID, variables, VisType.PCP3D))
            {
                return new GameObject(VisType.PCP3D + "not suitable for " + variables);
            }

            var ds = dataProvider.dataSets[dataSetID];

            var factory = Services.instance.Factory3DETV;
            var vis = factory.CreatePCP(ds, variables).gameObject;

            vis = factory.PutETVOnAnchor(vis);

            vis.transform.position = NewETVPlaceHolder.position;
            AddNetworkAnchor(vis, dataSetID, variables, VisType.PCP3D);

            return vis;
        } catch(Exception e)
        {
            var allVars = "";
            foreach(var s in variables)
                allVars += s;
            Debug.LogError("Creation of requested Visualization for variable " + allVars + " failed.");
            Debug.LogException(e);
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
            if(!CheckIfSuitable(dataSetID, variables, VisType.ScatterPlot2D))
            {
                return new GameObject(VisType.ScatterPlot2D + "not suitable for " + variables);
            }

            var ds = dataProvider.dataSets[dataSetID];

            var factory = Services.instance.Factory2DETV;
            var vis = factory.CreateScatterplot(ds, variables).gameObject;

            vis = factory.PutETVOnAnchor(vis);

            vis.transform.position = NewETVPlaceHolder.position;
            AddNetworkAnchor(vis, dataSetID, variables, VisType.ScatterPlot2D);

            return vis;
        } catch(Exception e)
        {
            Debug.LogError("Creation of requested Visualization for variable " + variables + " failed.");
            Debug.LogException(e);
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
            if(!CheckIfSuitable(dataSetID, variables, VisType.ScatterPlot3D))
            {
                return new GameObject(VisType.ScatterPlot3D + "not suitable for " + variables);
            }

            var ds = dataProvider.dataSets[dataSetID];

            var factory = Services.instance.Factory3DETV;
            var vis = factory.CreateScatterplot(ds, variables).gameObject;

            vis = factory.PutETVOnAnchor(vis);

            vis.transform.position = NewETVPlaceHolder.position;
            AddNetworkAnchor(vis, dataSetID, variables, VisType.ScatterPlot3D);

            return vis;
        } catch(Exception e)
        {
            Debug.LogError("Creation of requested Visualization for variable " + variables + " failed.");
            Debug.LogException(e);
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
            if(!CheckIfSuitable(dataSetID, variables, VisType.LineChartXY2D))
            {
                return new GameObject(VisType.LineChartXY2D + "not suitable for " + variables);
            }

            var ds = dataProvider.dataSets[dataSetID];

            var factory = Services.instance.Factory2DETV;
            var vis = factory.CreateLineChart(ds, variables[0], variables[1]).gameObject;

            vis = factory.PutETVOnAnchor(vis);

            vis.transform.position = NewETVPlaceHolder.position;
            AddNetworkAnchor(vis, dataSetID, variables, VisType.LineChartXY2D);

            return vis;

        } catch(Exception e)
        {
            Debug.LogError("Creation of requested Visualization for variable " + variables + " failed.");
            Debug.LogException(e);
            return new GameObject("Creation Failed");
        }
    }

    public void AddNetworkAnchor(GameObject etv, int dataSetID, string[] attributes, VisType visType)
    {
        if(isServer)
        {
            var networkAnchor = Instantiate(NetworkAnchorPrefab);
            if(networkAnchor.GetComponent<NetworkAnchor>() != null)
            {
                NetworkServer.Spawn(networkAnchor);
                networkAnchor.GetComponent<NetworkAnchor>().Init(dataSetID, attributes, visType);
                networkAnchor.GetComponent<NetworkAnchor>().ETV = etv;
            }
        }

    }

    public GameObject GenerateVisFrom(int dataSetID, string variable, VisType visType)
    {
        return GenerateVisFrom(dataSetID, new string[] { variable }, visType);
    }


    public GameObject GenerateVisFrom(int dataSetID, string[] variables, VisType visType)
    {
        try
        {
            if(!CheckIfSuitable(dataSetID, variables, visType))
            {
                return new GameObject(visType + " not suitable for " + variables);
            }

            // SWITCH create vis
            GameObject vis;

            switch(visType)
            {
                case VisType.SingleAxis:
                    vis = GenerateSingle3DAxisFrom(dataSetID, variables[0]);
                    break;
                case VisType.ScatterPlot2D:
                    vis = GenerateScatterplot2DFrom(dataSetID, variables);
                    break;
                case VisType.ScatterPlot3D:
                    vis = GenerateScatterplot3DFrom(dataSetID, variables);
                    break;
                case VisType.PCP2D:
                    vis = GeneratePCP2DFrom(dataSetID, variables);
                    break;
                case VisType.PCP3D:
                    vis = GeneratePCP2DFrom(dataSetID, variables);
                    break;
                case VisType.LineChartXY2D:
                    vis = GenerateLineplot2DFrom(dataSetID, variables);
                    break;
                case VisType.BarChart2D:
                    vis = GenerateBarChart2DFrom(dataSetID, variables[0]);
                    break;
                case VisType.BarChart3D:
                    vis = GenerateBarChart2DFrom(dataSetID, variables[0]);
                    break;
                case VisType.BarMap3D:
                    vis = GenerateBarMap3DFrom(dataSetID, variables);
                    break;
                default:
                    vis = new GameObject("Failed");
                    vis = Services.ETVFactory2D().PutETVOnAnchor(vis);
                    vis.transform.position = NewETVPlaceHolder.transform.position;
                    break;
            }
            
            GameManager.gameManager.PersistETV(vis, dataSetID, variables, visType);

            return vis;
        } 
        catch(Exception e)
        {
            Debug.Log("Creation of requested Visualization failed.");
            Debug.LogError(e.Message);
            Debug.LogError(e.StackTrace);
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
    public VisType[] ListPossibleVisualizations(int dataSetID, string[] attNames)
    {
        int[] nomIDs, ordIDs, ivlIDs, ratIDs;

        AttributeProcessor.ExtractAttributeIDs(dataProvider.dataSets[dataSetID], attNames, out nomIDs, out ordIDs, out ivlIDs, out ratIDs);

        var count = new Vector4(nomIDs.Length, ordIDs.Length, ivlIDs.Length, ratIDs.Length);

        // Only one categorical attribute
        if(count.x + count.y == 1 && count.z + count.w == 0)
            return new VisType[] {VisType.SingleAxis, VisType.BarChart2D, VisType.BarChart3D };

        // Only one attribute
        if(count.x + count.y + count.z + count.w == 1)
            return new VisType[] { VisType.SingleAxis };

        // Two categorical attributes
        else if(count.x + count.y == 2 && count.z + count.w == 0)
            return new VisType[] { VisType.BarMap3D, VisType.PCP2D, VisType.PCP3D, VisType.ScatterPlot2D };

        else if(count.x + count.y + count.z + count.w == 2)
            return new VisType[] { VisType.PCP2D, VisType.PCP3D, VisType.ScatterPlot2D };

        else if(count.z == 1 && count.w == 1)
            return new VisType[] { VisType.PCP2D, VisType.PCP3D, VisType.ScatterPlot2D, VisType.LineChartXY2D };

        else if(count.z + count.w == 2)
            return new VisType[] { VisType.PCP2D, VisType.PCP3D, VisType.ScatterPlot2D };

        else if(count.x + count.y + count.z + count.w == 3)
            return new VisType[] { VisType.PCP2D, VisType.PCP3D, VisType.ScatterPlot3D };

        else
            return new VisType[] { VisType.PCP2D, VisType.PCP3D };
    }

    /// <summary>
    /// Checks whether the given attributes of the given DataSet are suitable for
    /// the visualization type in question
    /// </summary>
    /// <param name="dataSetID">ID of the data set</param>
    /// <param name="attributes">attributes in question</param>
    /// <param name="visType">planned visualization type</param>
    /// <returns>if suitable</returns>
    public bool CheckIfSuitable(int dataSetID, string[] attributes, VisType visType)
    {
        return CheckIfSuitable(dataSetID, attributes, new VisType[] { visType });
    }
    public bool CheckIfSuitable(int dataSetID, string[] attributes, VisType[] visTypes)
    {
        // SingleAxis3D, BarChart2D, BarChart3D, BarMap3D, PCP2D, PCP3D, ScatterXY2D, ScatterXYZ3D, LineXY2D
        var suitables = ListPossibleVisualizations(dataSetID, attributes);

        bool suitable = true;

        foreach(var t in visTypes)
        {
            suitable &= suitables.Contains(t);
        }

        if(!suitable)
        {
            var atts = "";
            var vists = "";
            foreach(var att in attributes) atts += (att + ", ");
            foreach(var vist in visTypes) vists += (vist.ToString() + ", ");
            Debug.LogWarning("Visualization type(s) " + vists + " not suitable for attribute(s) " + atts);
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

public enum VisType
{
    SingleAxis, BarChart2D, BarChart3D, BarMap3D, LineChartXY2D, ScatterPlot2D, ScatterPlot3D, PCP2D, PCP3D
}
