using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionThingMover : MonoBehaviour
{
    public Camera cam;

    public float xPct;
    public float yPct;

    // Start is called before the first frame update
    void Update()
    {
        transform.position = cam.ScreenToWorldPoint(new Vector3(xPct*Screen.width, yPct*Screen.height, .5f));
    }
}
