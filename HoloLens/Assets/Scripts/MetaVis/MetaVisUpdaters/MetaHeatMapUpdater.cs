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

public class MetaHeatMapUpdater : AMetaVisUpdater
{
    protected override MetaVisType Type()
    {
        return MetaVisType.HEATMAP;
    }

    protected override void UpdateNormalVector()
    {
        normal = Vector3.Cross(
                    spanningAxes.A.GetAxisDirectionGlobal(),
                    spanningAxes.B.GetAxisDirectionGlobal());
    }

    protected override void UpdatePosition()
    {
        metaVisualization.transform.position =
                    (spanningAxes.A.GetAxisBaseGlobal()
                    + spanningAxes.B.GetAxisBaseGlobal()) / 2f;
    }

    protected override void UpdateRotation()
    {
        if(signedAngle > 0)
        {
            var rot = new Quaternion();
            rot.SetLookRotation(spanningAxes.A.GetAxisDirectionGlobal(), normal);
            metaVisualization.transform.rotation = rot;
        } else
        {
            var rot = new Quaternion();
            rot.SetLookRotation(spanningAxes.B.GetAxisDirectionGlobal(), normal);
            metaVisualization.transform.rotation = rot;
        }
    }
}
