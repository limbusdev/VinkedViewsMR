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

    public abstract GameObject CreateETVParallelCoordinatesPlot(DataSetMultiDimensionalPoints data, float[] ticks);
    public abstract GameObject CreateETVLineChart(DataSetLines data, float minX, float maxX, float minY, float maxY, float ticksX, float ticksY);
    public abstract GameObject CreateETVBarChart(DataSet data, int attributeID);
    public abstract GameObject CreateETVScatterPlot(DataSetPoints data, float[] mins, float[] maxs, float[] ticks);
}
