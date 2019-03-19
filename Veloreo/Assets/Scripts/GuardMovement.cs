using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardMovement : MonoBehaviour
{
    [SerializeField]
    private List<Vector2> positions = new List<Vector2>()
    {
        new Vector2(0, 4),
        new Vector2(0, -2),
    };
    [SerializeField]
    private float speed = .1f;
    private int numPositions;
    private int currentGoal;

    void Start()
    {
        numPositions = positions.Count;
        currentGoal = 0;
    }

    void FixedUpdate()
    {
        moveTowards(positions[currentGoal]);
        updateCurrentGoal();
    }

    private void moveTowards(Vector2 position)
    {
        Vector2 currentPositon = transform.position;
        Vector2 nextStep = Vector2.MoveTowards(currentPositon, position, speed);
        transform.position = nextStep;
    }

    private void updateCurrentGoal()
    {
        if (transform.position.Equals(positions[currentGoal]))
        {
            currentGoal = (currentGoal + 1) % numPositions;
        }
    }
}
