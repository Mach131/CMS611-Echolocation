using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls the player's movements, including reading inputs as well as collisions and
/// toggling mobility.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 1;
    [SerializeField]
    private float wallAdjustmentFactor = 0.1f;
    [SerializeField]
    private float guardAdjusmentFactor = 1.05f;
    [SerializeField]
    private float collisionPauseTime = 0.5f;

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
        //hitting a wall
        Wall wall = collision.gameObject.GetComponent<Wall>();
        if (wall != null)
        {
            handleWallCollision(wall);
        }

        //hitting a goal
        Goal goal = collision.gameObject.GetComponent<Goal>();
        if (goal != null)
        {
            handleGoalCollision(goal);
        }

        Guard guard = collision.gameObject.GetComponent<Guard>();
        if (guard != null)
        {
            handleGuardCollision(guard);
        }
    }

    /// <summary>
    /// Handle collision with a wall; currently pushes the player back a bit and stops
    /// them from moving for a bit, stopping them from getting stuck in a wall.
    /// </summary>
    /// <param name="wall">The wall that the player came in contact with</param>
    private void handleWallCollision(Wall wall)
    {
        //TODO: change health, i-frames?

        //push the player back a bit so they're not just stuck in the wall
        disableMovement(collisionPauseTime);
        Vector3 pushback = getPushbackDirection(wall);
        transform.position += pushback * wallAdjustmentFactor;
    }

    /// <summary>
    /// Handle collision with a goal; currently just transitions to the scene specified by
    /// the goal object.
    /// </summary>
    /// <param name="goal">The goal that the player came in contact with</param>
    private void handleGoalCollision(Goal goal)
    {
        //TODO: might be good to have some sort of pause/results popup before transition

        SceneManager.LoadScene(goal.getNextScene());
    }

    /// <summary>
    /// Handle collision with a guard.
    /// </summary>
    /// <param name="guard">Guard that is collided with.</param>
    private void handleGuardCollision(Guard guard)
    {
        disableMovement(collisionPauseTime);
        Vector3 pushback = getGuardPushback(guard);
        transform.position = guard.transform.position - guardAdjusmentFactor * pushback;
    }

    /// <summary>
    /// Handle intserction with a guard.
    /// </summary>
    /// <param name="guard">Guard that is collided with.</param>
    /// <returns></returns>
    private Vector3 getGuardPushback(Guard guard)
    {
        // find the direction between the center of the two circles.
        float xDir = guard.transform.position.x - transform.position.x;
        float yDir = guard.transform.position.y - transform.position.y;
        Vector2 dir = new Vector2(xDir, yDir).normalized;

        // find distance to stop intersection
        CircleCollider2D playerCollider = GetComponent<CircleCollider2D>();
        CircleCollider2D guardCollider = guard.GetComponent<CircleCollider2D>();
        // this relies on the sprites being a circle 

        // TODO: generalize collison more
        float distance = (playerCollider.radius + guardCollider.radius) * transform.localScale.x;
        return distance * dir;
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
    
    /// <summary>
    /// Determines which direction is away from a wall. Prioritizes perpendicular directions
    /// if the wall's orientation is horizontal or vertical.
    /// </summary>
    /// <param name="wall">The wall to move away from</param>
    /// <returns>A normalized vector indicating a direction moving away from the wall</returns>
    private Vector2 getPushbackDirection(Wall wall)
    {
        Vector3 wallPosition = wall.transform.position;
        Vector3 playerPosition = transform.position;

        float xDelta = playerPosition.x - wallPosition.x;
        float yDelta = playerPosition.y - wallPosition.y;

        switch (wall.getOrientation())
        {
            case Wall.Orientation.horizontal:
                return new Vector2(0, 1) * Mathf.Sign(yDelta);

            case Wall.Orientation.vertical:
                return new Vector2(1, 0) * Mathf.Sign(xDelta);

            case Wall.Orientation.square:
                return new Vector2(xDelta, yDelta).normalized;

            default:
                throw new System.Exception("how did i get here");
        }
    }
}
