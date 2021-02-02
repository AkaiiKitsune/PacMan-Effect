using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable] 
public class TempoEvent : UnityEvent<int> { }; //Créer un system d'Event qui transmet des variable en int

public class AudioPosition : MonoBehaviour{    
    AudioSource musicSource = null;

    [SerializeField] private int _bpm;

    [SerializeField] private float _currentAbsoluteMusicPosition = 0f;
    [SerializeField] private float _currentRelativeMusicPosition = 0f;

    [SerializeField] TempoEvent eventBPM;

    float previousMusicTime = 0;

    float _nextBPMTimeCode = 0F;
    int _cptBPM = 0;

    void Start() { 
        musicSource = GetComponent<AudioSource>();
        Debug.Log("Audio clip length : " + musicSource.clip.length);
    }

    // Update is called once per frame
    void Update()    
    {
        if (musicSource != null)        
        {            
            _currentAbsoluteMusicPosition = musicSource.time;            
            _currentRelativeMusicPosition = _currentAbsoluteMusicPosition  / musicSource.clip.length;   
        }

        // _nextBPMTimeCode prend la valeur du prochain BPM si la musique se reboucle le _nextBPMTimeCode ne sera jamais atteint
        // pour remédier, si _nextBPMTimeCode dépasse la longeur de la mussique il sera réinitialiser
        if (musicSource.time < previousMusicTime)
        {
            Debug.Log("ça boucle");
            _nextBPMTimeCode = 0;
            _cptBPM = 0;
        }

        previousMusicTime = musicSource.time;

        if (_currentAbsoluteMusicPosition > _nextBPMTimeCode)
        {
            _nextBPMTimeCode += 60F / _bpm;

            _cptBPM++;

            eventBPM.Invoke(_cptBPM);
        }      


    }
}
