using Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AETVFactory : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public abstract GameObject CreateSingleAxis(DataSet data, int attributeID, LoM lom);
    public abstract GameObject CreateETVParallelCoordinatesPlot(DataSet data, string[] attIDs);
    public abstract GameObject CreateETVLineChart(DataSet data, string attributeNameA, string attributeNameB);
    public abstract GameObject CreateETVBarChart(DataSet data, string attributeName);
    public abstract GameObject CreateETVScatterPlot(DataSet data, string[] attIDs);
    public abstract GameObject PutETVOnAnchor(GameObject ETV);
}
