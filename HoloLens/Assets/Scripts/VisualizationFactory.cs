using GraphicalPrimitive;
using HoloToolkit.Unity.Collections;
using Model;
using Model.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizationFactory : MonoBehaviour {

    public GameObject NewETVPosition;
    public GameObject ObjectCollection;
    public GameObject CubeIconVariable;
    public DataProvider dataProvider;

    public Material lineMaterial;


    // Use this for initialization
    void Start ()
    {
        DataSet fbiData = dataProvider.dataSets[0];

        var educationObjects = new List<InformationObject>();
        educationObjects.Add(new InformationObject(
            new GenericAttribute<string>[] {
                new GenericAttribute<string>("Sex", "male", LevelOfMeasurement.NOMINAL),
                new GenericAttribute<string>("Level of Education", "ungraduated", LevelOfMeasurement.ORDINAL)
                },
            new GenericAttribute<float>[] {
                new GenericAttribute<float>("Fraction [%]", 20, LevelOfMeasurement.RATIO)
            },
            new GenericAttribute<int>[] {},
            new GenericAttribute<Vector2>[] {},
            new GenericAttribute<Vector3>[] {}
            ));
        educationObjects.Add(new InformationObject(
            new GenericAttribute<string>[] {
                new GenericAttribute<string>("Sex", "male", LevelOfMeasurement.NOMINAL),
                new GenericAttribute<string>("Level of Education", "Highschool", LevelOfMeasurement.ORDINAL)
                },
            new GenericAttribute<float>[] {
                new GenericAttribute<float>("Fraction [%]", 40, LevelOfMeasurement.RATIO)
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
                new GenericAttribute<float>("Fraction [%]", 40, LevelOfMeasurement.RATIO)
            },
            new GenericAttribute<int>[] { },
            new GenericAttribute<Vector2>[] { },
            new GenericAttribute<Vector3>[] { }
            ));
        var educationalData = new DataSet("Educational Data", "", educationObjects);

        GameObject new2DBarChart = ServiceLocator.instance.ETV2DFactoryService.CreateETVBarChart(educationalData, 1, 0);
        GameObject newETV2DBarChart = ServiceLocator.instance.ETV2DFactoryService.PutETVOnAnchor(new2DBarChart);

        GameObject new2DBarChart2 = ServiceLocator.instance.ETV2DFactoryService.CreateETVBarChart(educationalData, 1, 0);
        GameObject newETV2DBarChart2 = ServiceLocator.instance.ETV2DFactoryService.PutETVOnAnchor(new2DBarChart2);
        newETV2DBarChart.transform.Translate(new Vector3(1, 1, 1));

        foreach(InformationObject o in educationalData.dataObjects)
        {
            DrawVisBridgesBetweenAllRepresentativeGameObjectsOf(o);
        }
        

        /*
        DataSet educationData = new DataSet("Educational Data", "", null);

        DataSetMatrix2x2Nominal dataSet = new DataSetMatrix2x2Nominal(
            new string[] { "male", "female", "other" },
            new string[] { "ungraduated", "Highschool", "University" },
            new float[,] { { 20, 40, 40 }, { 18, 30, 52 }, { 10, 50, 40 } },
            "Fraction", "%");

        GameObject bm = ServiceLocator.instance.ETV3DFactoryService.CreateETVBarMap(dataSet, 10);
        bm.GetComponent<ETV3DBarMap>().ChangeColoringScheme(ETVColorSchemes.Rainbow);

        GameObject newETV = ServiceLocator.instance.ETV3DFactoryService.PutETVOnAnchor(bm);

        //newETV.transform.position = NewETVPosition.transform.position;

        for(int i=0; i<dataProvider.GetAvailableVariables().Length; i++)
        {
            GameObject etvIcon = Instantiate(CubeIconVariable);
            CubeIconVariable civ = etvIcon.GetComponent<CubeIconVariable>();
            civ.Init(new string[] { dataProvider.GetAvailableVariables()[i] }, new string[] { "" }, new DataType[] { dataProvider.GetTypeOfVariable(dataProvider.GetAvailableVariables()[i]) });
            etvIcon.transform.parent = ObjectCollection.transform;
        }

        ObjectCollection.GetComponent<ObjectCollection>().UpdateCollection();




        IDictionary<string, InformationObject> barChartValues = new Dictionary<string, InformationObject>();
        barChartValues.Add("Baden-Württemberg", new InformationObject(
            new string[] { "Population", "Area" }, new float[] { 10880000f, 35751.65f }));
        barChartValues.Add("Bayern", new InformationObject(
            new string[] { "Population", "Area" }, new float[] { 12843500f, 70549.19f }));
        barChartValues.Add("Berlin", new InformationObject(
            new string[] { "Population", "Area" }, new float[] { 3520000f, 891.75f }));
        barChartValues.Add("Brandenburg", new InformationObject(
            new string[] { "Population", "Area" }, new float[] { 2484800f, 29477.16f }));
        barChartValues.Add("Bremem", new InformationObject(
            new string[] { "Population", "Area" }, new float[] { 671500f, 404.23f }));
        barChartValues.Add("Hamburg", new InformationObject(
            new string[] { "Population", "Area" }, new float[] { 1860800f, 755.16f }));
        barChartValues.Add("Hessen", new InformationObject(
            new string[] { "Population", "Area" }, new float[] { 6176000f, 21114.72f }));
        barChartValues.Add("Mecklenurg-Vorpommern", new InformationObject(
            new string[] { "Population", "Area" }, new float[] { 1612300f, 23174.17f }));
        barChartValues.Add("Niedersachsen", new InformationObject(
            new string[] { "Population", "Area" }, new float[] { 7927000f, 47618.24f }));
        barChartValues.Add("Nordrhein-Westfalen", new InformationObject(
            new string[] { "Population", "Area" }, new float[] { 17865000f, 34083.52f }));
        barChartValues.Add("Rheinland-Pfalz", new InformationObject(
            new string[] { "Population", "Area" }, new float[] { 4073300f, 19847.39f }));
        barChartValues.Add("Saarland", new InformationObject(
            new string[] { "Population", "Area" }, new float[] { 995600f, 2568.65f }));
        barChartValues.Add("Sachsen", new InformationObject(
            new string[] { "Population", "Area" }, new float[] { 4084000f, 18414.82f }));
        barChartValues.Add("Sachsen-Anhalt", new InformationObject(
            new string[] { "Population", "Area" }, new float[] { 2245500f, 20445.26f }));
        barChartValues.Add("Schleswig-Holstein", new InformationObject(
            new string[] { "Population", "Area" }, new float[] { 2865000f, -15763.18f }));
        barChartValues.Add("Thüringen", new InformationObject(
            new string[] { "Population", "Area" }, new float[] { 2170700f, 16172.14f }));


        DataSet cityDataSet = new DataSet(
            "Cities",
            "",
            new string[] { "Population", "Area" },
            new string[] { "People", "km^2" },
            barChartValues
            );

        GameObject singleAxis3D = ServiceLocator.instance.ETV3DFactoryService.CreateSingleAxis(cityDataSet, 0);
        GameObject anchoredAxis = ServiceLocator.instance.ETV3DFactoryService.PutETVOnAnchor(singleAxis3D);
        anchoredAxis.transform.Translate(new Vector3(-1, 0, 0));*/
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DrawVisBridgesBetweenAllRepresentativeGameObjectsOf(InformationObject obj)
    {
        

        foreach(IList<GameObject> list in obj.representativeGameObjectsByAttributeName.Values)
        {
            foreach(GameObject go in list)
            {
                foreach(GameObject other in list)
                {
                    if(!other.Equals(go))
                    {
                        GameObject visBridge = new GameObject("VisBridge");

                        CurvedLineRenderer clr = visBridge.AddComponent<CurvedLineRenderer>();
                        clr.lineWidth = 0.01f;
                        clr.lineSegmentSize = 0.01f;
                        clr.showGizmos = false;
                        
                        LineRenderer lr = visBridge.GetComponent<LineRenderer>();
                        lr.startColor = Color.green;
                        lr.endColor = Color.yellow;
                        lr.material = lineMaterial;
                        lr.startWidth = 0.01f;
                        lr.endWidth = 0.01f;

                        GameObject visBridgePort = go.GetComponent<AGraphicalPrimitive>().visBridgePort;
                        GameObject otherVisBridgePort = other.GetComponent<AGraphicalPrimitive>().visBridgePort;
                        
                        var clp = new GameObject("LinePoint");
                        clp.AddComponent<CurvedLinePoint>();
                        clp.transform.position = visBridgePort.transform.position;
                        clp.transform.parent = visBridge.transform;

                        // Set the first padding object as default
                        GameObject paddingObject = go.GetComponent<AGraphicalPrimitive>().visBridgePortPadding[0];

                        // If there are more than 1 padding objects
                        if(go.GetComponent<AGraphicalPrimitive>().visBridgePortPadding.Length > 1)
                        {
                            // Lookup all but the first padding objects
                            for(int i = 1; i < go.GetComponent<AGraphicalPrimitive>().visBridgePortPadding.Length; i++)
                            {
                                GameObject pad = go.GetComponent<AGraphicalPrimitive>().visBridgePortPadding[i];

                                // Look if it is nearer to the other representative object than the currently set padding
                                float oldDistance, newDistance;
                                oldDistance = Vector3.Distance(paddingObject.transform.position, otherVisBridgePort.transform.position);
                                newDistance = Vector3.Distance(pad.transform.position, otherVisBridgePort.transform.position);

                                if(newDistance < oldDistance)
                                {
                                    paddingObject = pad;
                                }
                            }
                        }

                        clp = new GameObject("LinePoint");
                        clp.AddComponent<CurvedLinePoint>();
                        clp.transform.position = paddingObject.transform.position;
                        clp.transform.parent = visBridge.transform;

                        // Set the first padding object as default
                        paddingObject = other.GetComponent<AGraphicalPrimitive>().visBridgePortPadding[0];

                        // If there are more than 1 padding objects
                        if(other.GetComponent<AGraphicalPrimitive>().visBridgePortPadding.Length > 1)
                        {
                            // Lookup all but the first padding objects
                            for(int i = 1; i < other.GetComponent<AGraphicalPrimitive>().visBridgePortPadding.Length; i++)
                            {
                                GameObject pad = other.GetComponent<AGraphicalPrimitive>().visBridgePortPadding[i];

                                // Look if it is nearer to the other representative object than the currently set padding
                                float oldDistance, newDistance;
                                oldDistance = Vector3.Distance(paddingObject.transform.position, visBridgePort.transform.position);
                                newDistance = Vector3.Distance(pad.transform.position, visBridgePort.transform.position);

                                if(newDistance < oldDistance)
                                {
                                    paddingObject = pad;
                                }
                            }
                        }

                        clp = new GameObject("LinePoint");
                        clp.AddComponent<CurvedLinePoint>();
                        clp.transform.position = paddingObject.transform.position;
                        clp.transform.parent = visBridge.transform;

                        clp = new GameObject("LinePoint");
                        clp.AddComponent<CurvedLinePoint>();
                        clp.transform.position = otherVisBridgePort.transform.position;
                        clp.transform.parent = visBridge.transform;

                        //objCounter++;
                    }
                }
            }
        }
    }
}
