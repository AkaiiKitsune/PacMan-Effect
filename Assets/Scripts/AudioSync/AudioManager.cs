using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioClip death, chomp, wakka, fuite;
    static AudioSource AudioSrc;
    void Start()
    {
        death = Resources.Load<AudioClip>("Sounds/PacmanDead");
        chomp = Resources.Load<AudioClip>("Sounds/PacChomp");
        wakka = Resources.Load<AudioClip>("Sounds/Wakka");
        fuite = Resources.Load<AudioClip>("Sounds/GhostFuite");



        AudioSrc = GetComponent<AudioSource>();
    }
    
    public static void PlaySound(string choix)
    {
        
        switch (choix)
        {
            case "death":        
                AudioSrc.PlayOneShot(death);
                //Debug.Log(AudioSrc.clip.length);
                break;
            case "chomp":        
                AudioSrc.PlayOneShot(chomp);
                //Debug.Log(AudioSrc.clip.length);
                break;
            case "wakka":        
                AudioSrc.PlayOneShot(wakka, 0.7f);
                //Debug.Log(AudioSrc.clip.length); 
                break;
            case "fuite":        
                AudioSrc.PlayOneShot(fuite);
                //Debug.Log(AudioSrc.clip.length); 
                break;
        }
    }
}
