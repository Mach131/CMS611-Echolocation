using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Controls text for the waves remaining display
/// </summary>
public class WaveText : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    string startingWaveAmount;

    // Start is called before the first frame update
    void Start()
    {
        startingWaveAmount = (FloorManager.maximumWaves).ToString();
        textMesh = GetComponent<TextMeshProUGUI>();
        textMesh.text = startingWaveAmount + " waves remaining";
    }

    /// <summary>
    /// Changes the display of the waves remaining text to update to the
    /// current amount of remaining waves
    /// </summary>
    /// /// <param name="wavesRemaining">The amount of waves left</param>
    public void changeTextDisplay(int wavesRemaining)
    {
        textMesh.text = wavesRemaining.ToString() + " waves remaining";
    }

    void Update()
    {

    }
}