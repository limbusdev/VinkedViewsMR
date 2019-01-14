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