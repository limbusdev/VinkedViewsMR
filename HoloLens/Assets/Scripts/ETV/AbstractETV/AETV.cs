using System.Collections.Generic;
using GraphicalPrimitive;
using Model;
using UnityEngine;

namespace ETV
{
    public abstract class AETV : MonoBehaviour, IEuclideanTransformableView
    {
        [SerializeField]
        public Transform Anchor;
        
        protected DataSet Data { get; set; }

        public IDictionary<AxisDirection, AAxis> Axes { get; private set; } 
            = new Dictionary<AxisDirection, AAxis>();
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
            Axes[axisDirection].labelVariableText = axisVariable;
            Axes[axisDirection].UpdateAxis();
        }

        public void AddBarChartAxis(string attributeName, AxisDirection dir)
        {
            var axis = AxisFactory().CreateAutoTickedAxis(attributeName, AxisDirection.X, Data);
            axis.transform.parent = Anchor;
            Axes.Add(dir, axis.GetComponent<AAxis>());

            RegisterAxis(axis.GetComponent<AAxis>());
        }

        public void AddAxis(string attributeName, AxisDirection dir, float length=1f)
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
            Axes.Add(dir, axis.GetComponent<AAxis>());

            RegisterAxis(axis.GetComponent<AAxis>());
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
            return Axes[dir].GetComponent<AAxis>();
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
            Axes.Add(AxisDirection.Y, yAxis.GetComponent<AAxis>());
        }

        public void RememberRelationOf(InfoObject o, AGraphicalPrimitive p)
        {
            infoObject2primitive.Add(o, p);
            Services.VisBridgeSys().RegisterGraphicalPrimitive(o, p);
        }

        public virtual void Dispose()
        {
            gameObject.SetActive(false);
            disposed = true;

            foreach(var axisKey in Axes.Keys)
            {
                var axis = Axes[axisKey];
                axis.Dispose();
            }

            foreach(var prim in infoObject2primitive.Values)
            {
                prim.Dispose();
            }
            
            Destroy(gameObject);
        }
    }
}
