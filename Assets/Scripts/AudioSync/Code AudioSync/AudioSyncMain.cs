using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSyncMain : MonoBehaviour  //Permet de faire la laisons entre les différents gameObject_effect avec le AudioSync. 
{
    [SerializeField] private AudioSync audioSync;
    //[SerializeField] private InstantiateObjectsInCircle circleInstantiator;
    [SerializeField] private InstantiateObjectsInLine lineInstantiator;
    // Start is called before the first frame update
    void Start()
    {
        //On crée et initialise les différents GameObject_Effect
        //circleInstantiator.SetScales(audioSync.GetSpectrum());
        //circleInstantiator.CreateCircle();
        lineInstantiator.SetScales(audioSync.getReducedSpectrum());
        lineInstantiator.CreateLine();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //on les affiche et on les actualises en suite
        //circleInstantiator.Visualizer();
        //circleInstantiator.SetScales(audioSync.GetSpectrum());

        lineInstantiator.SetScales(audioSync.getReducedSpectrum());
        lineInstantiator.Visualizer();
    }
}
