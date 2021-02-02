using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterialProperties : MonoBehaviour
{
    //Cette fonction s'éxecute sur le Game Object
    Material objectMaterial;
    Color originalEmissionColor;

    [SerializeField] private float fadeSpeed = 0.05F;
    private float fade = 0;
    Color colorToDisplay = Color.white * 2;

    // équivalent d'un constructeur
    void Awake()
    {
        objectMaterial = GetComponent<MeshRenderer>().materials[0];         //Récupère le matériau zéro pour pouvoir modifier la couleur 
        originalEmissionColor = objectMaterial.GetColor("_BaseColor");  //Permet de sauvegarder la couleur d'origine
    }

    // Un Update... et il sert a rien ¯\_(ツ)_/¯
    void Update()
    {
        fade -= fadeSpeed;
        objectMaterial.SetColor("_BaseColor", Color.Lerp(originalEmissionColor, colorToDisplay, fade));
        objectMaterial.SetColor("_EmissionColor", Color.Lerp(originalEmissionColor, colorToDisplay, fade));
    }

    //Permet de changer la couleur en blanc quand on appele la fonction
    public void GoWhite()
    {

        colorToDisplay = Color.white * 2;
        fade = 1;
    }

    public void GoDark()
    {
        colorToDisplay = Color.black;
        fade = 1;
    }

    public void GhostGoWhite()
    {
        colorToDisplay = Color.white;
        fade = 1;
    }
}
