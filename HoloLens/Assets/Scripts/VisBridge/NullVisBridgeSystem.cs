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

using System.Collections.Generic;
using GraphicalPrimitive;
using Model;

namespace VisBridges
{
    /// <summary>
    /// Dummy implementation of AVisBridgeSystem. Try to provide the ServiceLocator
    /// with a concrete implementation.
    /// </summary>
    public class NullVisBridgeSystem : AVisBridgeSystem
    {
        public override void ToggleVisBridgeFor(InfoObject obj)
        {
            // Nothing
        }
        
        public override void ToggleVisBridgeFor(AGraphicalPrimitive prim)
        {
            // Do Nothing
        }

        public override IList<InfoObject> GetInfoObjectsRepresentedBy(AGraphicalPrimitive p)
        {
            return new List<InfoObject>();
        }

        public override IList<AGraphicalPrimitive> GetRepresentativePrimitivesOf(InfoObject o)
        {
            return new List<AGraphicalPrimitive>();
        }

        public override void OnDispose(AGraphicalPrimitive observable)
        {
            // Do Nothing
        }

        public override void OnChange(AGraphicalPrimitive observable)
        {
            // Do Nothing
        }

        public override void RegisterGraphicalPrimitive(InfoObject o, AGraphicalPrimitive p)
        {
            // Do Nothing
        }
    }
}