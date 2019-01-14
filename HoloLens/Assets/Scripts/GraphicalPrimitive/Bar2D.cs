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
    public class Bar2D : ABar
    {
        public GameObject barFront;
        public GameObject barBack;

        
        protected override void ApplyColor(Color color)
        {
            Renderer rend = barBack.GetComponent<Renderer>();
            rend.material.color = color;

            rend = barFront.GetComponent<Renderer>();
            rend.material.color = color;
        }
       
        override public void SetSize(float width, float height, float depth=1)
        {
            base.SetSize(width, height, 1);
            bar.transform.localScale = new Vector3(width, height, 1);
            var textMesh = label.GetComponent<TextMesh>();
            textMesh.anchor = (height < 0) ? TextAnchor.UpperCenter : TextAnchor.LowerCenter;
            label.transform.localPosition = new Vector3(0, height, 0);
        }
    }
}
