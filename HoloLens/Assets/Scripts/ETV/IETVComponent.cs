using ETV;
public interface IETVComponent
{
    /// <summary>
    /// Used to assign a component to the etv it belongs to.
    /// </summary>
    /// <param name="etv"></param>
    void Assign(AETV etv);

    /// <summary>
    /// Returns the etv this component belongs to
    /// </summary>
    /// <returns></returns>
    AETV Base();
}
