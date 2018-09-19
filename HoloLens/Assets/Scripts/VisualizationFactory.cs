using GraphicalPrimitive;
using HoloToolkit.Unity.Collections;
using Model;
using Model.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VisBridge;
using System.Linq;

/// <summary>
/// Main class for visualization generation from databases.
/// </summary>
public class VisualizationFactory : MonoBehaviour
{
    // ........................................................................ Field to populate in editor
    public GameObject ObjectBasedVisBridgePrefab;
    public GameObject NewETVPosition;
    public GameObject ObjectCollection;
    public GameObject CubeIconVariable;
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
        var fact2 = ServiceLocator.instance.ETV2DFactoryService;
        var prim2Dfactory = ServiceLocator.instance.PrimitiveFactory2Dservice;
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

        var ae5 = GenerateBarChart2DFrom(1, "Crime");
        ae5.transform.position = new Vector3(0, 0, 4);

        /*
        DataSet fbiData = dataProvider.dataSets[0];

        GameObject new2DBarChart = ServiceLocator.instance.ETV2DFactoryService.CreateETVBarChart(dataProvider.dataSets[1], 2);
        GameObject newETV2DBarChart = ServiceLocator.instance.ETV2DFactoryService.PutETVOnAnchor(new2DBarChart);
        newETV2DBarChart.transform.Translate(new Vector3(1.98f * 2, 0, -.39f * 2));
        newETV2DBarChart.GetComponent<ETVAnchor>().VisAnchor.transform.parent.LookAt(Vector3.zero);

        GameObject new2DBarChart2 = ServiceLocator.instance.ETV2DFactoryService.CreateETVBarChart(dataProvider.dataSets[1], 4);
        GameObject newETV2DBarChart2 = ServiceLocator.instance.ETV2DFactoryService.PutETVOnAnchor(new2DBarChart2);
        newETV2DBarChart2.transform.Translate(new Vector3(-.12f * 2, 0, -1.97f * 2));
        newETV2DBarChart2.GetComponent<ETVAnchor>().VisAnchor.transform.parent.LookAt(Vector3.zero);

        GameObject new2DBarChart3 = ServiceLocator.instance.ETV2DFactoryService.CreateETVBarChart(dataProvider.dataSets[1], 6);
        GameObject newETV2DBarChart3 = ServiceLocator.instance.ETV2DFactoryService.PutETVOnAnchor(new2DBarChart3);
        newETV2DBarChart3.transform.Translate(new Vector3(-1.22f * 2, 0, -1.53f * 2));
        newETV2DBarChart3.GetComponent<ETVAnchor>().VisAnchor.transform.parent.LookAt(Vector3.zero);

        //GameObject new2DPCP = ServiceLocator.instance.ETV2DFactoryService.CreateETVParallelCoordinatesPlot(fbiData, new int[] { }, new int[] { }, new int[] {0 }, new int[] { 0,1,3,5 });
        //GameObject newETV2DPCP = ServiceLocator.instance.ETV2DFactoryService.PutETVOnAnchor(new2DPCP);
        //newETV2DPCP.transform.Translate(new Vector3(-1.84f * 2, 0, -.72f * 2));
        //newETV2DPCP.GetComponent<ETVAnchor>().VisAnchor.transform.parent.LookAt(Vector3.zero);

        GameObject new2DXY = ServiceLocator.instance.ETV2DFactoryService.CreateETVLineChart(fbiData, 0, 2, false, true);
        GameObject newETV2DXY = ServiceLocator.instance.ETV2DFactoryService.PutETVOnAnchor(new2DXY);
        newETV2DXY.transform.Translate(new Vector3(-1.97f * 2, 0, .22f * 2));
        newETV2DXY.GetComponent<ETVAnchor>().VisAnchor.transform.parent.LookAt(Vector3.zero);

        GameObject new2DXY2 = ServiceLocator.instance.ETV2DFactoryService.CreateETVLineChart(fbiData, 0, 1, false, true);
        GameObject newETV2DXY2 = ServiceLocator.instance.ETV2DFactoryService.PutETVOnAnchor(new2DXY2);
        newETV2DXY2.transform.Translate(new Vector3(-1.67f * 2, 0, 1 * 2));
        newETV2DXY2.GetComponent<ETVAnchor>().VisAnchor.transform.parent.LookAt(Vector3.zero);

        GameObject new2DScatter = ServiceLocator.instance.ETV2DFactoryService.CreateETVScatterPlot(fbiData, new int[] { 3, 5 });
        GameObject newETV2DScatter = ServiceLocator.instance.ETV2DFactoryService.PutETVOnAnchor(new2DScatter);
        newETV2DScatter.transform.Translate(new Vector3(-.92f * 2, 0, 1.73f * 2));
        newETV2DScatter.GetComponent<ETVAnchor>().VisAnchor.transform.parent.LookAt(Vector3.zero);

        GameObject new3DBarChart = ServiceLocator.instance.ETV3DFactoryService.CreateETVBarChart(dataProvider.dataSets[1], 9);
        GameObject newETV3DBarChart = ServiceLocator.instance.ETV3DFactoryService.PutETVOnAnchor(new3DBarChart);
        newETV3DBarChart.transform.Translate(new Vector3(1 * 2, 0, 1.71f * 2));
        newETV3DBarChart.GetComponent<ETVAnchor>().VisAnchor.transform.parent.LookAt(Vector3.zero);

        GameObject new3DBarMap = ServiceLocator.instance.ETV3DFactoryService.CreateETVBarMap(dataProvider.dataSets[1], 2, 4);
        GameObject newETV3DBarMap = ServiceLocator.instance.ETV3DFactoryService.PutETVOnAnchor(new3DBarMap);
        newETV3DBarMap.transform.Translate(new Vector3(1.86f * 2, 0, .71f * 2));
        newETV3DBarMap.GetComponent<ETVAnchor>().VisAnchor.transform.parent.LookAt(Vector3.zero);

        GameObject new3DScatter = ServiceLocator.instance.ETV3DFactoryService.CreateETVScatterPlot(fbiData, new int[] { 1, 0, 2 });
        GameObject newETV3DScatter = ServiceLocator.instance.ETV3DFactoryService.PutETVOnAnchor(new3DScatter);
        newETV3DScatter.transform.Translate(new Vector3(.94f * 2, 0, -1.74f * 2));
        newETV3DScatter.GetComponent<ETVAnchor>().VisAnchor.transform.parent.LookAt(Vector3.zero);

        //GameObject new3DScatter2 = ServiceLocator.instance.ETV3DFactoryService.CreateETVParallelCoordinatesPlot(
        //    fbiData, new int[] { }, new int[] { }, new int[] { 0 }, new int[] { 0, 1, 3, 5 });
        //GameObject newETV3DScatter2 = ServiceLocator.instance.ETV3DFactoryService.PutETVOnAnchor(new3DScatter2);
        //newETV3DScatter2.transform.Translate(new Vector3(.94f * 4, 0, -1.74f * 4));
        //newETV3DScatter2.GetComponent<ETVAnchor>().VisAnchor.transform.parent.LookAt(Vector3.zero);

        // Exoplanets
        GameObject ex1 = ServiceLocator.instance.ETV3DFactoryService.CreateETVScatterPlot(dataProvider.dataSets[2], new int[] { 0, 1, 2 });
        GameObject exETV1 = ServiceLocator.instance.ETV3DFactoryService.PutETVOnAnchor(ex1);
        exETV1.transform.Translate(new Vector3(1.98f * 2, 0, -.39f * 2 - 10));
        exETV1.GetComponent<ETVAnchor>().VisAnchor.transform.parent.LookAt(new Vector3(0, 0, -10));

        GameObject ex2 = ServiceLocator.instance.ETV3DFactoryService.CreateETVBarChart(dataProvider.dataSets[2], 1);
        GameObject exETV2 = ServiceLocator.instance.ETV3DFactoryService.PutETVOnAnchor(ex2);
        exETV2.transform.Translate(new Vector3(-.12f * 2, 0, -1.97f * 2 - 10));
        exETV2.GetComponent<ETVAnchor>().VisAnchor.transform.parent.LookAt(new Vector3(0, 0, -10));

        GameObject ex3 = ServiceLocator.instance.ETV3DFactoryService.CreateETVBarMap(dataProvider.dataSets[2], 1, 2);
        GameObject exETV3 = ServiceLocator.instance.ETV3DFactoryService.PutETVOnAnchor(ex3);
        exETV3.transform.Translate(new Vector3(-1.22f * 2, 0, -1.53f * 2 - 10));
        exETV3.GetComponent<ETVAnchor>().VisAnchor.transform.parent.LookAt(new Vector3(0, 0, -10));


        //foreach(InformationObject o in educationalData.dataObjects)
        //{
        //    DrawVisBridgesBetweenAllRepresentativeGameObjectsOf(o);
        //}

        DrawVisBridgesBetweenAllRepresentativeGameObjectsOf(fbiData.informationObjects[0]);

        DrawVisBridgesBetweenAllRepresentativeGameObjectsOf(dataProvider.dataSets[1].informationObjects[0]);
        */

    }

    void Update()
    {

    }


    // ........................................................................ VisBridge generation
    /// <summary>
    /// Generates VisBridge-GameObjects that connect all graphical primitives in all
    /// active visualizations that represent the given InformationObject to each other.
    /// </summary>
    /// <param name="obj">InformationObject in question</param>
    public void DrawVisBridgesBetweenAllRepresentativeGameObjectsOf(InformationObject obj)
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
        var ds = dataProvider.dataSets[dataSetID];
        var type = ds.GetTypeOf(variable);

        var vis = ServiceLocator.instance.ETV3DFactoryService.CreateSingleAxis(ds, ds.GetIDOf(variable), type);
        vis = ServiceLocator.instance.ETV3DFactoryService.PutETVOnAnchor(vis);

        vis.transform.position = NewETVPosition.transform.position;

        return vis;
    }

    /// <summary>
    /// Generates a 2D bar chart for one categorical attribute.
    /// </summary>
    /// <param name="dataSetID"></param>
    /// <param name="variable">Attribute to generate the histogram from.</param>
    /// <returns>GameObject containing the anchored visualization.</returns>
    public GameObject GenerateBarChart2DFrom(int dataSetID, string variable)
    {
        var ds = dataProvider.dataSets[dataSetID];
        var type = ds.GetTypeOf(variable);

        var factory = ServiceLocator.instance.ETV2DFactoryService;
        var vis = factory.CreateETVBarChart(ds, variable);
        vis = factory.PutETVOnAnchor(vis);

        vis.transform.position = NewETVPosition.transform.position;

        return vis;
    }

    /// <summary>
    /// Generates a 3D bar chart for one categorical attribute.
    /// </summary>
    /// <param name="dataSetID"></param>
    /// <param name="variable">Attribute to generate the histogram from.</param>
    /// <returns>GameObject containing the anchored visualization.</returns>
    public GameObject GenerateBarChart3DFrom(int dataSetID, string variable)
    {
        var ds = dataProvider.dataSets[dataSetID];
        var type = ds.GetTypeOf(variable);

        var factory = ServiceLocator.instance.ETV3DFactoryService;
        var vis = factory.CreateETVBarChart(ds, variable);
        vis = factory.PutETVOnAnchor(vis);

        vis.transform.position = NewETVPosition.transform.position;

        return vis;
    }

    /// <summary>
    /// Generates a 3D bar map for two categorical attributes.
    /// </summary>
    /// <param name="dataSetID"></param>
    /// <param name="variables">Attributes to generate the histogram from.</param>
    /// <returns>GameObject containing the anchored visualization.</returns>
    public GameObject GenerateBarMap3DFrom(int dataSetID, string[] variables)
    {
        var ds = dataProvider.dataSets[dataSetID];
        var type1 = ds.GetTypeOf(variables[0]);
        var type2 = ds.GetTypeOf(variables[1]);

        var factory = ServiceLocator.instance.ETV3DFactoryService;
        var vis = factory.CreateETVBarMap(ds, ds.GetIDOf(variables[0]), ds.GetIDOf(variables[1]));
        vis = factory.PutETVOnAnchor(vis);

        vis.transform.position = NewETVPosition.transform.position;

        return vis;
    }

    /// <summary>
    /// Generates a 2D Parallel Coordinates Plot for n attributes.
    /// </summary>
    /// <param name="dataSetID"></param>
    /// <param name="variables">Attributes to be present in the PCP.</param>
    /// <returns>GameObject containing the anchored visualization.</returns>
    public GameObject GeneratePCP2DFrom(int dataSetID, string[] variables)
    {
        var ds = dataProvider.dataSets[dataSetID];

        var factory = ServiceLocator.instance.ETV2DFactoryService;
        var vis = factory.CreateETVParallelCoordinatesPlot(ds, variables);

        vis = factory.PutETVOnAnchor(vis);

        vis.transform.position = NewETVPosition.transform.position;

        return vis;
    }

    /// <summary>
    /// Generates a 3D Parallel Coordinates Plot for n attributes.
    /// </summary>
    /// <param name="dataSetID"></param>
    /// <param name="variables">Attributes to be present in the PCP.</param>
    /// <returns>GameObject containing the anchored visualization.</returns>
    public GameObject GeneratePCP3DFrom(int dataSetID, string[] variables)
    {
        var ds = dataProvider.dataSets[dataSetID];

        var factory = ServiceLocator.instance.ETV3DFactoryService;
        var vis = factory.CreateETVParallelCoordinatesPlot(ds, variables);

        vis = factory.PutETVOnAnchor(vis);

        vis.transform.position = NewETVPosition.transform.position;

        return vis;
    }

    /// <summary>
    /// Generates a 2D Scatterplot for 2 attributes.
    /// </summary>
    /// <param name="dataSetID"></param>
    /// <param name="variables">Attributes to be present in the scatterplot.</param>
    /// <returns>GameObject containing the anchored visualization.</returns>
    public GameObject GenerateScatterplot2DFrom(int dataSetID, string[] variables)
    {
        var ds = dataProvider.dataSets[dataSetID];

        var factory = ServiceLocator.instance.ETV2DFactoryService;
        var vis = factory.CreateETVScatterPlot(ds, variables);

        vis = factory.PutETVOnAnchor(vis);

        vis.transform.position = NewETVPosition.transform.position;

        return vis;
    }

    /// <summary>
    /// Generates a 3D Scatterplot for 3 attributes.
    /// </summary>
    /// <param name="dataSetID"></param>
    /// <param name="variables">Attributes to be present in the scatterplot.</param>
    /// <returns>GameObject containing the anchored visualization.</returns>
    public GameObject GenerateScatterplot3DFrom(int dataSetID, string[] variables)
    {
        return null;
    }

    /// <summary>
    /// Generates a 2D line plot for 2 attributes.
    /// </summary>
    /// <param name="dataSetID"></param>
    /// <param name="variables">Attributes to be present in the line plot.</param>
    /// <returns>GameObject containing the anchored visualization.</returns>
    public GameObject GenerateLineplot2DFrom(int dataSetID, string[] variables)
    {
        return null;
    }


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
            return new string[] { "SingleAxis3D", "BarChart2D", "BarChart3D" };
        } else if(count == new Vector4(0, 0, 2, 0))
        {
            return new string[] { "BarMap3D", "PCP2D", "PCP3D" };
        } else if(count == new Vector4(0, 0, 0, 1))
        {
            return new string[] { "SingleAxis3D" };
        } else if(count == new Vector4(0, 0, 0, 2))
        {
            return new string[] { "PCP2D", "PCP3D", "LineXY2D", "ScatterXY2D" };
        } else if(count == new Vector4(0, 0, 0, 3))
        {
            return new string[] { "PCP2D", "PCP3D", "ScatterXYZ3D" };
        } else
        {
            return new string[] { "PCP2D", "PCP3D" };
        }
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
