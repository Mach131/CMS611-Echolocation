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

    private bool canMove;
    private float moveEnableTimer;

    private float timeAdjustedSpeed
    {
        get
        {
            return speed * Time.deltaTime;
        }
    }

    /// <summary>
    /// Prevent the player from moving until enableMovement is called.
    /// </summary>
    public void disableMovement()
    {
        disableMovement(Mathf.Infinity);
    }

    /// <summary>
    /// Prevent the player from moving for a certain amount of time.
    /// </summary>
    /// <param name="time">How many seconds to wait before automatically re-enabling movement</param>
    public void disableMovement(float time)
    {
        canMove = false;
        moveEnableTimer = time;
    }

    /// <summary>
    /// Allow the player to move again after having disabled their movement.
    /// </summary>
    public void enableMovement()
    {
        canMove = true;
    }


    //// Private methods

    // Called before first update
    private void Start()
    {
        canMove = true;
    }

    // Called every frame
    private void Update()
    {
        if (canMove)
        {
            Vector3 displacement = getInputDirection() * timeAdjustedSpeed;
            transform.position += displacement;
        } else
        {
            moveEnableTimer -= Time.deltaTime;
            if (moveEnableTimer <= 0)
            {
                canMove = true;
            }
        }
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
