using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSound : MonoBehaviour
{
    private AudioSource note;
    // Start is called before the first frame update
    void Start()
    {
        note = GetComponent<AudioSource>();        
    }
    
    public void collideSound()
    {
        note.Play(0);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
