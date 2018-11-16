using System.Collections.Generic;
using UnityEngine;
using GraphicalPrimitive;
using Model;
using ETV;

namespace VisBridges
{
    /// <summary>
    /// VisBridge class. Represents a VisBridge, which connects multiple
    /// graphical primitives of one or more visualizations, if they
    /// directly or indirectly represent the same information object.
    /// </summary>
    public class VisBridge : MonoBehaviour, IObserver<VisBridgeTree>
    {
        
        // .................................................................... Unit Components
        public AGraphicalPrimitive center;
        public VisBridgeTree VisBridgeTreePrefab;
        public VisBridgeBranch branchPrefab;

        // .................................................................... Properties
        private IDictionary<AETV, VisBridgeTree> trees = new Dictionary<AETV, VisBridgeTree>();

        // List of information objects currently active in the VisBridge
        public IList<InfoObject> connectedInfObs = new List<InfoObject>();
        public IList<AGraphicalPrimitive> centroids = new List<AGraphicalPrimitive>();

        // .................................................................... Unity
        // Use this for initialization
        public void Awake()
        {
            center.Brush(Color.green);
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

            // Setup centroids
            int index = connectedInfObs.IndexOf(o);
            if(index >= centroids.Count || centroids[index] == null)
            {
                var centroid = Services.PrimFactory3D().CreateBoxPrimitive();
                centroid.SetColor(color, color);
                centroid.transform.parent = center.transform;
                centroid.transform.localPosition = new Vector3(0,VisBridgeSystem.offsetDist*10*index,0);
                centroid.transform.localScale = new Vector3(1, VisBridgeSystem.offsetDist*10, 1);
                centroids.Insert(index, centroid);
            }
            
            // Get all visual primitives that represent the given InfoObject
            var prims = Services.VisBridgeSys().GetRepresentativePrimitivesOf(o);

            foreach(var prim in prims)
            {
                // Add tree if neccessary
                VisBridgeTree tree;
                if(!trees.ContainsKey(prim.Base()))
                {
                    tree = Instantiate(VisBridgeTreePrefab);
                    tree.Assign(this, prim.Base());
                    trees.Add(prim.Base(), tree);
                    tree.Subscribe(this);
                } else
                {
                    tree = trees[prim.Base()];
                }

                // Add primitive to tree
                tree.Connect(this, o, prim, color, index);
                prim.Brush(color);
            }
        }
        

        /// <summary>
        /// Removes connections to all representative graphical
        /// primitives of the given information object.
        /// </summary>
        /// <param name="o"></param>
        public void Disconnect(InfoObject o)
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
            // TODO
            return false;
        }

        

        public void Dispose()
        {
            // TODO
        }


        // .................................................................... IObserver<VisBridgeTree>
        public void Observe(VisBridgeTree observable)
        {
            observable.Subscribe(this);
        }

        public void Ignore(VisBridgeTree observable)
        {
            throw new System.NotImplementedException();
        }

        public void OnDispose(VisBridgeTree observable)
        {
            trees.Remove(observable.ID);
        }

        public void OnChange(VisBridgeTree observable)
        {
            // Update kartesian center
            var c = Vector3.zero;
            foreach(var tree in trees.Values)
            {
                c += tree.GetKartesianCenterOfPrims();
            }
            c = (trees.Count == 0) ? Vector3.zero : c / trees.Count;
            center.transform.position = c;
        }
    }
}
