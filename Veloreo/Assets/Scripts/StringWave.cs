using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringWave : MonoBehaviour
{
    private LineRenderer playerString;
    private float segmentWidth;
    [SerializeField]
    private float vibrationDuration = 10f;
    private float timeVibrating;

    // Start is called before the first frame update
    void Start()
    {
        playerString = GetComponent<LineRenderer>();
        segmentWidth = (playerString.GetPosition(1).x - playerString.GetPosition(0).x) / 100f;
        playerString.positionCount = 100;
        timeVibrating = 0f;
    }

    public void startVibration()
    {
        timeVibrating = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timeVibrating < vibrationDuration)
        {
            vibrateString();
        } else
        {
            clearString();
        }
    }

    private void vibrateString()
    {
        for (int i = 0; i < playerString.positionCount; i++)
        {
            float x = segmentWidth * i;
            float y = Mathf.Sin(x + Time.time);
            playerString.SetPosition(i, new Vector3(x, y, 0f));
            timeVibrating += Time.fixedDeltaTime;
        }
    }

    private void clearString()
    {
        for (int i = 0; i < playerString.positionCount; i++)
        {
            float x = segmentWidth * i;
            playerString.SetPosition(i, new Vector3(x, 0, 0));
        }
    }
}
