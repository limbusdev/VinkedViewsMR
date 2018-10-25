/*
Copyright 2018 Georg Eckert

(Lincensed under MIT license)

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
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE 
SOFTWARE.
*/

using GraphicalPrimitive;
using Model;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MetaVisSystem : MonoBehaviour
{
    [SerializeField]
    public DataProvider dataProvider;

    public static float triggerMetaVisDistance = 2f;

    private IList<AAxis> axes;
    private IDictionary<MetaVisKey,GameObject> metaVisualizations;

    private void Awake()
    {
        axes = new List<AAxis>();
        metaVisualizations = new Dictionary<MetaVisKey, GameObject>();
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Match every observed axis against every other observed axis
		foreach(var axisA in axes)
        {
            foreach(var axisB in axes)
            {
                // Check if there isn't a MetaVis with both axes already
                if(!metaVisualizations.ContainsKey(new MetaVisKey(axisA, axisB)))
                {
                    // If they're not identical and not representing the same data dimension
                    // and are near enough to each other
                    if(CheckIfNearEnough(axisA, axisB))
                    {
                        int dataSetID;
                        if(CheckIfCompatible(axisA, axisB, out dataSetID))
                        {
                            // span a new metavisualization between them
                            metaVisualizations.Add(new MetaVisKey(axisA, axisB), SpanMetaVisBetween(axisA, axisB, dataSetID));
                        }
                    }
                }
            }
        }
	}

    /// <summary>
    /// Adds the given axis to the list of permanently observed axes.
    /// If observed, axis can span metavisualizations between them, if
    /// they fullfill given factors.
    /// </summary>
    /// <param name="axis"></param>
    public void ObserveAxis(AAxis axis)
    {
        axes.Add(axis);
    }

    /// <summary>
    /// Removes the given axis from the list of permanently observed axes.
    /// Use this, when destroying or disabling a visualization.
    /// </summary>
    /// <param name="axis"></param>
    public void StopObservationOf(AAxis axis)
    {
        axes.Remove(axis);
    }

    /// <summary>
    /// Generate a metavisualization between the given axes.
    /// </summary>
    /// <param name="axisA"></param>
    /// <param name="axisB"></param>
    /// <returns>A new metavisualization</returns>
    public GameObject SpanMetaVisBetween(AAxis axisA, AAxis axisB, int dataSetID)
    {
        // Which metavisualization will do?
        switch(WhichMetaVis(axisA, axisB, dataSetID))
        {
            default:
                return SpanMetaVisImmersiveAxis(axisA, axisB, dataSetID);
        }
    }

    /// <summary>
    /// Checks whether the given axis tips and bases are both nearer to each other
    /// than a predefined border value.
    /// </summary>
    /// <param name="axisA"></param>
    /// <param name="axisB"></param>
    /// <returns>If they are near enough to span a MetaVis.</returns>
    public bool CheckIfNearEnough(AAxis axisA, AAxis axisB)
    {
        if(
            ((axisA.GetAxisBaseGlobal() - axisB.GetAxisBaseGlobal()).magnitude < triggerMetaVisDistance
            || (axisA.GetAxisBaseGlobal() - axisB.GetAxisTipGlobal()).magnitude < triggerMetaVisDistance)
            &&
            ((axisA.GetAxisTipGlobal() - axisB.GetAxisTipGlobal()).magnitude < triggerMetaVisDistance
            || (axisA.GetAxisTipGlobal() - axisB.GetAxisTipGlobal()).magnitude < triggerMetaVisDistance)
            )
        {
            return true;
        } else
        {
            return false;
        }
    }

    /// <summary>
    /// Checks for a dataset which contains both represented attributes and 
    /// returns it's ID.
    /// </summary>
    /// <param name="axisA"></param>
    /// <param name="axisB"></param>
    /// <returns>Whether such a dataset exists</returns>
    public bool CheckIfCompatible(AAxis axisA, AAxis axisB, out int dataSetID)
    {
        if(axisA.attributeStats.name == axisB.attributeStats.name)
        {
            Debug.LogWarning("Given axes represent the same dimension and can't span a useful MetaVis.");
            dataSetID = -1;
            return false;
        }

        for(int id = 0; id < dataProvider.dataSets.Length; id++)
        {
            DataSet data = dataProvider.dataSets[id];

            bool existsA = data.attributeIDsByName.ContainsKey(axisA.attributeStats.name);
            bool existsB = data.attributeIDsByName.ContainsKey(axisB.attributeStats.name);
            bool correctLoMA = data.GetTypeOf(axisA.attributeStats.name) == axisA.attributeStats.type;
            bool correctLoMB = data.GetTypeOf(axisB.attributeStats.name) == axisB.attributeStats.type;

            if(existsA && existsB && correctLoMA && correctLoMB)
            {
                dataSetID = id;
                return true;
            }
        }

        dataSetID = -1;
        return false;
    }

    public enum MetaVisType
    {
        FlexibleLinkedAxis, ImmersiveAxis, 
    }

    public MetaVisType WhichMetaVis(AAxis axisA, AAxis axisB, int dataSetID)
    {
        return MetaVisType.ImmersiveAxis;
    }

    public GameObject SpanMetaVisImmersiveAxis(AAxis axisA, AAxis axisB, int dataSetID)
    {
        return GenerateImmersiveAxes(dataSetID, new string[] { axisA.attributeStats.name, axisB.attributeStats.name }, axisA, axisB);
    }



    ///////////////////////////////////////////////////////////////////////////////////////////
    // ........................................................................ MetaVis FACTORY

    /// <summary>
    /// Generates a 2D Parallel Coordinates Plot for n attributes.
    /// </summary>
    /// <param name="dataSetID"></param>
    /// <param name="variables">Attributes to be present in the PCP.</param>
    /// <returns>GameObject containing the anchored visualization.</returns>
    public GameObject GenerateImmersiveAxes(int dataSetID, string[] variables, AAxis axisA, AAxis axisB)
    {
        try
        {
            var ds = dataProvider.dataSets[dataSetID];

            var factory = ServiceLocator.instance.FactoryMetaVis;
            var vis = factory.CreateFlexiblePCP(ds, variables, axisA, axisB);
            
            return vis;
        } catch(Exception e)
        {
            Debug.Log("Creation of requested MetaVis for variables " + variables + " failed.");
            Debug.LogError(e.Message);
            Debug.LogError(e.StackTrace);
            return new GameObject("Creation Failed");
        }
    }

    // ........................................................................ INNER CLASSES
    public class MetaVisKey
    {
        public AAxis axisA, axisB;

        public MetaVisKey(AAxis axisA, AAxis axisB)
        {
            this.axisA = axisA;
            this.axisB = axisB;
        }

        public override bool Equals(object obj)
        {
            if(obj is MetaVisKey)
            {
                var other = obj as MetaVisKey;
                if(other.axisA.Equals(axisA) && other.axisB.Equals(axisB))
                    return true;
                else if(other.axisA.Equals(axisB) && other.axisB.Equals(axisA))
                    return true;
                else
                    return false;
            } else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            var hashCode = -624926263;
            hashCode = hashCode + EqualityComparer<AAxis>.Default.GetHashCode(axisA);
            hashCode = hashCode + EqualityComparer<AAxis>.Default.GetHashCode(axisB);
            return hashCode;
        }
    }
}
