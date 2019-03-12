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
    private float revealTime;
    [SerializeField]
    private float hideTime = .25f;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.enabled = false;
        timeWaiting = 1f;
        revealTime = 0f;
    }

    public void flash(float waitTime)
    {
        timeWaiting = 0;
        revealTime = waitTime;
    }

    private void updateVisibility()
    {
        if (timeWaiting < revealTime + hideTime)
        {
            timeWaiting += Time.deltaTime;
        }
        if (timeWaiting >= revealTime + hideTime)
        {
            sprite.enabled = false;
        }
        else if (timeWaiting >= revealTime)
        { 
            sprite.enabled = true;
        }
        
    }

    private void Update()
    {
        updateVisibility();
    }
}
