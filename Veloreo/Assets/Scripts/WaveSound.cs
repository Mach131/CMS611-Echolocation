using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides an interface to work with the sound associated with waves.
/// </summary>
public class WaveSound : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve volumeEnvelope = AnimationCurve.Linear(0, 1, 1, 0); 

    private AudioSource audioSource;

    private const float sineBaseFrequency = 250;

    //// Public Methods
    
    /// <summary>
    /// Plays a sine wave, using the current envelopes of this object.
    /// </summary>
    /// <param name="frequency">The frequency of the sine wave</param>
    /// <param name="durationSeconds">The duration of the volume envelope</param>
    public void playSineWave(float frequency, float durationSeconds)
    {
        audioSource.pitch = frequency / sineBaseFrequency;
        StartCoroutine(controlSineWave(durationSeconds));
    }

    //// Private Methods

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Controls a sine wave being played over a given period of time, based on this object's envelopes.
    /// </summary>
    /// <param name="durationSeconds">The duration of the volume envelope</param>
    /// <returns>what is an IEnumerator</returns>
    private IEnumerator controlSineWave(float durationSeconds)
    {
        audioSource.volume = volumeEnvelope.Evaluate(0);
        audioSource.Play();

        float timer = 0;
        while (timer < durationSeconds)
        {
            audioSource.volume = volumeEnvelope.Evaluate(timer / durationSeconds);
            yield return null;
            timer += Time.deltaTime;
        }

        audioSource.Stop();
    }
}
