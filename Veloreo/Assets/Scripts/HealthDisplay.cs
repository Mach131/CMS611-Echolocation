using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains actions that control the display of the health bar UI
/// </summary>
public class HealthDisplay : MonoBehaviour
{
    //refers to health cubes in UI
    public string[] healthNames = { "Health (0)", "Health (1)", "Health (2)", "Health (3)", "Health (4)" };
    public GameObject[] unitList;
    public int livesLeft = 5;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);
        unitList = GameObject.FindGameObjectsWithTag("health0");

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Takes one health/life away from health bar display when called--called by PlayerData in doDamage function
    /// </summary>
    public void changeHealthDisplay()
    {
        livesLeft--;
        foreach (GameObject i in unitList)
        { 
            if (i.name == healthNames[livesLeft])
            {
                i.SetActive(false);
            }
        }
    }
}
