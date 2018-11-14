using GraphicalPrimitive;
using Model;
using System.Collections.Generic;
using UnityEngine;

namespace VisBridge
{
    /// <summary>
    /// Manages visual connections between the representations of one or multiple
    /// information objects. Such representations are graphical primitives in
    /// visualizations.
    /// </summary>
    public abstract class AVisBridgeSystem : MonoBehaviour, IGPObserver<AGraphicalPrimitive>
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

        public abstract void Notify(AGraphicalPrimitive observable);

        public void Observe(AGraphicalPrimitive observable)
        {
            subscriptions.Add(observable);
        }
    }
}