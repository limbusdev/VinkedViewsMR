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
using System.Collections.Generic;
using UnityEngine;

namespace VisBridges
{
    /// <summary>
    /// Manages visual connections between the representations of one or multiple
    /// information objects. Such representations are graphical primitives in
    /// visualizations.
    /// </summary>
    public abstract class AVisBridgeSystem : MonoBehaviour, IPrimitiveObserver
    {
        private IList<AGraphicalPrimitive> subscriptions = new List<AGraphicalPrimitive>();

        /// <summary>
        /// Toggles (On/Off) a VisBridge of the given information object. Including all
        /// vinks to representative graphical primitives in all visible visualizations.
        /// </summary>
        /// <param name="obj"></param>
        public abstract void ToggleVisBridgeFor(InfoObject obj);
        public abstract void ToggleVisBridgeFor(AGraphicalPrimitive prim);
        

        // .................................................................... IObserver

        /// <summary>
        /// Generates lookup tables for easy representational relation access.
        /// </summary>
        /// <param name="o"></param>
        /// <param name="p"></param>
        public abstract void RegisterGraphicalPrimitive(InfoObject o, AGraphicalPrimitive p);

        public abstract IList<InfoObject> GetInfoObjectsRepresentedBy(AGraphicalPrimitive p);

        /// <summary>
        /// (HELPER METHOD)
        /// Finds all relevant representative graphical primitives of the provided
        /// information objects.
        /// </summary>
        /// <param name="o">information object whose representatives are looked for</param>
        /// <returns>all representative graphical primitives</returns>
        public abstract IList<AGraphicalPrimitive> GetRepresentativePrimitivesOf(InfoObject o);
        
        public virtual void OnDispose(AGraphicalPrimitive observable)
        {
            subscriptions.Remove(observable);
        }

        public abstract void OnChange(AGraphicalPrimitive observable);

        public void Observe(AGraphicalPrimitive observable)
        {
            subscriptions.Add(observable);
        }

        public void Ignore(AGraphicalPrimitive observable)
        {
            subscriptions.Remove(observable);
        }
    }
}