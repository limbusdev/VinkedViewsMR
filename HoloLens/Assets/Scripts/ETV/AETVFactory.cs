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
    public abstract GameObject CreateETVLineChart(DataSetLines data, float minX, float maxX, float minY, float maxY, float ticksX, float ticksY);
    public abstract GameObject CreateETVBarChart(DataSet data, int nominalAttributeID, int numericAttributeID);
    public abstract GameObject CreateETVScatterPlot(DataSetPoints data, float[] mins, float[] maxs, float[] ticks);
    public abstract GameObject PutETVOnAnchor(GameObject ETV);
}
