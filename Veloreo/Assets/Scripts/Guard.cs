using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains some information about a goal's state.
/// </summary>
public class Guard : MonoBehaviour
{
    private SpriteRenderer sprite;
    private float timeWaiting;
    [SerializeField]
    private float hideTime = .3f;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
        // janky way to start them off hidden
        timeWaiting = hideTime + 1f;
    }

    public void Flash()
    {
        timeWaiting = 0;
        sprite.enabled = true;
    }

    private void updateVisibility()
    {
        if (timeWaiting < hideTime)
        {
            timeWaiting += Time.deltaTime;
        }

        if (timeWaiting >= hideTime)
        {
            sprite.enabled = false;
        }
    }

    private void Update()
    {
        updateVisibility();
    }
}
