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
