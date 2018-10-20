using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GraphicalPrimitive;

namespace VisBridge
{
    public class MultiVisBridge : MonoBehaviour
    {
        public AGraphicalPrimitive visBridgeCenterSphere;
        public ObjectBasedVisBridge obvbPrefab;

        private IDictionary<AGraphicalPrimitive, ObjectBasedVisBridge> visBridges;
        private bool initialized = false;

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

        public void Init(AGraphicalPrimitive[] primitives)
        {
            visBridges = new Dictionary<AGraphicalPrimitive, ObjectBasedVisBridge>();

            var visBridge = new GameObject("VisBridge");
            
            foreach(var prim in primitives)
            {
                if(!visBridges.ContainsKey(prim))
                {
                    var bridge = Instantiate(obvbPrefab);
                    bridge.Init(prim, visBridgeCenterSphere);
                    visBridges.Add(prim, bridge);

                    prim.Brush(Color.green);
                }
            }

            visBridgeCenterSphere.Brush(Color.green);

            initialized = true;
        }
    }
}
