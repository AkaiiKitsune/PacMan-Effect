using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MusicEvent : UnityEvent<int> { }

//permet de récupérer le componenet AudioSource du gameobject
[RequireComponent(typeof(AudioSource))]
public class AudioSync : MonoBehaviour //Cette classe permet d'utiliser des fonctions pour d'autres script
{
    [SerializeField] private MusicEvent eventsOn;
    [SerializeField] private MusicEvent eventsOff;

    [SerializeField] private float triggerThreshold = 0.5f;


    private AudioSource mainAudioSource;        //Permet d'avoir le component AudioSource afin de récupérer le spectre audio
    [SerializeField] private int nbIndex = 512;
    [SerializeField] private int sampleRate;
    private float maxFreq;
    private float freqPerIndex;
    [SerializeField] private float[] spectrum;  //Permet de stocker le spectre pour l'utiliser dans d'autre opération (~~= mainAudioSource)

    [SerializeField] private List<int> freqLimits = new List<int>();
    [SerializeField] private float[] reducedSpectrum;

    //Buffer smoth
    [SerializeField] private bool mustUseBuffer = false;    //ermet de spécifier si on utilise le buffer

    [SerializeField] private float[] reducedSpectrumBuffer; //stocke le reducedSpectrum mais avec le buffer Smoth
    [SerializeField] private float[] bufferSmooth;          //Sert à définir le smoth des lignes

    //Normalise
    [SerializeField] private float[] reducedSpectrumMaximum;
    [SerializeField] private float[] normalizedReducedSpectrum;
    [SerializeField] private float[] normalizedReducedSpectrumBuffer;


    // équivalent d'un constructeur
    void Awake()
    {
        mainAudioSource = GetComponent<AudioSource>();
        sampleRate = AudioSettings.outputSampleRate;

        maxFreq = sampleRate / 2f;
        freqPerIndex = maxFreq / nbIndex;
        spectrum = new float[nbIndex];
        reducedSpectrum = new float[freqLimits.Count];

        reducedSpectrumBuffer = new float[freqLimits.Count];
        bufferSmooth = new float[freqLimits.Count];

        reducedSpectrumMaximum = new float[freqLimits.Count];
        normalizedReducedSpectrum = new float[freqLimits.Count];
        normalizedReducedSpectrumBuffer = new float[freqLimits.Count];
    }

    // Permet de faire update en mieux ?
    void FixedUpdate()
    {
        UpdateSpectrumData();
        UpdateReducedSpectrum();
        ComputeBuffer();
        UpdateNormalizedSpectrum();
    }

    //Permet de récupérer les nouvelle information du spectre audio sur l'AudioSource
    void UpdateSpectrumData()
    {
        mainAudioSource.GetSpectrumData(spectrum, 0, FFTWindow.Blackman);
    }

    public float[] GetSpectrum()
    {
        return spectrum;
    }

    void UpdateNormalizedSpectrum()
    {
        for (int i = 0; i < reducedSpectrum.Length; i++)
        {
            if ( reducedSpectrum[i] > reducedSpectrumMaximum[i])
            {
                reducedSpectrumMaximum[i] = reducedSpectrum[i];
            }

            normalizedReducedSpectrum[i] = reducedSpectrum[i] / reducedSpectrumMaximum[i];
            normalizedReducedSpectrumBuffer[i] = reducedSpectrumBuffer[i] / reducedSpectrumMaximum[i];

            //Il fait soit un event Off ou On 
            if(normalizedReducedSpectrumBuffer[i] < triggerThreshold)
            {
                eventsOff.Invoke(i);
            }
            else
            {
                eventsOn.Invoke(i);
            }
        }
    }

    void UpdateReducedSpectrum()
    {
        int j = 0;
        reducedSpectrum[0] = 0f;

        for (int i = 0; i < spectrum.Length; i++)
        {
            float currentFreq = (i + 1) * freqPerIndex;

            if (currentFreq > freqLimits[j])
            {
                j++;
                reducedSpectrum[j] = 0f;
            }

            reducedSpectrum[j] += spectrum[i];

        }
    }

    void ComputeBuffer()
    {
        for (int i = 0; i < reducedSpectrum.Length; ++i)
        {
            if (reducedSpectrum[i] > reducedSpectrumBuffer[i])
            {
                reducedSpectrumBuffer[i] = reducedSpectrum[i];
                bufferSmooth[i] = 0.005f;

            }
            else
            {
                /* reducedFftSamplesBuffer[i] -= reducedFftSamplesBufferDecrease[i];
                 reducedFftSamplesBufferDecrease[i] *= 1.2f;*/
                bufferSmooth[i] = (reducedSpectrumBuffer[i] - reducedSpectrum[i]) / 8f;
                reducedSpectrumBuffer[i] -= bufferSmooth[i];

            }
        }
    }

    public float[] getReducedSpectrum()
    {
        if (mustUseBuffer)
        {
            return reducedSpectrumBuffer;
        }
        else
        {
            return reducedSpectrum;
        }

    }
}
