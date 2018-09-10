using HoloToolkit.Unity.Collections;
using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizationFactory : MonoBehaviour {

    public GameObject NewETVPosition;
    public GameObject ObjectCollection;
    public GameObject CubeIconVariable;
    public DataProvider dataProvider;


    // Use this for initialization
    void Start ()
    {
        


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
        anchoredAxis.transform.Translate(new Vector3(-1, 0, 0));
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
