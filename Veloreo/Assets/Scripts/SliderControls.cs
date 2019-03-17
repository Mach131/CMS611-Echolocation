using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Sets slider position and controls
/// </summary>
public class SliderControls : MonoBehaviour
{
    private static Slider pitchSlider;
    FloorManager floor;
    // Start is called before the first frame update
    void Start()
    {
        floor = GameObject.Find("FloorManager").GetComponent<FloorManager>();

        pitchSlider = gameObject.GetComponent<Slider>();
        pitchSlider.transform.position = new Vector3(650.0f, 20.0f, 0.0f);
        pitchSlider.minValue = 1;
        pitchSlider.maxValue = 5;
        pitchSlider.value = 5;
        pitchSlider.wholeNumbers = true;
    }

    /// <summary>
    /// Getter method for slider value
    /// </summary>
    /// <returns> the value of the slider</returns>
    public static float getValue()
    {
        return pitchSlider.value;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
