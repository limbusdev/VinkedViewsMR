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

using ETV;
public interface IETVComponent
{
    /// <summary>
    /// Used to assign a component to the etv it belongs to.
    /// </summary>
    /// <param name="etv"></param>
    void Assign(AETV etv);

    /// <summary>
    /// Returns the etv this component belongs to
    /// </summary>
    /// <returns></returns>
    AETV Base();
}
