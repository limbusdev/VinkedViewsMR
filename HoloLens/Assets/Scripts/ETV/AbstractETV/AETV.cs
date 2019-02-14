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
using System.Collections.Generic;
using GraphicalPrimitive;
using Model;
using UnityEngine;

namespace ETV
{
    public abstract class AETV : MonoBehaviour, IEuclideanTransformableView, IObservable<AETV>
    {
        [SerializeField]
        public Transform Anchor;
        
        protected DataSet Data { get; set; }

        public IDictionary<AxisDirection, IList<AAxis>> Axes { get; private set; } 
            = new Dictionary<AxisDirection, IList<AAxis>>();
        public IDictionary<InfoObject, AGraphicalPrimitive> infoObject2primitive { get; private set; } 
            = new Dictionary<InfoObject, AGraphicalPrimitive>();
        public IDictionary<string, IList<AAxis>> registeredAxes = new Dictionary<string, IList<AAxis>>();
        
        public abstract void DrawGraph();
        public abstract void SetUpAxes();
        public abstract void UpdateETV();
        public virtual AGraphicalPrimitiveFactory AxisFactory()
        {
            return Services.PrimFactory2D();
        }
        
        public bool isMetaVis { get; private set; } = false;
        public bool disposed { get; private set; } = false;
        
        public ETVColorSchemes colorScheme { get; set; }

        public virtual void ChangeColoringScheme(ETVColorSchemes scheme)
        {
            this.colorScheme = scheme;
        }

        public virtual void Init(DataSet data, bool isMetaVis)
        {
            this.Data = data;
            this.isMetaVis = isMetaVis;
        }

        public void SetAxisLabels(AxisDirection axisDirection, string axisVariable)
        {
            foreach(var axis in Axes[axisDirection])
            {
                axis.labelVariableText = axisVariable;
                axis.UpdateAxis();
            }
        }

        public void AddBarChartAxis(string attributeName, AxisDirection dir)
        {
            var axis = AxisFactory().CreateAutoTickedAxis(attributeName, AxisDirection.X, Data);
            axis.transform.parent = Anchor;

            if(!Axes.ContainsKey(dir))
            {
                Axes.Add(dir, new List<AAxis>());
            }
            Axes[dir].Add(axis.GetComponent<AAxis>());

            RegisterAxis(axis.GetComponent<AAxis>());
        }

        public AAxis AddAxis(string attributeName, AxisDirection dir, float length=1f)
        {
            var lom = Data.TypeOf(attributeName);

            GameObject axis;
            switch(lom)
            {
                case LoM.NOMINAL:
                    axis = AxisFactory().CreateFixedLengthAutoTickedAxis(attributeName, length, dir, Data);
                    break;
                case LoM.ORDINAL:
                    axis = AxisFactory().CreateFixedLengthAutoTickedAxis(attributeName, length, dir, Data);
                    break;
                case LoM.INTERVAL:
                    axis = AxisFactory().CreateAutoTickedAxis(attributeName, dir, Data);
                    break;
                default: // RATIO
                    axis = AxisFactory().CreateAutoTickedAxis(attributeName, dir, Data);
                    break;
            }
            axis.transform.parent = Anchor;


            if(!Axes.ContainsKey(dir))
            {
                Axes.Add(dir, new List<AAxis>());
            }
            Axes[dir].Add(axis.GetComponent<AAxis>());

            RegisterAxis(axis.GetComponent<AAxis>());

            return axis.GetComponent<AAxis>();
        }

        public void RegisterAxis(AAxis axis)
        {
            if(!registeredAxes.ContainsKey(axis.attributeName))
            {
                registeredAxes.Add(axis.attributeName, new List<AAxis>());
            }
            registeredAxes[axis.attributeName].Add(axis);
            axis.Assign(this);

            if(!isMetaVis)
            {
                Services.MetaVisSys().Observe(axis.GetComponent<AAxis>());
            }
        }

        public AAxis GetAxis(AxisDirection dir)
        {
            return Axes[dir][0].GetComponent<AAxis>();
        }
        

        public void AddAggregatedAxis(string attributeName, AxisDirection dir, out float max, out float length)
        {
            var lom = Data.TypeOf(attributeName);

            switch(lom)
            {
                case LoM.NOMINAL:
                    var mea = Data.nominalStatistics[attributeName];
                    length = (mea.numberOfUniqueValues + 1) * .15f;
                    max = mea.distMax;
                    break;
                default:
                    var mea2 = Data.ordinalStatistics[attributeName];
                    length = (mea2.numberOfUniqueValues + 1) * .15f;
                    max = mea2.distMax;
                    break;
            }

            var yAxis = AxisFactory().CreateAutoTickedAxis("Amount", max);
            yAxis.transform.parent = Anchor;
            var comp = yAxis.GetComponent<AAxis>();

            comp.Assign(this);


            if(!Axes.ContainsKey(AxisDirection.Y))
            {
                Axes.Add(AxisDirection.Y, new List<AAxis>());
            }
            Axes[AxisDirection.Y].Add(yAxis.GetComponent<AAxis>());
        }

        public void RememberRelationOf(InfoObject o, AGraphicalPrimitive p)
        {
            infoObject2primitive.Add(o, p);
            p.Assign(this);
            Services.VisBridgeSys().RegisterGraphicalPrimitive(o, p);
        }

        public virtual void Dispose()
        {
            gameObject.SetActive(false);
            disposed = true;

            foreach(var axisKey in Axes.Keys)
            {
                var axis = Axes[axisKey];
                foreach(var a in axis)
                {
                    a.Dispose();
                }
            }

            foreach(var prim in infoObject2primitive.Values)
            {
                prim.Dispose();
            }

            foreach(var o in observers)
            {
                o.OnDispose(this);
            }
            
            Destroy(gameObject);
        }


        // .................................................................... IObservable<AETV>
        private IList<IObserver<AETV>> observers
            = new List<IObserver<AETV>>();

        public void Subscribe(IObserver<AETV> observer)
        {
            if(!observers.Contains(observer))
                observers.Add(observer);
        }

        public void Unsubscribe(IObserver<AETV> observer)
        {
            if(observers.Contains(observer))
                observers.Remove(observer);
        }

        
        void OnEnable()
        {
            foreach(var o in observers)
                o.OnChange(this);
        }

        public virtual void SetVisibility(bool visible)
        {
            if(gameObject != null)
            {
                int layer = visible ? LayerMask.NameToLayer("Default") : LayerMask.NameToLayer("Invisible");

                gameObject.layer = layer;
                foreach(var t in gameObject.GetComponentsInChildren<Transform>())
                    t.gameObject.layer = layer;
            }
        }

        void OnDisable()
        {
            if(!this.enabled)
            {
                foreach(var o in observers)
                    o.OnChange(this);
            }
        }
    }
}
