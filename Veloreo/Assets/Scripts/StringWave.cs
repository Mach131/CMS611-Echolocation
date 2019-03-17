using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringWave : MonoBehaviour
{
    [SerializeField]
    private float vibrationDuration = 100f;
    private LineRenderer playerString;
    private float segmentWidth;
    private float timeVibrating;

    // use these for making waves look different
    private float amplitudeScale;
    private float waveSpeed;
    private float waveDuration;

    // use these to scale things since it will look weird otherwise
    private float frequencyScale = .05f;

    // Start is called before the first frame update
    void Start()
    {
        playerString = GetComponent<LineRenderer>();
        segmentWidth = (playerString.GetPosition(1).x - playerString.GetPosition(0).x) / 100f;
        playerString.positionCount = 100;
        timeVibrating = vibrationDuration;
    }

    public void startVibration(float amp, float speed, float duration)
    {
        timeVibrating = 0f;
        amplitudeScale = amp;
        waveSpeed = speed;
        waveDuration = duration;
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
        float halfway = segmentWidth * playerString.positionCount / 2f;
        float fullLength = (playerString.positionCount - 1) * segmentWidth;
        for (int i = 0; i < playerString.positionCount; i++)
        {
            float x = segmentWidth * i;
            float amplitude =  (x < halfway) ? (x / halfway) : (Mathf.Abs(x - fullLength) / halfway);
            float timeScale = (vibrationDuration - timeVibrating) / vibrationDuration;
            float scaledAmplitude = amplitudeScale * amplitude * timeScale;
            float frequency = frequencyScale * (waveDuration / waveSpeed);
            float y = scaledAmplitude * Mathf.Sin(frequency*(Mathf.Abs(x - halfway) - Time.time));
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
