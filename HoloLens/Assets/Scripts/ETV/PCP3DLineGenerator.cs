namespace ETV
{
    public class PCP3DLineGenerator : APCPLineGenerator
    {
        public override AGraphicalPrimitiveFactory GetProperFactory()
        {
            return Services.PrimFactory3D();
        }
    }
}
