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