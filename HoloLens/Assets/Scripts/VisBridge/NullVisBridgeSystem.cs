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
            throw new System.NotImplementedException();
        }

        public override IList<InfoObject> GetInfoObjectsRepresentedBy(AGraphicalPrimitive p)
        {
            throw new System.NotImplementedException();
        }

        public override IList<AGraphicalPrimitive> GetRepresentativePrimitivesOf(InfoObject o)
        {
            throw new System.NotImplementedException();
        }

        public override void OnDispose(AGraphicalPrimitive observable)
        {
            throw new System.NotImplementedException();
        }

        public override void OnChange(AGraphicalPrimitive observable)
        {
            throw new System.NotImplementedException();
        }

        public override void RegisterGraphicalPrimitive(InfoObject o, AGraphicalPrimitive p)
        {
            throw new System.NotImplementedException();
        }
    }
}