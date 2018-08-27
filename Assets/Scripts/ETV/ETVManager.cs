using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * 
 * */
public class ETVManager : MonoBehaviour {

	// Use this for initialization
	void Start () {

        IDictionary<string, DataObject> barChartValues = new Dictionary<string,DataObject>();
        barChartValues.Add("Baden-Württemberg",     new DataObject(
            new string[] { "Population", "Area" }, new float[] { 10880000f, 35751.65f }));
        barChartValues.Add("Bayern",                new DataObject(
            new string[] { "Population", "Area" }, new float[] { 12843500f, 70549.19f }));
        barChartValues.Add("Berlin",                new DataObject(
            new string[] { "Population", "Area" }, new float[] { 3520000f, 891.75f }));
        barChartValues.Add("Brandenburg",           new DataObject(
            new string[] { "Population", "Area" }, new float[] { 2484800f, 29477.16f }));
        barChartValues.Add("Bremem",                new DataObject(
            new string[] { "Population", "Area" }, new float[] { 671500f, 404.23f }));
        barChartValues.Add("Hamburg",               new DataObject(
            new string[] { "Population", "Area" }, new float[] { 1860800f, 755.16f }));
        barChartValues.Add("Hessen",                new DataObject(
            new string[] { "Population", "Area" }, new float[] { 6176000f, 21114.72f }));
        barChartValues.Add("Mecklenurg-Vorpommern", new DataObject(
            new string[] { "Population", "Area" }, new float[] { 1612300f, 23174.17f }));
        barChartValues.Add("Niedersachsen",         new DataObject(
            new string[] { "Population", "Area" }, new float[] { 7927000f, 47618.24f }));
        barChartValues.Add("Nordrhein-Westfalen",   new DataObject(
            new string[] { "Population", "Area" }, new float[] { 17865000f, 34083.52f }));
        barChartValues.Add("Rheinland-Pfalz",       new DataObject(
            new string[] { "Population", "Area" }, new float[] { 4073300f, 19847.39f }));
        barChartValues.Add("Saarland",              new DataObject(
            new string[] { "Population", "Area" }, new float[] { 995600f, 2568.65f }));
        barChartValues.Add("Sachsen",               new DataObject(
            new string[] { "Population", "Area" }, new float[] { 4084000f, 18414.82f }));
        barChartValues.Add("Sachsen-Anhalt",        new DataObject(
            new string[] { "Population", "Area" }, new float[] { 2245500f, 20445.26f }));
        barChartValues.Add("Schleswig-Holstein",    new DataObject(
            new string[] { "Population", "Area" }, new float[] { 2865000f, -15763.18f }));
        barChartValues.Add("Thüringen",             new DataObject(
            new string[] { "Population", "Area" }, new float[] { 2170700f, 16172.14f }));


        DataSet cityDataSet = new DataSet(
            "Cities",
            "",
            new string[] { "Population", "Area" },
            new string[] { "People", "km^2" },
            barChartValues
            );



        /*GameObject EtvBarChart = ServiceLocator.instance.ETV3DFactoryService.CreateETVBarChart(cityDataSet, 0);
        EtvBarChart.GetComponent<ETV3DBarChart>().ChangeColoringScheme(ETVColorSchemes.GrayZebra);
        EtvBarChart.transform.position = new Vector3(-4, 0, 0);

        GameObject Etv2DBarChart = ServiceLocator.instance.ETV2DFactoryService.CreateETVBarChart(cityDataSet, 0);
        Etv2DBarChart.GetComponent<ETV2DBarChart>().ChangeColoringScheme(ETVColorSchemes.Rainbow);

        GameObject virt2Ddevice = ServiceLocator.instance.ETV2DFactoryService.CreateVirtualDevice();
        virt2Ddevice.GetComponent<ETV2DVirtualDevice>().BindVisualization(Etv2DBarChart);

        GameObject EtvGroupedBarChart = ServiceLocator.instance.factoryETV3Dservice.Create3DGroupedBarChart(barChartValues);
        EtvGroupedBarChart.GetComponent<ETV3DGroupedBarChart>().ChangeColoringScheme(ETVColorSchemes.Rainbow);
        EtvGroupedBarChart.GetComponent<ETV3DGroupedBarChart>().SetLegendActive(true);*/

        AGraphicalPrimitiveFactory factory = ServiceLocator.instance.PrimitiveFactory3Dservice;
        //GameObject axis3D = factory.CreateAxis(Color.red, "", "", new Vector3(1,1,1), 1.5f);
        //axis3D.transform.localPosition = new Vector3(2,0,2);


        Vector2[] kaninchen = new Vector2[] {
           new Vector2(1,2),
           new Vector2(2,4),
           new Vector2(3,8),
           new Vector2(4,16),
           new Vector2(5,32)
           };
        LineObject lo = new LineObject("Kaninchen", kaninchen);
        DataSetLines dsl = new DataSetLines(new LineObject[] { lo }, "Zeit", "a", "Tiere", "tausend");
        //GameObject lc = ServiceLocator.instance.ETV2DFactoryService.CreateETVLineChart(dsl);


        //GameObject testBar = factory.CreateBar(1,1,1,1);




        /*IDictionary<string, DataObject> barChartValues2 = new Dictionary<string, DataObject>();
        barChartValues2.Add("Baden-Württemberg", new DataObject(
            new string[] { "Population" }, new float[] { 10880000f }));
        barChartValues2.Add("Bayern", new DataObject(
            new string[] { "Population" }, new float[] { 12843500f }));
        barChartValues2.Add("Berlin", new DataObject(
            new string[] { "Population" }, new float[] { 3520000f }));
        barChartValues2.Add("Brandenburg", new DataObject(
            new string[] { "Population" }, new float[] { 2484800f }));
        barChartValues2.Add("Bremem", new DataObject(
            new string[] { "Population" }, new float[] { 671500f }));
        barChartValues2.Add("Hamburg", new DataObject(
            new string[] { "Population" }, new float[] { 1860800f }));
        


        GameObject EtvBarChart2 = new GameObject("ETV 3D Bar Chart2");

        EtvBarChart2.AddComponent<ETV3DBarChart>();
        EtvBarChart2.GetComponent<ETV3DBarChart>().Init(barChartValues2, 0);
        EtvBarChart2.GetComponent<ETV3DBarChart>().ChangeColoringScheme(ETVColorSchemes.Rainbow);

        EtvBarChart2.transform.localPosition = new Vector3(0, 0, -3);

        GameObject test2Dbar = ServiceLocator.instance.get2DFactory().CreateBar(.75f, 1f, .02f, 0);
        test2Dbar.transform.localPosition = new Vector3(-2, 0, 0);
        test2Dbar.GetComponent<GraphicalPrimitive.Bar2D>().ChangeColor(Color.red);

        GameObject barChart2D = new GameObject();
        barChart2D.AddComponent<ETV2DBarChart>();
        barChart2D.GetComponent<ETV2DBarChart>().Init(barChartValues2);
        barChart2D.transform.localPosition = new Vector3(-5,0,0);

        barChart2D.GetComponent<ETV2DBarChart>().ChangeColoringScheme(ETVColorSchemes.Rainbow);*/
    }

    // Update is called once per frame
    void Update () {
		
	}
}
