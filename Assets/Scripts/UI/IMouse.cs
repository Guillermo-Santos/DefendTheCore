/// <summary>
/// <para>
///     Interface to use for mouse movement over gameobject colliders.
/// </para>
/// Used to beat the limitation of the new input system where this
/// options on the monobehavior do not work by default.
/// </summary>
public interface IMouse
{
    /// <summary>
    /// Method to use for mouse clicks.
    /// </summary>
    void OnMouseDown();
    /// <summary>
    /// Method to use when mouse is over the gamebject.
    /// </summary>
    void OnMouseEnter();
    /// <summary>
    /// Method to use when the mouse isn't over the gameobject anymore.
    /// </summary>
    void OnMouseExit();
}
