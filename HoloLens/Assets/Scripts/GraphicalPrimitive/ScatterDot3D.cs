using UnityEngine;

namespace GraphicalPrimitive
{
    public class ScatterDot3D : AScatterDot
    {
        private void Update()
        {
            dot.transform.LookAt(Camera.main.transform);
        }
    }
}


