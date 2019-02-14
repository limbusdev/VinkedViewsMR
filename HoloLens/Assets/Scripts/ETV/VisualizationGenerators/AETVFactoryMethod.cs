/*
Copyright 2019 Georg Eckert (MIT License)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to
deal in the Software without restriction, including without limitation the
rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
sell copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
IN THE SOFTWARE.
*/
using System;
using UnityEngine;

public enum VisType
{
    SingleAxis3D,
    Histogram2D,
    Histogram3D,
    HistogramHeatmap3D,
    LineChart2D,
    ScatterPlot2D,
    ScatterPlot3D,
    PCP2D,
    PCP3D
}

public abstract class AETVFactoryMethod : MonoBehaviour
{
    public GameObject GenerateVisualization(int dataSetID, string[] variables)
    {
        try
        {
            if(!CheckIfSuitable(dataSetID, variables))
            {
                return new GameObject("not suitable");
            }

            return GeneratorTemplate(dataSetID, variables);

        } catch(Exception e)
        {
            Debug.LogError("Creation of requested Visualization for variable failed.");
            Debug.LogException(e);
            return new GameObject("Creation Failed");
        }
    }

    /// <summary>
    /// Checks whether the given attributes of the given DataSet are suitable for
    /// the visualization type in question
    /// </summary>
    /// <param name="dataSetID">ID of the data set</param>
    /// <param name="attributes">attributes in question</param>
    /// <returns>if suitable</returns>
    public bool CheckIfSuitable(int dataSetID, string[] variables)
    {
        int[] nomIDs, ordIDs, ivlIDs, ratIDs;

        AttributeProcessor.ExtractAttributeIDs(
            Services.DataBase().dataSets[dataSetID], 
            variables, 
            out nomIDs, 
            out ordIDs, 
            out ivlIDs, 
            out ratIDs);

        return CheckTemplate(nomIDs.Length, ordIDs.Length, ivlIDs.Length, ratIDs.Length);
    }


    protected abstract GameObject GeneratorTemplate(int dataSetID, string[] variables);
    protected abstract bool CheckTemplate(int nominals, int ordinals, int intervals, int rationals);
}
