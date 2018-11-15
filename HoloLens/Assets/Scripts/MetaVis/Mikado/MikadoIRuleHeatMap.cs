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
