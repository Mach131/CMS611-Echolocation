using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// More of a public interface for the player object; holds state about their health
/// and win/loss conditions.
/// </summary>
public class PlayerData : MonoBehaviour
{
    // currentHealth should always be non-negative
    private int currentHealth;

    private const int maximumHealth = 4;

    //// Public methods

    /// <summary>
    /// Do damage to the player and update their health accordingly. 
    /// </summary>
    /// <param name="damage">The amount to lower the player's health by</param>
    public void doDamage(int damage)
    {
        currentHealth = Mathf.Max(currentHealth - damage, 0);
    }

    //// Observers

    /// <summary>
    /// Get the current health of the player.
    /// </summary>
    /// <returns>Non-negative integer representing player's health</returns>
    public int getHealth()
    {
        return currentHealth;
    }

    //// Private methods

    // Start is called before the first frame update
    void Start()
    {
        initialize();
    }

    /// <summary>
    /// Initializes state-related variables, as if the player was just beginning a new level.
    /// </summary>
    private void initialize()
    {
        currentHealth = maximumHealth;
    }
}
