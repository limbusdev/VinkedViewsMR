using ETV;
using GraphicalPrimitive;
using Model;
using System.Collections.Generic;

namespace VisBridge
{
    public class VisBridgeSystem : AVisBridgeSystem
    {
        // .................................................................... PREFABS
        public VisBridge visBridgePrefab;
        public DataProvider Data;

        // .................................................................... Properties
        private IDictionary<int, VisBridge> visBridges = new Dictionary<int, VisBridge>();
        

        // .................................................................... VisBridge generation
        
        /// <summary>
        /// Instantiates VisBridge for the given Data Set ID,
        /// if there isn't any.
        /// </summary>
        /// <param name="dataSetID"></param>
        private void InitVisBridgeFor(int dataSetID)
        {
            if(!visBridges.ContainsKey(dataSetID) || visBridges[dataSetID] == null)
            {
                visBridges.Add(dataSetID, Instantiate(visBridgePrefab));
            }
        }

        // .................................................................... AVisBridgeSystem Implementation

        public override void ToggleVisBridgeFor(InfoObject infO)
        {
            InitVisBridgeFor(infO.dataSetID);

            // ............................................ VisBridge aktivation/deactivation
            // if their already is a visbridge for the objects dataset, disable it
            if(visBridges[infO.dataSetID].Connects(infO))
                visBridges[infO.dataSetID].RemoveInfoObject(infO);
            else // draw a new one
            {
                var color = Data.dataSets[infO.dataSetID].colorTableBrushing[infO];
                visBridges[infO.dataSetID].Connect(infO, color);
            }
                
        }

        public override void ToggleVisBridgeFor(AGraphicalPrimitive prim)
        {
            // VisBridge aktivation/deactivation
            // Get all represented Information Objects
            var infOs = representedInfoObjects[prim];

            // for every represented information object, toggle it in the visbridge
            foreach(var infO in infOs)
            {
                ToggleVisBridgeFor(infO);
            }
        }
        

        // .................................................................... IObserver

        private IDictionary<InfoObject, IList<AGraphicalPrimitive>> observedVisElements 
            = new Dictionary<InfoObject, IList<AGraphicalPrimitive>>();
        private IDictionary<AGraphicalPrimitive, IList<InfoObject>> representedInfoObjects 
            = new Dictionary<AGraphicalPrimitive, IList<InfoObject>>();

        public override void RegisterGraphicalPrimitive(InfoObject o, AGraphicalPrimitive p)
        {
            p.Subscribe(this);

            // Create lookup table entries
            if(!observedVisElements.ContainsKey(o))
                observedVisElements.Add(o, new List<AGraphicalPrimitive>());
            observedVisElements[o].Add(p);

            if(!representedInfoObjects.ContainsKey(p))
                representedInfoObjects.Add(p, new List<InfoObject>());
            representedInfoObjects[p].Add(o);
            
            // Add to visbridge if there is one
            if(visBridges.ContainsKey(o.dataSetID) && visBridges[o.dataSetID].Connects(o))
            {
                ToggleVisBridgeFor(o);
                ToggleVisBridgeFor(o);
            }
        }

        public override IList<InfoObject> GetInfoObjectsRepresentedBy(AGraphicalPrimitive p)
        {
            return representedInfoObjects[p];
        }

        public override IList<AGraphicalPrimitive> GetRepresentativePrimitivesOf(InfoObject o)
        {
            return observedVisElements[o];
        }

        public override void OnDispose(AGraphicalPrimitive prim)
        {
            foreach(var infO in representedInfoObjects[prim])
            {
                if(observedVisElements.ContainsKey(infO))
                {
                    if(observedVisElements[infO].Contains(prim))
                    {
                        observedVisElements[infO].Remove(prim);
                    }
                }
            }

            if(representedInfoObjects.ContainsKey(prim))
            {
                representedInfoObjects.Remove(prim);
            }            
        }

        public override void OnChange(AGraphicalPrimitive observable)
        {
            // Remove visbridge twig, if prim is disabled,
            // leave it, if it is invisible
        }

        
    }
}