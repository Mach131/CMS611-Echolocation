using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains some information about a wall's state.
/// </summary>
public class Wall : MonoBehaviour
{
    //// Observers

    public enum Orientation { horizontal, vertical, square };

    /// <summary>
    /// Gets the orientation of the wall based on its scaling. Mostly used for determining
    /// how the player acts when running into it.
    /// </summary>
    /// <returns>Horizontal if the wall is wider than it is tall, Vertical if the opposite
    /// is true, and square if its dimensions are equal.</returns>
    public Orientation getOrientation()
    {
        Vector3 scale = transform.rotation * transform.localScale;
        float xScale = Mathf.Abs(scale.x);
        float yScale = Mathf.Abs(scale.y);

        if (xScale == yScale)
        {
            return Orientation.square;
        } else
        {
            return xScale > yScale ? Orientation.horizontal : Orientation.vertical;
        }
    }

    /// <summary>
    /// Gets the thickness of the wall.
    /// </summary>
    /// <returns>The size of the wall's shorter side in world space</returns>
    public float getThickness()
    {
        if (getOrientation() == Orientation.horizontal)
        {
            return transform.lossyScale.y;
        } else
        {
            return transform.lossyScale.x;
        }
    }
}
