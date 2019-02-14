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
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

/// <summary>
/// Main class for visualization generation from databases.
/// </summary>
public class VisualizationFactory : MonoBehaviour
{
    // ........................................................................ Fields to populate in editor
    public static bool onServer = false;


    // Prefabs
    public Transform NewETVPlaceHolder;
    public GameObject ObjectCollection;
    public GameObject CubeIconVariable;
    public GameObject NetworkAnchorPrefab;

    [SerializeField]
    public Material lineMaterial;
    public VisFactoryInteractionReceiver interactionReceiver;

    public AETVFactoryMethod[] factoryMethodHooks;

    public IDictionary<VisType, AETVFactoryMethod> generators;

    private bool initialized = false;



    // ........................................................................ Private properties

    private IList<GameObject> activeVisualizations;



    // ........................................................................ MonoBehaviour methods
    public void Initialize()
    {
        if(!initialized)
        {
            activeVisualizations = new List<GameObject>();
            generators = new Dictionary<VisType, AETVFactoryMethod>();
            foreach(var method in factoryMethodHooks)
            {
                var key = VisType.SingleAxis3D;
                Enum.TryParse(method.gameObject.name, true, out key);
                generators.Add(key, method);
            }
            initialized = true;
        }
    }

    // ........................................................................ Visualization generation

    /// <summary>
    /// Generates visualizations from one attribute.
    /// </summary>
    /// <param name="dataSetID">data set to use</param>
    /// <param name="variable">attribute to visualize</param>
    /// <param name="visType">visualization type to generate</param>
    /// <returns></returns>
    public GameObject GenerateVisFrom(int dataSetID, string variable, VisType visType, bool persist=true)
    {
        return GenerateVisFrom(dataSetID, new string[] { variable }, visType, persist);
    }

    /// <summary>
    /// Generates visualizations from several attributes.
    /// </summary>
    /// <param name="dataSetID">data set to use</param>
    /// <param name="variable">attribute to visualize</param>
    /// <param name="visType">visualization type to generate</param>
    /// <returns></returns>
    public GameObject GenerateVisFrom(int dataSetID, string[] variables, VisType visType, bool persist = true)
    {
        if(!initialized)
        {
            Initialize();
        }

        try
        {
            var vis = generators[visType].GenerateVisualization(dataSetID, variables);
            
            vis = Services.ETVFactory2D().PutETVOnAnchor(vis);
            
            AddNetworkAnchor(vis, dataSetID, variables, visType);


            Services.Persistence().PersistETV(vis, dataSetID, variables, visType);

            vis.transform.parent = GameObject.FindGameObjectWithTag("RootWorldAnchor").transform;
            vis.transform.position = NewETVPlaceHolder.position;

            return vis;
        } catch(Exception e)
        {
            Debug.Log("Creation of requested Visualization failed.");
            Debug.LogError(e.Message);
            Debug.LogError(e.StackTrace);
            return new GameObject("Creation Failed");
        }
    }



    // ........................................................................ internal creation methods
    
        
        
    public void AddNetworkAnchor(GameObject etv, int dataSetID, string[] attributes, VisType visType)
    {
        if(onServer)
        {
            var networkAnchor = Instantiate(NetworkAnchorPrefab);
            if(networkAnchor.GetComponent<NetworkAnchor>() != null)
            {
                NetworkServer.Spawn(networkAnchor);
                networkAnchor.GetComponent<NetworkAnchor>().Init(dataSetID, attributes, visType);
                networkAnchor.GetComponent<NetworkAnchor>().ETV = etv;
            }
        }

    }




    // ........................................................................ Helper methods

    /// <summary>
    /// List which visualizations are suitable for the combination of provided attributes.
    /// </summary>
    /// <param name="dataSetID">ID of DataSet to use.</param>
    /// <param name="variables">Names of the attributes to visualize.</param>
    /// <returns>List of suitable visualizations.</returns>
    public VisType[] ListPossibleVisualizations(int dataSetID, string[] variables)
    {
        int[] nomIDs, ordIDs, ivlIDs, ratIDs;

        var dataProvider = Services.DataBase();
        AttributeProcessor.ExtractAttributeIDs(dataProvider.dataSets[dataSetID], variables, out nomIDs, out ordIDs, out ivlIDs, out ratIDs);

        var suitableVisTypes = new List<VisType>();

        foreach(var visType in generators.Keys)
        {
            if(generators[visType].CheckIfSuitable(dataSetID, variables))
            {
                suitableVisTypes.Add(visType);
            }
        }

        if(suitableVisTypes.Count == 0)
            return new VisType[] { VisType.PCP2D };
        else
            return suitableVisTypes.ToArray();
    }

    
    public void AddNewVisualization(GameObject visualization)
    {
        if(visualization.GetComponent<ETVAnchor>() != null)
        {
            activeVisualizations.Add(visualization);
        } else
        {
            Debug.LogWarning("Given GameObject is not an anchored visualization!");
        }
    }

    
}


