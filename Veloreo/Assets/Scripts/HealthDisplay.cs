using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains actions that control the display of the health bar UI
/// </summary>
public class HealthDisplay : MonoBehaviour
{   
    //refers to health cubes in UI
    public string[] healthTags = { "health0", "health1", "health2", "health3", "health4" };

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);
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
        if(gameObject.tag == healthTags[FindObjectOfType<PlayerData>().getHealth()])
        {
            gameObject.SetActive(false);
        }
    }
}
