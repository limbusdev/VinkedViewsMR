using GraphicalPrimitive;

namespace mikado
{
    /// <summary>
    /// Interface to implement implicit MIKADO rules.
    /// Mikado is a system to build rules, which define
    /// what constellation of axes implies which MetaVis.
    /// </summary>
    public interface IMikadoImplicitRule
    {
        /// <summary>
        /// Does the provided constellation of axes fulfill
        /// the rules requirements?
        /// </summary>
        /// <param name="constellation">set of axes</param>
        /// <returns>whether this rule can be applied</returns>
        bool RuleApplies(AAxis[] constellation);
    }
}