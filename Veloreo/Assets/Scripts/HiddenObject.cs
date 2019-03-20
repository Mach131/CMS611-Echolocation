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

    [SerializeField]
    private float hideTime = .4f;
    private float revealTime;
    private float timeWaiting;
     
    private Renderer rend;

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
        timeWaiting = hideTime + 1f;
        rend = GetComponent<Renderer>();
        revealTime = hideTime / 2.0f;
        setLocalVisibility(debugVisibility);
    }

    /// <summary>
    /// Sets the visibility of an individual object.
    /// </summary>
    /// <param name="visible">Whether or not the object should be visible on a black background</param>
    private void setLocalVisibility(bool visible)
    {
        rend.enabled = visible;
    }

    public void Flash()
    {
        timeWaiting = 0;
        rend.enabled = true;
        Color color = rend.material.color;
        color.a = 0;
        rend.material.color = color;

    }

    private void Update()
    {
        if (timeWaiting < hideTime)
        {
            timeWaiting += Time.deltaTime;
            Color color = rend.material.color;
            if (timeWaiting < revealTime)
            {
                color.a = timeWaiting / revealTime;
            } else
            {
                color.a = (timeWaiting - revealTime) / revealTime;
            }
            rend.material.color = color;
        }

        if (timeWaiting >= hideTime)
        {
            rend.enabled = false || debugVisibility;
        }
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
