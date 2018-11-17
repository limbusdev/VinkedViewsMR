using System.Collections.Generic;
using GraphicalPrimitive;
using Model;
using UnityEngine;

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