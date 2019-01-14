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

using GraphicalPrimitive;
using UnityEngine;

public class Tick : MonoBehaviour
{

    public TextMesh label;
    public LineRenderer lr;

    public void SetDirection(AxisDirection dir)
    {
        switch(dir)
        {
            case AxisDirection.X:
                label.alignment = TextAlignment.Left;
                label.anchor = TextAnchor.MiddleLeft;
                label.transform.localRotation = Quaternion.Euler(0, 0, -90);
                label.transform.localPosition = Vector3.down * .05f;
                break;
            case AxisDirection.Y:
                label.alignment = TextAlignment.Right;
                label.anchor = TextAnchor.MiddleRight;
                label.transform.localRotation = Quaternion.Euler(0, 0, 0);
                label.transform.localPosition = Vector3.left * .05f;
                break;
            default:
                label.alignment = TextAlignment.Right;
                label.anchor = TextAnchor.MiddleRight;
                label.transform.localRotation = Quaternion.Euler(0, 0, 0);
                label.transform.localPosition = Vector3.left * .05f;
                break;
        }
    }
}
