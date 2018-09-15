using GraphicalPrimitive;
using HoloToolkit.Unity.Collections;
using Model;
using Model.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VisBridge;
using System.Linq;

public class VisualizationFactory : MonoBehaviour {

    public GameObject ObjectBasedVisBridgePrefab;
    public GameObject NewETVPosition;
    public GameObject ObjectCollection;
    public GameObject CubeIconVariable;
    public DataProvider dataProvider;

    public Material lineMaterial;

    private IList<GameObject> activeVisualizations;
    private IList<ObjectBasedVisBridge> activeVisBridges;

    

    private void Awake()
    {
        activeVisualizations = new List<GameObject>();
        activeVisBridges = new List<ObjectBasedVisBridge>();
    }

    // Use this for initialization
    void Start()
    {
        DataSet fbiData = dataProvider.dataSets[0];

        var educationObjects = new List<InformationObject>();
        educationObjects.Add(new InformationObject(
            new GenericAttribute<string>[] {
                new GenericAttribute<string>("Sex", "male", LevelOfMeasurement.NOMINAL),
                new GenericAttribute<string>("Level of Education", "ungraduated", LevelOfMeasurement.ORDINAL)
                },
            new GenericAttribute<float>[] {
                new GenericAttribute<float>("Fraction [%]", 20, LevelOfMeasurement.RATIO),
                new GenericAttribute<float>("Mean Age [a]", 25, LevelOfMeasurement.RATIO)
            },
            new GenericAttribute<int>[] { },
            new GenericAttribute<Vector2>[] { },
            new GenericAttribute<Vector3>[] { }
            ));
        educationObjects.Add(new InformationObject(
            new GenericAttribute<string>[] {
                new GenericAttribute<string>("Sex", "male", LevelOfMeasurement.NOMINAL),
                new GenericAttribute<string>("Level of Education", "Highschool", LevelOfMeasurement.ORDINAL)
                },
            new GenericAttribute<float>[] {
                new GenericAttribute<float>("Fraction [%]", 40, LevelOfMeasurement.RATIO),
                new GenericAttribute<float>("Mean Age [a]", 27, LevelOfMeasurement.RATIO)
            },
            new GenericAttribute<int>[] { },
            new GenericAttribute<Vector2>[] { },
            new GenericAttribute<Vector3>[] { }
            ));
        educationObjects.Add(new InformationObject(
            new GenericAttribute<string>[] {
                new GenericAttribute<string>("Sex", "male", LevelOfMeasurement.NOMINAL),
                new GenericAttribute<string>("Level of Education", "University", LevelOfMeasurement.ORDINAL)
                },
            new GenericAttribute<float>[] {
                new GenericAttribute<float>("Fraction [%]", 40, LevelOfMeasurement.RATIO),
                new GenericAttribute<float>("Mean Age [a]", 31, LevelOfMeasurement.RATIO)
            },
            new GenericAttribute<int>[] { },
            new GenericAttribute<Vector2>[] { },
            new GenericAttribute<Vector3>[] { }
            ));
        var educationalData = new DataSet("Educational Data", "", educationObjects);

        GameObject new2DBarChart = ServiceLocator.instance.ETV2DFactoryService.CreateETVBarChart(educationalData, 1);
        GameObject newETV2DBarChart = ServiceLocator.instance.ETV2DFactoryService.PutETVOnAnchor(new2DBarChart);

        GameObject new2DBarChart2 = ServiceLocator.instance.ETV2DFactoryService.CreateETVBarChart(dataProvider.dataSets[1], 6);
        GameObject newETV2DBarChart2 = ServiceLocator.instance.ETV2DFactoryService.PutETVOnAnchor(new2DBarChart2);
        newETV2DBarChart.transform.Translate(new Vector3(1, 1, 1));

        GameObject new2DBarChart3 = ServiceLocator.instance.ETV2DFactoryService.CreateETVBarChart(dataProvider.dataSets[1], 3);
        GameObject newETV2DBarChart3 = ServiceLocator.instance.ETV2DFactoryService.PutETVOnAnchor(new2DBarChart3);
        newETV2DBarChart3.transform.Translate(new Vector3(-1, 1, -2));

        GameObject new2DPCP = ServiceLocator.instance.ETV2DFactoryService.CreateETVParallelCoordinatesPlot(educationalData, new int[] { 0, 1 }, new int[] { });
        GameObject newETV2DPCP = ServiceLocator.instance.ETV2DFactoryService.PutETVOnAnchor(new2DPCP);
        newETV2DPCP.transform.Translate(new Vector3(-2, 0, 0));

        GameObject new2DXY = ServiceLocator.instance.ETV2DFactoryService.CreateETVLineChart(educationalData, 0, 1, false, true);
        GameObject newETV2DXY = ServiceLocator.instance.ETV2DFactoryService.PutETVOnAnchor(new2DXY);
        newETV2DXY.transform.Translate(new Vector3(-2, 0, 2));

        GameObject new2DXY2 = ServiceLocator.instance.ETV2DFactoryService.CreateETVLineChart(fbiData, 0, 1, false, true);
        GameObject newETV2DXY2 = ServiceLocator.instance.ETV2DFactoryService.PutETVOnAnchor(new2DXY2);
        newETV2DXY2.transform.Translate(new Vector3(-3, 0, 1));

        GameObject new2DScatter = ServiceLocator.instance.ETV2DFactoryService.CreateETVScatterPlot(fbiData, new int[] { 3, 5 });
        GameObject newETV2DScatter = ServiceLocator.instance.ETV2DFactoryService.PutETVOnAnchor(new2DScatter);
        newETV2DScatter.transform.Translate(new Vector3(-3, 0, -3));

        GameObject new3DBarChart = ServiceLocator.instance.ETV3DFactoryService.CreateETVBarChart(dataProvider.dataSets[1], 4);
        GameObject newETV3DBarChart = ServiceLocator.instance.ETV3DFactoryService.PutETVOnAnchor(new3DBarChart);
        newETV3DBarChart.transform.Translate(new Vector3(-3, 0, -5));

        GameObject new3DBarMap = ServiceLocator.instance.ETV3DFactoryService.CreateETVBarMap(dataProvider.dataSets[1], 4,6);
        GameObject newETV3DBarMap = ServiceLocator.instance.ETV3DFactoryService.PutETVOnAnchor(new3DBarMap);
        newETV3DBarMap.transform.Translate(new Vector3(6, 0, 4));


        //foreach(InformationObject o in educationalData.dataObjects)
        //{
        //    DrawVisBridgesBetweenAllRepresentativeGameObjectsOf(o);
        //}

        DrawVisBridgesBetweenAllRepresentativeGameObjectsOf(educationalData.informationObjects.First());

        DrawVisBridgesBetweenAllRepresentativeGameObjectsOf(dataProvider.dataSets[1].informationObjects[0]);


    }
	
	// Update is called once per frame
	void Update () {
		
	}

    

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

    public void AddNewVisualization(GameObject visualization)
    {
        if(visualization.GetComponent<ETVAnchor>() != null)
        {
            activeVisualizations.Add(visualization);
        } 
        else
        {
            Debug.LogWarning("Given GameObject ist not an anchored visualization!");
        }
    }

    private void UpdateVisualizationConnections()
    {

    }

    private void UpdateVisBridges()
    {

    }

    
}
