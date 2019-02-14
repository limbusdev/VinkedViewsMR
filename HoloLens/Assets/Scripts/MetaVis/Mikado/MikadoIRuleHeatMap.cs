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
using UnityEngine;

namespace mikado
{
    public class MikadoIRuleHeatMap : IMikadoImplicitRule
    {
        public static IMikadoImplicitRule I = new MikadoIRuleHeatMap();

        public bool RuleApplies(AAxis[] constellation)
        {
            if(constellation.Length != 2)
                return false;
            else
            {
                var axes = new AxisPair(constellation[0], constellation[1]);
                var angle = Vector3.Angle(axes.A.GetAxisDirectionGlobal(), axes.B.GetAxisDirectionGlobal());
                var orthogonalCase = (Mathf.Abs(angle - 90) < 5);

                var lomA = axes.A.stats.type;
                var lomB = axes.B.stats.type;

                var bothCategorical = (lomA == LoM.NOMINAL || lomA == LoM.ORDINAL) &&
                       (lomB == LoM.NOMINAL || lomB == LoM.ORDINAL);

                return (orthogonalCase && bothCategorical);
            }
        }
    }
}
