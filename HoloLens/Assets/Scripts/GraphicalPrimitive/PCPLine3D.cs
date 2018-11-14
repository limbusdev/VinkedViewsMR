using UnityEngine;

namespace GraphicalPrimitive
{
    public class PCPLine3D : APCPLine
    {
        public override LineRenderer GetNewProperLineRenderer()
        {
            return Services.PrimFactory3D().GetNew3DLineRenderer();
        }
    }
}