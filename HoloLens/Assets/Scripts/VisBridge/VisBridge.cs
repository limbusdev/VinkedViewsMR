using System.Collections.Generic;
using UnityEngine;
using GraphicalPrimitive;
using Model;
using ETV;

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

        private IDictionary<AETV, IList<AGraphicalPrimitive>> etv2prim = new Dictionary<AETV, IList<AGraphicalPrimitive>>();
        private IDictionary<AETV, AGraphicalPrimitive> etv2bundleNode = new Dictionary<AETV, AGraphicalPrimitive>();

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

                    // Edge Bundling
                    
                    bridge.Init(prim, centerSphere, color);
                    bridgeBranches.Add(prim, bridge);
                    bridge.transform.parent = gameObject.transform;
                    prim.Brush(color);
                    Observe(prim);
                }
            }

            foreach(var etv in etv2prim.Keys)
            {
                CreateBundledVisBridgeBranches(etv2prim[etv]);
            }
        }

        private Vector3 CreateBundledVisBridgeBranches(IList<AGraphicalPrimitive> bundlePrims)
        {
            if(!(bundlePrims[0].Base() == null) && !etv2bundleNode.ContainsKey(bundlePrims[0].Base()))
            {
                etv2bundleNode.Add(bundlePrims[0].Base(), Instantiate(centerSphere));
            }

            Vector3 bundlePoint = Vector3.zero;

            // TODO create only one per bundle
            var bundleObject = etv2bundleNode[bundlePrims[0].Base()];

            
            int counter = 0;
            foreach(var p in bundlePrims)
            {
                var branch = bridgeBranches[p];
                branch.target = bundleObject;
                branch.Update();
                bundlePoint += branch.origin.visBridgePort.transform.position;
                counter++;
            }
            bundlePoint /= counter;
            bundlePoint += bundlePoint;
            bundlePoint += centerSphere.transform.position;
            bundlePoint /= 3f;

            bundleObject.transform.position = bundlePoint;

            // Create branch from bundle to visBridge-center
            var centerBranch = Instantiate(branchPrefab);
            centerBranch.Init(bundleObject, centerSphere, Color.green);

            return bundlePoint;
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
            foreach(var prims in etv2prim.Values)
            {
                foreach(var sub in prims)
                {
                    sub.Unsubscribe(this);
                }
            }
            etv2prim.Clear();
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

        public void Observe(AGraphicalPrimitive prim)
        {
            prim.Subscribe(this);
            // Remember to which etv it belongs
            if(prim.Base() != null && !etv2prim.ContainsKey(prim.Base()))
            {
                etv2prim.Add(prim.Base(), new List<AGraphicalPrimitive>());
            }
            etv2prim[prim.Base()].Add(prim);
        }

        public void Ignore(AGraphicalPrimitive observable)
        {
            if(observable.Base() != null && etv2prim.ContainsKey(observable.Base()))
            {
                etv2prim[observable.Base()].Remove(observable);
            }
        }
    }
}
