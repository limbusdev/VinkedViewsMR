using GraphicalPrimitive;
using System.Collections.Generic;

/// <summary>
/// Pairs which contain the same axis in arbitrary order, are considered equal.
/// </summary>
public class AxisPair
{
    public AAxis A { get; private set; }
    public AAxis B { get; private set; }

    public AxisPair(AAxis a, AAxis b)
    {
        A = a;
        B = b;
    }

    public override bool Equals(object obj)
    {
        if(obj is AxisPair)
        {
            var other = obj as AxisPair;
            return (
                other.A.Equals(A) && other.B.Equals(B)
                ||
                other.A.Equals(B) && other.B.Equals(A));
        } else
            return false;
    }

    public override int GetHashCode()
    {
        var hashCode = -624926263;
        hashCode = hashCode + EqualityComparer<AAxis>.Default.GetHashCode(A);
        hashCode = hashCode + EqualityComparer<AAxis>.Default.GetHashCode(B);
        return hashCode;
    }
}