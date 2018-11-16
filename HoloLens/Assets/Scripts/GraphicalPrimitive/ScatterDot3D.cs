using UnityEngine;

namespace GraphicalPrimitive
{
    public class ScatterDot3D : AScatterDot
    {
        public override void Update()
        {
            base.Update();
            dot.transform.LookAt(Camera.main.transform);
        }
    }
}


