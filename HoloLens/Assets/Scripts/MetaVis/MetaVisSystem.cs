using GraphicalPrimitive;
using Model;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MetaVisualization
{
    public class MetaVisSystem : AMetaVisSystem
    {
        [SerializeField]
        public DataProvider dataProvider;

        public static float triggerMetaVisDistance = 2f;

        private IList<AAxis> axes;
        private IDictionary<MetaVisKey, GameObject> metaVisualizations;

        private void Awake()
        {
            axes = new List<AAxis>();
            metaVisualizations = new Dictionary<MetaVisKey, GameObject>();
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
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

        public override void ObserveAxis(AAxis axis)
        {
            axes.Add(axis);
        }


        public override void StopObservationOf(AAxis axis)
        {
            axes.Remove(axis);
        }


        public override GameObject SpanMetaVisBetween(AAxis axisA, AAxis axisB, int dataSetID)
        {
            // Which metavisualization will do?
            switch(WhichMetaVis(axisA, axisB, dataSetID))
            {
                default:
                    return SpanMetaVisImmersiveAxis(axisA, axisB, dataSetID);
            }
        }

        public override bool CheckIfNearEnough(AAxis axisA, AAxis axisB)
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

        public override bool CheckIfCompatible(AAxis axisA, AAxis axisB, out int dataSetID)
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

        public override MetaVisType WhichMetaVis(AAxis axisA, AAxis axisB, int dataSetID)
        {
            return MetaVisType.ImmersiveAxis;
        }

        public override GameObject SpanMetaVisImmersiveAxis(AAxis axisA, AAxis axisB, int dataSetID)
        {
            return GenerateImmersiveAxes(dataSetID, new string[] { axisA.attributeStats.name, axisB.attributeStats.name }, axisA, axisB);
        }



        ///////////////////////////////////////////////////////////////////////////////////////////
        // ........................................................................ MetaVis FACTORY

        public override GameObject GenerateImmersiveAxes(int dataSetID, string[] variables, AAxis axisA, AAxis axisB)
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
    }
}