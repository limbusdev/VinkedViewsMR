namespace ETV
{
    public class PCP3DLineGenerator : APCPLineGenerator
    {
        public override AGraphicalPrimitiveFactory GetProperFactory()
        {
            return ServiceLocator.PrimitivePlant3D();
        }
    }
}
