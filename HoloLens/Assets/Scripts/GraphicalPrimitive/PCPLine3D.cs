using UnityEngine;

namespace GraphicalPrimitive
{
    public class PCPLine3D : APCPLine
    {
        public override LineRenderer GetNewProperLineRenderer()
        {
            return ServiceLocator.PrimitivePlant3D().GetNew3DLineRenderer();
        }
    }
}