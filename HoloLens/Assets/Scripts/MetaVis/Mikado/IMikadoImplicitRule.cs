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

namespace mikado
{
    /// <summary>
    /// Interface to implement implicit MIKADO rules.
    /// Mikado is a system to build rules, which define
    /// what constellation of axes implies which MetaVis.
    /// </summary>
    public interface IMikadoImplicitRule
    {
        /// <summary>
        /// Does the provided constellation of axes fulfill
        /// the rules requirements?
        /// </summary>
        /// <param name="constellation">set of axes</param>
        /// <returns>whether this rule can be applied</returns>
        bool RuleApplies(AAxis[] constellation);
    }
}