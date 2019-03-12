using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Contains some information about a goal's state.
/// </summary>
public class Goal : MonoBehaviour
{
    [SerializeField]
    private string nextSceneName = "";

    /// <summary>
    /// Get the scene that this goal transfers the player to.
    /// </summary>
    /// <returns>The name of the scene following the level in which this goal is placed</returns>
    public string getNextScene()
    {
        return nextSceneName;
    }
}
