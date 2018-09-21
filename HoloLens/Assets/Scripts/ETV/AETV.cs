using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphicalPrimitive;
using Model;
using UnityEngine;

namespace ETV
{
    public abstract class AETV : MonoBehaviour, IEuclideanTransformableView
    {
        protected DataSet data { get; set; }

        public IDictionary<AxisDirection, AAxis> axes { get; set; }

        public abstract void ChangeColoringScheme(ETVColorSchemes scheme);
        public abstract void DrawGraph();
        public abstract void SetUpAxis();
        public abstract void UpdateETV();
        public abstract AGraphicalPrimitiveFactory GetGraphicalPrimitiveFactory();

        public virtual void Awake()
        {
            axes = new Dictionary<AxisDirection, AAxis>();
        }

        public virtual void Init(DataSet data)
        {
            this.data = data;
        }

        public void SetAxisLabels(AxisDirection axisDirection, string axisVariable)
        {
            axes[axisDirection].labelVariableText = axisVariable;
            axes[axisDirection].UpdateAxis();
        }

        public void AddBarChartAxis(string attributeName, AxisDirection dir, DataSet data, Transform parent)
        {
            var axis = GetGraphicalPrimitiveFactory().CreateAutoTickedAxis(attributeName, AxisDirection.X, data);
            axis.transform.parent = parent;
            axes.Add(dir, axis.GetComponent<AAxis>());
        }

        public void AddAxis(string attributeName, LoM lom, AxisDirection dir, DataSet data, Transform parent)
        {
            var factory = GetGraphicalPrimitiveFactory();

            GameObject axis;
            switch(lom)
            {
                case LoM.NOMINAL:
                    axis = factory.CreateFixedLengthAutoTickedAxis(attributeName, 1f, dir, data);
                    break;
                case LoM.ORDINAL:
                    axis = factory.CreateFixedLengthAutoTickedAxis(attributeName, 1f, dir, data);
                    break;
                case LoM.INTERVAL:
                    axis = factory.CreateAutoTickedAxis(attributeName, dir, data);
                    break;
                default: // RATIO
                    axis = factory.CreateAutoTickedAxis(attributeName, dir, data);
                    break;
            }
            axis.transform.parent = parent;
            axes.Add(dir, axis.GetComponent<AAxis>());
        }

        public AAxis GetAxis(AxisDirection dir)
        {
            return axes[dir].GetComponent<AAxis>();
        }

        public void AddAggregatedAxis(string attributeName, LoM lom, AxisDirection dir, DataSet data, Transform parent, out float max, out float length)
        {
            var factory = GetGraphicalPrimitiveFactory();

            switch(lom)
            {
                case LoM.NOMINAL:
                    var mea = data.nominalAttribStats[attributeName];
                    length = (mea.numberOfUniqueValues + 1) * .15f;
                    max = mea.distMax;
                    break;
                default:
                    var mea2 = data.ordinalAttribStats[attributeName];
                    length = (mea2.numberOfUniqueValues + 1) * .15f;
                    max = mea2.distMax;
                    break;
            }

            var yAxis = factory.CreateAutoTickedAxis("Amount", max);
            yAxis.transform.parent = parent;
            
            axes.Add(AxisDirection.Y, yAxis.GetComponent<AAxis>());
        }
    }
}
