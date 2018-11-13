using UnityEngine;

namespace GraphicalPrimitive
{
    public class AScatterDot : AGraphicalPrimitive
    {
        public MeshRenderer[] renderers;
        public GameObject dot;
        private Vector3 unhighlightedScale;

        private void Awake()
        {
            unhighlightedScale = dot.transform.localScale;
        }

        protected override void ApplyColor(Color color)
        {
            foreach(var rend in renderers)
                rend.material.color = color;
        }

        

        public override void Highlight()
        {
            base.Highlight();
            dot.transform.localScale = unhighlightedScale * 3f;
        }

        public override void Unhighlight()
        {
            base.Unhighlight();
            dot.transform.localScale = unhighlightedScale;
        }

        public override void Unbrush()
        {
            base.Unbrush();
            dot.transform.localScale = unhighlightedScale;
        }
    }
}