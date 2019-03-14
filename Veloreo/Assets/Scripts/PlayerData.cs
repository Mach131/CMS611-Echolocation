using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// More of a public interface for the player object; holds state about their health
/// and win/loss conditions.
/// </summary>
public class PlayerData : MonoBehaviour
{
    /// <summary>
    /// onDeath is called when the player runs out of health; other classes can
    /// subscribe listeners to it to do things when that happens.
    /// </summary>
    public delegate void GameStateEvent();
    public event GameStateEvent onDeath;

    [SerializeField]
    private string failSceneName = "";

    // currentHealth should always be non-negative
    private int currentHealth;

    private const int maximumHealth = 5;

    //// Public methods

    /// <summary>
    /// Do damage to the player and update their health accordingly. If the player's
    /// health drops to zero, the functions subscribed to onDeath are called.
    /// </summary>
    /// <param name="damage">The amount to lower the player's health by</param>
    public void doDamage(int damage)
    {
        if (currentHealth > 0)
        {
            currentHealth = Mathf.Max(currentHealth - damage, 0);

            //change health display on canvas
            FindObjectOfType<HealthDisplay>().changeHealthDisplay();

            if (currentHealth == 0)
            {
                onDeath?.Invoke(); //apparently this lets it only happen if it's not null
                SceneManager.LoadScene(failSceneName);
            }
        }
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
