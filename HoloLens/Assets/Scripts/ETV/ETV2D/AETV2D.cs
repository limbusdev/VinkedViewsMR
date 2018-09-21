namespace ETV
{
    public abstract class AETV2D : AETV
    {
        public float[] bounds { get; set; }

        public override void Awake()
        {
            base.Awake();
            bounds = new float[] { 1, 1 };
        }
    }
}