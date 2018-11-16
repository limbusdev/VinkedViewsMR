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
    public class VisBridgeTree : MonoBehaviour, IObserver<VisBridgeBranch>, IObservable<VisBridgeTree>
    {
        private struct BridgePath
        {
            public InfoObject ID;
            public VisBridgeBranch trunk;
            public VisBridgeBranch branch;
            public AGraphicalPrimitive offset;

            public BridgePath(InfoObject iD, VisBridgeBranch trunk, VisBridgeBranch branch, AGraphicalPrimitive offset)
            {
                ID = iD;
                this.trunk = trunk;
                this.branch = branch;
                this.offset = offset;
            }
        }

        public AETV ID;
        public VisBridgeBranch branchPrefab;
        public VisBridge root;

        private IDictionary<InfoObject, BridgePath> paths
            = new Dictionary<InfoObject, BridgePath>();
        
        public AGraphicalPrimitive center { get; private set; }
        
        
        public void Assign(VisBridge visBridge, AETV ID)
        {
            root = visBridge;
            this.ID = ID;
        }

        public Vector3 GetKartesianCenterOfPrims()
        {
            var c = Vector3.zero;
            int i = 0;
            foreach(var o in paths.Keys)
            {
                var p = paths[o].branch.target;

                if(p.isActiveAndEnabled)
                {
                    c += p.transform.position;
                    i++;
                }
            }

            c = ((i == 0) ? Vector3.zero : (c /= paths.Count));
            return c;
        }

        // .................................................................... Methods


        public bool Connects(InfoObject i)
        {
            return paths.ContainsKey(i);
        }

        public void Connect(VisBridge bridge, InfoObject infO, AGraphicalPrimitive prim, Color color)
        {
            if(paths.ContainsKey(infO))
                return;

            if(center == null)
            {
                center = Services.PrimFactory3D().CreatePhantomPrimitive();
            }

            var bridgeCenter = bridge.centroids[infO];

            var offset = Services.PrimFactory3D().CreateBoxPrimitive();
            offset.transform.parent = center.transform;
            offset.transform.localScale = new Vector3(.05f,.005f,.05f);
            offset.SetColor(color, color);

            // Connect visBridgeCenter with treeCenter
            var trunk = Instantiate(branchPrefab);
            trunk.Init(bridgeCenter, offset, color, infO);

            var branch = Instantiate(branchPrefab);
            branch.Init(offset, prim, color, infO);

            paths.Add(infO, new BridgePath(infO, trunk, branch, offset));
            Observe(branch);

            UpdateCenter();
        }

        public void UpdateCenter()
        {
            // Update Offsets
            foreach(var key in paths.Keys)
            {
                var path = paths[key];
                var offset = path.offset;
                int offIndex = root.order.IndexOf(key);
                offset.transform.localPosition = new Vector3(0, offIndex * VisBridgeSystem.offsetDist, 0);
            }

            var c = Vector3.zero;
            foreach(var key in paths.Keys)
            {
                var p = paths[key].branch.target;
                c += p.visBridgePort.transform.position;
            }

            c /= paths.Count;
            c *= 2; // double weight
            c += root.center.transform.position;
            c /= 3f;

            if(!float.IsNaN(c.x) && !float.IsNaN(c.y) && !float.IsNaN(c.z))
                center.transform.position = c;

            foreach(var o in observers)
                o.OnChange(this);
        }

        // .................................................................... IObserver<VisBridgeBranch>
        public void Observe(VisBridgeBranch observable)
        {
            observable.Subscribe(this);
        }

        public void Ignore(VisBridgeBranch observable)
        {
            // Nothing yet
        }

        public void OnDispose(VisBridgeBranch observable)
        {
            paths[observable.ID].trunk.Dispose();
            paths[observable.ID].offset.Dispose();
            paths.Remove(observable.ID);

            if(paths.Count == 0)
                Dispose();
        }

        public void OnChange(VisBridgeBranch observable)
        {
            UpdateCenter();
        }

        public void Dispose()
        {
            foreach(var o in observers)
                o.OnDispose(this);
            observers.Clear();
            Destroy(center);
            Destroy(gameObject);
        }


        // .................................................................... IObservable<VisBridgeTree>
        private IList<IObserver<VisBridgeTree>> observers
            = new List<IObserver<VisBridgeTree>>();

        public void Subscribe(IObserver<VisBridgeTree> observer)
        {
            if(!observers.Contains(observer))
            observers.Add(observer);
        }

        public void Unsubscribe(IObserver<VisBridgeTree> observer)
        {
            if(observers.Contains(observer))
                observers.Remove(observer);
        }
    }
}