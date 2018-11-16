using ETV;
using GraphicalPrimitive;
using Model;
using System.Collections.Generic;
using UnityEngine;

namespace VisBridges
{
    /// <summary>
    /// A VisBridgeTree consists of the trunk, which connects the local center with
    /// the VisBridgeCenter and the single branches, which connect the local center
    /// with the brushed primitives of the same ETV.
    /// </summary>
    public class VisBridgeTree : MonoBehaviour, IPrimitiveObserver
    {
        public VisBridgeBranch branchPrefab;
        public VisBridge root;
        
        private IDictionary<AGraphicalPrimitive, VisBridgeBranch> trunk
            = new Dictionary<AGraphicalPrimitive, VisBridgeBranch>();
        private IDictionary<AGraphicalPrimitive, VisBridgeBranch> branches
            = new Dictionary<AGraphicalPrimitive, VisBridgeBranch>();

        public AGraphicalPrimitive center { get; private set; }
        private IDictionary<AGraphicalPrimitive, AGraphicalPrimitive> offsets
            = new Dictionary<AGraphicalPrimitive, AGraphicalPrimitive>();
        
        private float offsetDist = .01f;
        
        public void Assign(VisBridge visBridge)
        {
            root = visBridge;
        }

        public Vector3 GetKartesianCenterOfPrims()
        {
            var c = Vector3.zero;
            int i = 0;
            foreach(var p in branches.Keys)
            {
                if(p.isActiveAndEnabled)
                {
                    c += p.transform.position;
                    i++;
                }
            }

            c = ((i == 0) ? Vector3.zero : (c /= branches.Count));
            return c;
        }

        // .................................................................... Methods
        public void Connect(VisBridge bridge, InfoObject infO, AGraphicalPrimitive prim, Color color)
        {
            Observe(prim);

            if(center == null)
            {
                center = Services.PrimFactory3D().CreatePhantomPrimitive();
            }

            var bridgeCenter = bridge.centroids[infO];

            var offset = Services.PrimFactory3D().CreatePhantomPrimitive();
            offset.transform.parent = center.transform;
            offsets.Add(prim, offset);

            // Connect visBridgeCenter with treeCenter
            var trunkWedge = Instantiate(branchPrefab);
            trunkWedge.Init(bridgeCenter, offset, color);
            trunk.Add(prim, trunkWedge);

            var branch = Instantiate(branchPrefab);
            branch.Init(prim, offset, color);
            branches.Add(prim, branch);

            UpdateCenter();
        }

        private void UpdateCenter()
        {
            // Update Offsets
            int counter = 0;
            foreach(var key in offsets.Keys)
            {
                var offset = offsets[key];
                offset.transform.localPosition = new Vector3(0, counter * offsetDist, 0);
                counter++;
            }

            var c = Vector3.zero;
            foreach(var p in branches.Keys)
            {
                c += p.visBridgePort.transform.position;
            }
            c /= branches.Keys.Count;
            c += c; // double weight
            c += root.center.transform.position;
            c /= 3f;
            center.transform.position = c;
        }

        // .................................................................... IPrimitiveObserver
        public void Ignore(AGraphicalPrimitive observable)
        {
            var trk = trunk[observable];
            var brn = branches[observable];
            var ofs = offsets[observable];

            trunk.Remove(observable);
            branches.Remove(observable);
            offsets.Remove(observable);

            Destroy(trk);
            Destroy(brn);
            Destroy(ofs);
        }

        public void Observe(AGraphicalPrimitive prim)
        {
            prim.Subscribe(this);
        }

        public void OnChange(AGraphicalPrimitive observable)
        {
            UpdateCenter();
        }

        public void OnDispose(AGraphicalPrimitive observable)
        {
            Ignore(observable);
        }
    }
}