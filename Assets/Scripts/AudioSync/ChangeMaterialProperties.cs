using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterialProperties : MonoBehaviour
{
    Material objectMaterial;
    public bool mustGoWhite = false;
    Color originalEmissionColor;    

    // équivalent d'un constructeur
    void Awake()
    {
        objectMaterial = GetComponent<MeshRenderer>().materials[0];         //Récupère le matériau zéro pour pouvoir modifier la couleur 
        originalEmissionColor = objectMaterial.GetColor("_EmissionColor");  //Permet de sauvegarder la couleur d'origine
    }

    // Un Update... et il sert a rien ¯\_(ツ)_/¯
    void Update()
    {
       
    }

    //Permet de changer la couleur en blanc quand on appele la fonction
    public void GoWhite()
    {
        Color whiteColor = Color.white * 2;

        objectMaterial.SetColor("_EmissionColor", whiteColor);
    }

    //Permet de Reset la couleur quand on appele la fonction
    public void ResetColor()
    {
        objectMaterial.SetColor("_EmissionColor", originalEmissionColor);
    }
}
