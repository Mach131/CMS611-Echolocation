using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the player's movements, including reading inputs as well as collisions and
/// toggling mobility.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 1;

    private float timeAdjustedSpeed
    {
        get
        {
            return speed * Time.deltaTime;
        }
    }

    //// Private methods

    // Called every frame
    private void Update()
    {
        Vector3 displacement = getInputDirection() * timeAdjustedSpeed;
        transform.position += displacement;
    }

    // Called when entering a 2D collider
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Wall wall = collision.gameObject.GetComponent<Wall>();
        if (wall != null)
        {
            //hit a wall
        }
    }

    /// <summary>
    /// Reads the player's directional input and determines what angle to move at.
    /// </summary>
    /// <returns>A normalized vector indicating the angle input by the player</returns>
    private Vector2 getInputDirection()
    {
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        return new Vector2(xAxis, yAxis).normalized;
    }
}
