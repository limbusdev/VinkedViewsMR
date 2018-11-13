using UnityEngine;

namespace GraphicalPrimitive
{
    public abstract class ABar : AGraphicalPrimitive
    {
        public GameObject bar;

        public abstract void SetSize(float width, float height, float depth);

        public void SetLabelText(string newText)
        {
            label.GetComponent<TextMesh>().text = newText;
        }

        private Vector3 unhighlightedScale;

        public override void Highlight()
        {
            base.Highlight();
            unhighlightedScale = bar.transform.localScale;
            bar.transform.localScale = unhighlightedScale * 1.2f;
        }

        public override void Unhighlight()
        {
            base.Unhighlight();
            bar.transform.localScale = unhighlightedScale;
        }
    }
}
