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
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
