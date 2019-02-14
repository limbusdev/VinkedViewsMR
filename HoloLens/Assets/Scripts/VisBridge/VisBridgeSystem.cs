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
using GraphicalPrimitive;
using Model;
using System.Collections.Generic;
using System.Linq;

namespace VisBridges
{
    public class VisBridgeSystem : AVisBridgeSystem
    {
        public static float offsetDist = .005f;
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
            {
                visBridges[infO.dataSetID].Disconnect(infO);
                var prims = Services.VisBridgeSys().GetRepresentativePrimitivesOf(infO);
                foreach(var p in prims.ToList())
                {
                    p.Disconnect();
                }
            } else // draw a new one
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
                visBridges[o.dataSetID].Connect(o, o.dataSet.colorTableBrushing[o], p);
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
            if(prim.disposed)
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
        }

        public override void OnChange(AGraphicalPrimitive observable)
        {
            // Remove visbridge twig, if prim is disabled,
            // leave it, if it is invisible
        }

        
    }
}