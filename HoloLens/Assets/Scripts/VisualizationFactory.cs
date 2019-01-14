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
            vis.transform.localPosition = NewETVPlaceHolder.position - GameObject.FindGameObjectWithTag("RootWorldAnchor").transform.position;

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


