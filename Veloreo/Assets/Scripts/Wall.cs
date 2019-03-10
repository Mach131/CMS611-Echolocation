using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains some information about a wall's state, as well as options for debugging
/// like visibility.
/// </summary>
public class Wall : MonoBehaviour
{
    public bool debugVisibility = false;
    public Color normalColor;
    public Color visibleColor;

    private SpriteRenderer rend;

    public delegate void VisibilityAction(bool visible);
    private static VisibilityAction setAllVisible;

    //// Static Methods

    /// <summary>
    /// Change the visibility of all active wall objects.
    /// </summary>
    /// <param name="visible">Whether or not walls should be visible on a black background</param>
    public static void setVisibility(bool visible)
    {
        setAllVisible?.Invoke(visible); //calls if it is not null
    }

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

    //// Private Methods

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        setLocalVisibility(debugVisibility);
    }

    /// <summary>
    /// Sets the visibility of an individual wall object.
    /// </summary>
    /// <param name="visible">Whether or not the wall should be visible on a black background</param>
    private void setLocalVisibility(bool visible)
    {
        rend.color = visible ? visibleColor : normalColor;
    }

    // Add visibility method to static delegate
    private void OnEnable()
    {
        Wall.setAllVisible += setLocalVisibility;
    }
    private void OnDisable()
    {
        Wall.setAllVisible -= setLocalVisibility;
    }
}
