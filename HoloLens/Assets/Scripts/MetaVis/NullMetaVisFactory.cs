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
using GraphicalPrimitive;
using Model;

namespace MetaVisualization
{
    /// <summary>
    /// Dummy Implementation of AMetaVisFactory, for the client (pETV).
    /// </summary>
    public class NullMetaVisFactory : AMetaVisFactory
    {
        public override AETV CreateFlexiblePCP(DataSet data, string[] attIDs, AAxis axisA, AAxis axisB)
        {
            return new ETV3DFlexiblePCP();
        }

        public override AETV CreateMetaFlexibleLinedAxes(DataSet data, string[] attIDs, AAxis axisA, AAxis axisB)
        {
            throw new System.NotImplementedException();
        }

        public override AETV CreateMetaHeatmap3D(DataSet data, string[] attIDs, bool manualLength=false, float lengthA=1f, float lengthB=1f)
        {
            throw new System.NotImplementedException();
        }

        public override AETV CreateMetaScatterplot2D(DataSet data, string[] attIDs)
        {
            throw new System.NotImplementedException();
        }
    }
}
