using UnityEngine;

namespace GraphicalPrimitive
{
    class Bar3D : ABar
    {
        public GameObject geometry;

        protected override void ApplyColor(Color newColor)
        {
            geometry.GetComponent<Renderer>().material.color = newColor;
        }

        public override void SetSize(float width, float height, float depth)
        {
            bar.transform.localScale = new Vector3(width, height, depth);
            var textMesh = label.GetComponent<TextMesh>();
            textMesh.anchor = (height < 0) ? TextAnchor.UpperCenter : TextAnchor.LowerCenter;
            label.transform.localPosition = new Vector3(0, height, 0);
        }
        
    }
}
