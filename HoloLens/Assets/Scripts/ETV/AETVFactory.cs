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

    public abstract GameObject CreateSingleAxis(DataSet data, int attributeID, LevelOfMeasurement lom);
    public abstract GameObject CreateETVParallelCoordinatesPlot(DataSet data, string[] attIDs);
    public abstract GameObject CreateETVLineChart(DataSet data, int floatAttributeX, int floatAttributeY, bool xAxisBoundToZero, bool yAxisBoundToZero);
    public abstract GameObject CreateETVBarChart(DataSet data, int nominalAttributeID);
    public abstract GameObject CreateETVScatterPlot(DataSet data, int[] floatAttributeIDs);
    public abstract GameObject PutETVOnAnchor(GameObject ETV);
}
