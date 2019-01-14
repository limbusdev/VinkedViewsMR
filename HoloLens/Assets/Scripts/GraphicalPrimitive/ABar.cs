/*
Vinked Views
Copyright(C) 2018  Georg Eckert

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

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
