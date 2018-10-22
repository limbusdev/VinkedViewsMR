using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphicalPrimitive;
using Model;

namespace VisBridge
{
    public class MultiVisBridge : MonoBehaviour
    {
        public AGraphicalPrimitive visBridgeCenterSphere;
        public ObjectBasedVisBridge obvbPrefab;

        private IDictionary<AGraphicalPrimitive, ObjectBasedVisBridge> visBridges;
        private bool initialized = false;

        private IList<InfoObject> activeInfoObjects;

        // Use this for initialization
        void Start()
        {

        }

        void FixedUpdate()
        {
            if(initialized)
            {
                // Calculate kartesian center
                var center = new Vector3();
                var activeEndpoints = 0;
                foreach(var prim in visBridges.Keys)
                {
                    if(prim.isActiveAndEnabled)
                    {
                        center += prim.transform.position;
                        activeEndpoints++;
                    }
                }
                center /= activeEndpoints;

                visBridgeCenterSphere.transform.position = center;
            }
        }

        public bool HasInfoObject(InfoObject o)
        {
            return activeInfoObjects.Contains(o);
        }

        public void AddMorePrimitives(InfoObject o, AGraphicalPrimitive[] primitives)
        {
            if(!initialized)
                Init();

            activeInfoObjects.Add(o);

            foreach(var prim in primitives)
            {
                if(!visBridges.ContainsKey(prim))
                {
                    var bridge = Instantiate(obvbPrefab);
                    bridge.Init(prim, visBridgeCenterSphere);
                    visBridges.Add(prim, bridge);
                    bridge.transform.parent = gameObject.transform;
                    prim.Brush(Color.green);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="primitives"></param>
        /// <returns>No primitives left. Bridge destroyed.</returns>
        public bool RemovePrimitives(InfoObject o, AGraphicalPrimitive[] primitives)
        {
            activeInfoObjects.Remove(o);

            foreach(var prim in primitives)
            {
                if(visBridges.ContainsKey(prim))
                {
                    Destroy(visBridges[prim].gameObject);
                    visBridges.Remove(prim);
                }
            }

            if(visBridges.Count == 0)
            {
                Dispose();
                return true;
            } else
            {
                return false;
            }
        }

        public void Init()
        {
            visBridges = new Dictionary<AGraphicalPrimitive, ObjectBasedVisBridge>();
            activeInfoObjects = new List<InfoObject>();
            initialized = true;
            visBridgeCenterSphere.Brush(Color.green);
        }

        public void Dispose()
        {
            foreach(var bridgePart in visBridges.Values)
            {
                Destroy(bridgePart.gameObject);
            }
            Destroy(gameObject);
        }
    }
}
