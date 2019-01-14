/*
Vinked Views
Copyright(C) 2018  Georg Eckert

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
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
