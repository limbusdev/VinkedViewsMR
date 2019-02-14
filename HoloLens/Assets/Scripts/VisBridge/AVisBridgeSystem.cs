/*
Copyright 2019 Georg Eckert (MIT License)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to
deal in the Software without restriction, including without limitation the
rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
sell copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
IN THE SOFTWARE.
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