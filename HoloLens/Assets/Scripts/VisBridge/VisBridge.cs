using System.Collections.Generic;
using UnityEngine;
using GraphicalPrimitive;
using Model;

namespace VisBridge
{
    /// <summary>
    /// VisBridge class. Represents a VisBridge, which connects multiple
    /// graphical primitives of one or more visualizations, if they
    /// directly or indirectly represent the same information object.
    /// </summary>
    public class VisBridge : MonoBehaviour, IObserver<AGraphicalPrimitive>
    {
        // .................................................................... Unit Components
        public AGraphicalPrimitive centerSphere;
        public VisBridgeBranch branchPrefab;

        // .................................................................... Properties
        private IDictionary<AGraphicalPrimitive, VisBridgeBranch> bridgeBranches
            = new Dictionary<AGraphicalPrimitive, VisBridgeBranch>();

        // List of information objects currently active in the VisBridge
        private IList<InfoObject> connectedInfObs 
            = new List<InfoObject>();

        private IList<AGraphicalPrimitive> subscriptions = new List<AGraphicalPrimitive>();

        // .................................................................... Unity
        // Use this for initialization
        public void Awake()
        {
            centerSphere.Brush(Color.green);
        }

        void Update()
        {
            // Update kartesian center
            var center = new Vector3();
            var activeEndpoints = 0;
            foreach(var prim in bridgeBranches.Keys)
            {
                if(prim != null && prim.isActiveAndEnabled)
                {
                    center += prim.transform.position;
                    activeEndpoints++;
                }
            }

            if(activeEndpoints != 0)
            {
                center /= activeEndpoints;
                centerSphere.transform.position = center;
            }
        }


        // .................................................................... Methods

        /// <summary>
        /// Whether this VisBridge connects representative graphical primitives
        /// of the given information object.
        /// </summary>
        /// <param name="o">information object in question</param>
        /// <returns>whether it is connected</returns>
        public bool Connects(InfoObject o)
        {
            return connectedInfObs.Contains(o);
        }

        /// <summary>
        /// Draws VisBridge branches to representative GPs of the given
        /// information object.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="primitives"></param>
        public void Connect(InfoObject o, Color color)
        {
            connectedInfObs.Add(o);
            var prims = Services.VisBridgeSys().GetRepresentativePrimitivesOf(o);

            foreach(var prim in prims)
            {
                if(!bridgeBranches.ContainsKey(prim) && prim != null)
                {
                    var bridge = Instantiate(branchPrefab);
                    bridge.Init(prim, centerSphere, color);
                    bridgeBranches.Add(prim, bridge);
                    bridge.transform.parent = gameObject.transform;
                    prim.Brush(color);
                    Observe(prim);
                }
            }
        }

        /// <summary>
        /// Removes connections to all representative graphical
        /// primitives of the given information object.
        /// </summary>
        /// <param name="o"></param>
        public void RemoveInfoObject(InfoObject o)
        {
            connectedInfObs.Remove(o);
            var prims = Services.VisBridgeSys().GetRepresentativePrimitivesOf(o);
            RemovePrimitives(prims);
        }

        /// <summary>
        /// Removes connections to all representative GPs.
        /// </summary>
        /// <param name="primitives">primitives to cut off</param>
        /// <returns>No primitives left. Bridge destroyed.</returns>
        private bool RemovePrimitives(IList<AGraphicalPrimitive> primitives)
        {
            foreach(var prim in primitives)
            {
                if(bridgeBranches.ContainsKey(prim))
                {
                    Destroy(bridgeBranches[prim].gameObject);
                    bridgeBranches.Remove(prim);
                    prim.Unbrush();
                }
            }

            if(bridgeBranches.Count == 0)
            {
                return true;
            } else
            {
                return false;
            }
        }

        

        public void Dispose()
        {
            foreach(var bridgePart in bridgeBranches.Values)
            {
                Destroy(bridgePart.gameObject);
            }
            foreach(var sub in subscriptions)
            {
                sub.Unsubscribe(this);
            }
            subscriptions.Clear();
            Destroy(gameObject);
        }


        public void OnDispose(AGraphicalPrimitive observable)
        {
            RemovePrimitives(new List<AGraphicalPrimitive>() { observable });
        }

        public void OnChange(AGraphicalPrimitive observable)
        {
            // Nothing
        }

        public void Observe(AGraphicalPrimitive observable)
        {
            observable.Subscribe(this);
            subscriptions.Add(observable);
        }

        public void Ignore(AGraphicalPrimitive observable)
        {
            subscriptions.Remove(observable);
        }
    }
}
