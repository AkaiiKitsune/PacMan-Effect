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
        Color whiteColor = Color.white * 2;
        objectMaterial.SetColor("_BaseColor", Color.Lerp(originalEmissionColor, whiteColor, fade));
    }

    //Permet de changer la couleur en blanc quand on appele la fonction
    public void GoWhite()
    {
        
        Color whiteColor = Color.white * 2;
        objectMaterial.SetColor("_BaseColor", whiteColor);
        fade = 1;
    }
}
