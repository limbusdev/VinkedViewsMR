using UnityEngine;

namespace GraphicalPrimitive
{
    public class PCPLine2D : APCPLine
    {
        public override LineRenderer GetNewProperLineRenderer()
        {
            return Services.PrimFactory2D().GetNew2DLineRenderer();
        }
    }
}