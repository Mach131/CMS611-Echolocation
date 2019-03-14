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

    private HealthDisplay healthDisplay;
    public bool IsVulnerable { get; private set; }


    /// <summary>
    /// Do damage to the player and update their health accordingly. If the player's
    /// health drops to zero, the functions subscribed to onDeath are called.
    /// </summary>
    /// <param name="damage">The amount to lower the player's health by</param>
    /// <param name="invincibleTime">The time for which the player cannot be hurt again</param>
    public void doDamage(int damage, float invincibleTime)
    {
        if (currentHealth > 0 && IsVulnerable)
        {
            currentHealth = Mathf.Max(currentHealth - damage, 0);

            //grant invincibility
            StartCoroutine(giveInvincibility(invincibleTime));

            //change health display on canvas
            if (healthDisplay != null)
            {
                healthDisplay.changeHealthDisplay();
            }

            if (currentHealth == 0)
            {
                onDeath?.Invoke(); //apparently this lets it only happen if it's not null
                SceneManager.LoadScene(failSceneName);
            }
        }
    }

    /// <summary>
    /// Overload with default value of no invincibility time
    /// </summary>
    /// <param name="damage">The amount to lower the player's health by</param>
    public void doDamage(int damage)
    {
        doDamage(damage, 0);
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
        healthDisplay = FindObjectOfType<HealthDisplay>();
        IsVulnerable = true;
    }

    /// <summary>
    /// Make the player invincible for a set period of time.
    /// </summary>
    /// <param name="seconds">The time in seconds for which the player cannot take damage</param>
    /// <returns>I don't know how IEnumorators work tbh</returns>
    private IEnumerator giveInvincibility(float seconds)
    {
        IsVulnerable = false;
        yield return new WaitForSeconds(seconds);
        IsVulnerable = true;
    }
}
