using GraphicalPrimitive;
using Model;
using UnityEngine;

namespace ETV
{
    public class ETV3DSingleAxis : AETVSingleAxis
    {
        // .................................................................... Private Variables
        private GameObject axis;
        private string attributeName;


        // .................................................................... AETV3D Methods
        public override void Init(DataSet data, string attributeName, bool isMetaVis = false)
        {
            base.Init(data, isMetaVis);
            this.attributeName = attributeName;

            SetUpAxes();

            SetAxisLabels(AxisDirection.Y, attributeName);
        }
        

        public override void SetUpAxes()
        {
            AddAxis(attributeName, AxisDirection.Y);
        }

        public override void UpdateETV()
        {
            // Nothing yet
        }

        public override void DrawGraph()
        {
            // Nothing yet
        }

        public override AGraphicalPrimitiveFactory GetGraphicalPrimitiveFactory()
        {
            return ServiceLocator.instance.Factory3DPrimitives;
        }


        // .................................................................... MonoBehaviour Methods

    }
}