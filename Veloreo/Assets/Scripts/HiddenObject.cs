using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains state for objects that are meant to be hidden by the darkness, and allows
/// their visibility to be toggled.
/// </summary>
public class HiddenObject : MonoBehaviour
{
    public bool debugVisibility = false;
    public Color normalColor;
    public Color visibleColor;

    private SpriteRenderer rend;

    public delegate void VisibilityAction(bool visible);
    private static VisibilityAction setAllVisible;

    //// Static Methods

    /// <summary>
    /// Change the visibility of all active objects.
    /// </summary>
    /// <param name="visible">Whether or not objects should be visible on a black background</param>
    public static void setVisibility(bool visible)
    {
        setAllVisible?.Invoke(visible); //calls if it is not null
    }

    //// Private Methods

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        setLocalVisibility(debugVisibility);
    }

    /// <summary>
    /// Sets the visibility of an individual object.
    /// </summary>
    /// <param name="visible">Whether or not the object should be visible on a black background</param>
    private void setLocalVisibility(bool visible)
    {
        rend.color = visible ? visibleColor : normalColor;
    }

    // Add visibility method to static delegate
    private void OnEnable()
    {
        HiddenObject.setAllVisible += setLocalVisibility;
    }
    private void OnDisable()
    {
        HiddenObject.setAllVisible -= setLocalVisibility;
    }
}
