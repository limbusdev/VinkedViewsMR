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
using Model;
using UnityEngine;

namespace mikado
{
    public class MikadoIRuleHeatMap : IMikadoImplicitRule
    {
        public static IMikadoImplicitRule I = new MikadoIRuleHeatMap();

        public bool RuleApplies(AAxis[] constellation)
        {
            if(constellation.Length != 2)
                return false;
            else
            {
                var axes = new AxisPair(constellation[0], constellation[1]);
                var angle = Vector3.Angle(axes.A.GetAxisDirectionGlobal(), axes.B.GetAxisDirectionGlobal());
                var orthogonalCase = (Mathf.Abs(angle - 90) < 5);

                var lomA = axes.A.stats.type;
                var lomB = axes.B.stats.type;

                var bothCategorical = (lomA == LoM.NOMINAL || lomA == LoM.ORDINAL) &&
                       (lomB == LoM.NOMINAL || lomB == LoM.ORDINAL);

                return (orthogonalCase && bothCategorical);
            }
        }
    }
}
