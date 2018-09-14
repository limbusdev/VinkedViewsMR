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

    public abstract GameObject CreateSingleAxis(DataSet data, int attributeID);
    public abstract GameObject CreateETVParallelCoordinatesPlot(DataSet data, int[] floatAttributeIDs, int[] stringAttributeIDs);
    public abstract GameObject CreateETVLineChart(DataSet data, int floatAttributeX, int floatAttributeY, bool xAxisBoundToZero, bool yAxisBoundToZero);
    public abstract GameObject CreateETVBarChart(DataSet data, int nominalAttributeID, int numericAttributeID);
    public abstract GameObject CreateETVScatterPlot(DataSet data, int[] floatAttributeIDs);
    public abstract GameObject PutETVOnAnchor(GameObject ETV);
}
