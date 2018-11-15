using GraphicalPrimitive;

namespace mikado
{
    public class MikadoIRuleImmersiveAxes : IMikadoImplicitRule
    {
        public static IMikadoImplicitRule I = new MikadoIRuleImmersiveAxes();

        public bool RuleApplies(AAxis[] constellation)
        {
            return (constellation.Length > 1);
        }
    }
}
