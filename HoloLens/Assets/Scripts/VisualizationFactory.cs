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
using Model;
using ETV;

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
        TESTSetupFBI();
    }

    private void TESTSetupFBI()
    {
        try
        {
            var etvMan = ServiceLocator.instance.etvManager;
            var fact2 = ServiceLocator.instance.Factory2DETV;
            var prim2Dfactory = ServiceLocator.instance.Factory2DPrimitives;

            var etvYearPopulationCrimePCP2D = GeneratePCP2DFrom(0, new string[] { "Year", "Population", "Violent crime", "Rape (legacy)" });
            etvMan.AutoPlaceETV(etvYearPopulationCrimePCP2D);

            //var etvYearPopulationCrimePCP3D = GeneratePCP3DFrom(0, new string[] { "Year", "Population", "Violent crime", "Rape (legacy)" });
            //etvMan.AutoPlaceETV(etvYearPopulationCrimePCP3D);

            //var etvYearMurderScatterplot2D = GenerateScatterplot2DFrom(0, new string[] { "Year", "Murder/MS." });
            //etvMan.AutoPlaceETV(etvYearMurderScatterplot2D);

            //var etvYearCrimeMurderScatterplot3D = GenerateScatterplot3DFrom(0, new string[] { "Year", "Murder/MS.", "Violent crime" });
            //etvMan.AutoPlaceETV(etvYearCrimeMurderScatterplot3D);

            //var etvTimeRape = GenerateLineplot2DFrom(0, new string[] { "Year", "Rape (legacy)"});
            //etvMan.AutoPlaceETV(etvTimeRape);

            //var etvTimeMurder = GenerateLineplot2DFrom(0, new string[] { "Year", "Murder/MS." });
            //etvMan.AutoPlaceETV(etvTimeMurder);

            //var etvAxisMurder = GenerateSingle3DAxisFrom(0, "Murder/MS.");
            //etvAxisMurder.transform.position = new Vector3(1,0,0);

            //var etvAxisPopulation = GenerateSingle3DAxisFrom(0, "Population");
            //etvAxisPopulation.transform.position = new Vector3(1.5f, 0, 0);
            

            //DrawVisBridgesBetweenAllRepresentativeGameObjectsOf(dataProvider.dataSets[0].infoObjects[7]);

        } catch(Exception e)
        {
            Debug.LogError("TESTSetupFBI failed.");
            Debug.LogError(e.StackTrace);
        }
    }

    private void TESTPlaceAxes()
    {
        var etvMan = ServiceLocator.instance.etvManager;
        var fact2 = ServiceLocator.instance.Factory2DETV;
        var prim2Dfactory = ServiceLocator.instance.Factory2DPrimitives;

        var a1 = prim2Dfactory.CreateAutoTickedAxis("Year", AxisDirection.X, dataProvider.dataSets[0]);
        var ae1 = fact2.PutETVOnAnchor(a1);

        etvMan.AutoPlaceETV(ae1);

        var a2 = prim2Dfactory.CreateAutoTickedAxis("Population", AxisDirection.X, dataProvider.dataSets[0]);
        var ae2 = fact2.PutETVOnAnchor(a2);

        etvMan.AutoPlaceETV(ae2);

        var a3 = prim2Dfactory.CreateAutoTickedAxis("Crime", AxisDirection.X, dataProvider.dataSets[1]);
        var ae3 = fact2.PutETVOnAnchor(a3);

        etvMan.AutoPlaceETV(ae3);

        var a4 = prim2Dfactory.CreateAutoTickedAxis("Weapon", AxisDirection.X, dataProvider.dataSets[1]);
        var ae4 = fact2.PutETVOnAnchor(a4);

        etvMan.AutoPlaceETV(ae4);

        var fact3 = ServiceLocator.instance.Factory3DETV;
        var prim3Dfactory = ServiceLocator.instance.Factory3DPrimitives;
        var a3d1 = prim3Dfactory.CreateAutoTickedAxis("Year", AxisDirection.X, dataProvider.dataSets[0]);
        var a3de1 = fact3.PutETVOnAnchor(a3d1);

        etvMan.AutoPlaceETV(a3de1);

        var a3d2 = prim3Dfactory.CreateAutoTickedAxis("Population", AxisDirection.X, dataProvider.dataSets[0]);
        var a3de2 = fact3.PutETVOnAnchor(a3d2);

        etvMan.AutoPlaceETV(a3de2);

        var a3d3 = prim3Dfactory.CreateAutoTickedAxis("Crime", AxisDirection.X, dataProvider.dataSets[1]);
        var a3de3 = fact3.PutETVOnAnchor(a3d3);

        etvMan.AutoPlaceETV(a3de3);

        var a3d4 = prim3Dfactory.CreateAutoTickedAxis("Weapon", AxisDirection.X, dataProvider.dataSets[1]);
        var a3de4 = fact3.PutETVOnAnchor(a3d4);

        etvMan.AutoPlaceETV(a3de4);


    }


    // ........................................................................ VisBridge generation
    private IDictionary<int,IDictionary<InfoObject, GameObject>> visBridges;
    private IDictionary<int, MultiVisBridge> multiBridgesByDataSet;
    private bool visBridgeSystemInitialized = false;

    public void InitVisBridgeSystem()
    {
        visBridges = new Dictionary<int,IDictionary<InfoObject, GameObject>>();
        multiBridgesByDataSet = new Dictionary<int, MultiVisBridge>();
        visBridgeSystemInitialized = true;

        for(int i = 0; i < dataProvider.dataSets.Length; i++)
        {
            visBridges.Add(i, new Dictionary<InfoObject, GameObject>());
            multiBridgesByDataSet.Add(i, Instantiate(visBridgePrefab).GetComponent<MultiVisBridge>());
        }
    }

    public void ToggleVisBridgesBetweenAllRepresentativeGameObjectsOf(InfoObject obj)
    {
        if(!visBridgeSystemInitialized)
            InitVisBridgeSystem();

        if(multiBridgesByDataSet[obj.dataSetID].HasInfoObject(obj))
            RemoveInfoObjectFromVisBridge(obj);
        else
            DrawVisBridgesBetweenAllRepresentativeGameObjectsOf(obj);
    }

    /// <summary>
    /// Removes a visbridge connecting representative graphical primitives of the given information object
    /// </summary>
    /// <param name="obj"></param>
    public void RemoveInfoObjectFromVisBridge(InfoObject obj)
    {
        if(!visBridgeSystemInitialized)
            InitVisBridgeSystem();

        if(multiBridgesByDataSet[obj.dataSetID].HasInfoObject(obj))
        {
            bool visBridgeDestroyed = multiBridgesByDataSet[obj.dataSetID].RemovePrimitives(obj, GetRepresentativePrimitives(obj));

            if(visBridgeDestroyed)
                visBridges[obj.dataSetID].Remove(obj);
        }
    }

    /// <summary>
    /// Generates VisBridge-GameObjects that connect all graphical primitives in all
    /// active visualizations that represent the given InformationObject to each other.
    /// </summary>
    /// <param name="obj">InformationObject in question</param>
    public void DrawVisBridgesBetweenAllRepresentativeGameObjectsOf(InfoObject obj)
    {
        if(!visBridgeSystemInitialized)
            InitVisBridgeSystem();

        

        if(!GlobalSettings.onHoloLens) Debug.Log("Draw new Bridge");

        if(multiBridgesByDataSet.ContainsKey(obj.dataSetID))
        {
            multiBridgesByDataSet[obj.dataSetID].AddMorePrimitives(obj, GetRepresentativePrimitives(obj));
        }else
        {
            multiBridgesByDataSet.Add(obj.dataSetID, GenerateVisBridgeFor(obj, GetRepresentativePrimitives(obj)).GetComponent<MultiVisBridge>());
        }
        
    }

    private AGraphicalPrimitive[] GetRepresentativePrimitives(InfoObject o)
    {
        var numberOfRepresentativeObjects = 0;
        foreach(var listOrigins in o.representativeGameObjectsByAttributeName.Values)
        {
            numberOfRepresentativeObjects += listOrigins.Count;
        }

        var primitives = new AGraphicalPrimitive[numberOfRepresentativeObjects];

        int counter = 0;
        foreach(var listOrigins in o.representativeGameObjectsByAttributeName.Values)
        {
            foreach(var prim in listOrigins)
            {
                primitives[counter] = prim.GetComponent<AGraphicalPrimitive>();
                counter++;
            }
        }

        return primitives;
    }

    private GameObject CreateObjectBasedVisBridge(AGraphicalPrimitive origin, AGraphicalPrimitive target)
    {
        var visBridge = Instantiate(ObjectBasedVisBridgePrefab);
        visBridge.GetComponent<ObjectBasedVisBridge>().Init(origin, target);
        origin.Brush(Color.green);
        target.Brush(Color.green);

        return visBridge;
    }

    public GameObject visBridgePrefab;

    private GameObject GenerateVisBridgeFor(InfoObject o, AGraphicalPrimitive[] primitives)
    {
        var visBridge = Instantiate(visBridgePrefab);

        visBridge.GetComponent<MultiVisBridge>().AddMorePrimitives(o, primitives);

        return visBridge;
    }

    private GameObject AddToVisBridge(InfoObject o, AGraphicalPrimitive[] primitives)
    {
        try
        {
            var bridge = multiBridgesByDataSet[o.dataSetID];
            bridge.AddMorePrimitives(o, primitives);
            return bridge.gameObject;
        } catch(Exception e)
        {
            Debug.LogError(e.Message);
            return new GameObject();
        }
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
            if(!CheckIfSuitable(dataSetID, new string[] { variable }, VisType.BarChart2D))
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
            if(!CheckIfSuitable(dataSetID, new string[] { variable }, VisType.BarChart3D))
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
            if(!CheckIfSuitable(dataSetID, variables, VisType.BarMap3D))
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
            if(!CheckIfSuitable(dataSetID, variables, VisType.PCP2D))
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
            if(!CheckIfSuitable(dataSetID, variables, VisType.PCP3D))
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
            Debug.LogError(e.StackTrace);
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
            if(!CheckIfSuitable(dataSetID, variables, VisType.ScatterPlot3D))
            {
                return new GameObject("Not Suitable");
            }

            var ds = dataProvider.dataSets[dataSetID];

            var factory = ServiceLocator.instance.Factory3DETV;
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
                return new GameObject("Not Suitable");
            }

            var ds = dataProvider.dataSets[dataSetID];

            var factory = ServiceLocator.instance.Factory2DETV;
            var vis = factory.CreateETVLineChart(ds, variables[0], variables[1]);

            vis = factory.PutETVOnAnchor(vis);

            vis.transform.position = NewETVPosition.transform.position;

            return vis;

        } catch(Exception e)
        {
            Debug.Log("Creation of requested Visualization for variable " + variables + " failed.");
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
