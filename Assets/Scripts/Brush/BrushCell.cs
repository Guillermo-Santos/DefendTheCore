using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///Brush Cell stores the data to be painted in a grid cell.
/// </summary>
[Serializable]
public class BrushCell
{
    /// <summary>
    /// GameObject to be placed when painting.
    /// </summary>
    public GameObject gameObject { get { return m_GameObject; } set { m_GameObject = value; } }
    /// <summary>
    /// Position offset of the GameObject when painted.
    /// </summary>
    public Vector3 offset { get { return m_Offset; } set { m_Offset = value; } }
    /// <summary>
    /// Scale of the GameObject when painted.
    /// </summary>
    public Vector3 scale { get { return m_GameObject.transform.localScale; } set { m_GameObject.transform.localScale = value; } }
    /// <summary>
    /// Orientatio of the GameObject when painted.
    /// </summary>
    public Quaternion orientation { get { return m_Orientation; } set { m_Orientation = value; } }

    [SerializeField]
    private GameObject m_GameObject;
    [SerializeField]
    Vector3 m_Offset = Vector3.zero;
    [SerializeField]
    Vector3 m_Scale = Vector3.one;
    [SerializeField]
    Quaternion m_Orientation = Quaternion.identity;

    /// <summary>
    /// Hashes the contents of the brush cell.
    /// </summary>
    /// <returns>A hash code of the brush cell.</returns>
    public override int GetHashCode()
    {
        int hash;
        unchecked
        {
            hash = gameObject != null ? gameObject.GetInstanceID() : 0;
            hash = hash * 33 + offset.GetHashCode();
            hash = hash * 33 + scale.GetHashCode();
            hash = hash * 33 + orientation.GetHashCode();
        }
        return hash;
    }
}
