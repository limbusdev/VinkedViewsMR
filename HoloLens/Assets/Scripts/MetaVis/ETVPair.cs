using ETV;

public struct ETVPair
{
    public AETV A { get; private set; }
    public AETV B { get; private set; }

    public ETVPair(AETV a, AETV b)
    {
        A = a;
        B = b;
    }
}