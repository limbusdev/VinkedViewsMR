namespace ETV
{
    public class PCP2DLineGenerator : APCPLineGenerator
    {
        public override AGraphicalPrimitiveFactory GetProperFactory()
        {
            return Services.PrimFactory2D();
        }
    }
}
