using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraAngler : MonoBehaviour
{
    public int i = 0;
    public List<Vector3> positions = new List<Vector3>();
    public List<Vector3> rotations = new List<Vector3>();



    // Start is called before the first frame update
    void Start()
    {
        transform.position = positions[i];
        transform.SetPositionAndRotation(positions[i], Quaternion.Euler(rotations[i]));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
