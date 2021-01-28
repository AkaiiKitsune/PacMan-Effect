using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateObjectsInLine : MonoBehaviour
{
    [SerializeField] private float baseScale = 10f;
    [SerializeField] private float[] scaleValues;
    [SerializeField] private GameObject objectToInstantiate;
    [SerializeField] private List<GameObject> lineOfObjects;
    [SerializeField] private float maxScale;

    [SerializeField] private float distance = 100f;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    //Permet de modifier les élément un a un en les actualisants 
    public void Visualizer()
    {
        for (int i = 0; i < lineOfObjects.Count; ++i)
        {
            lineOfObjects[i].transform.localScale = new Vector3(baseScale, scaleValues[i] * maxScale, baseScale);
        }
    }

    public void CreateLine()
    {
        for (int i = 0; i < scaleValues.Length; ++i)
        {
            GameObject currentInstance = (GameObject)Instantiate(objectToInstantiate);
            currentInstance.transform.position = this.transform.position;
            currentInstance.transform.parent = this.transform;
            currentInstance.name = "CreatedObject" + i;

            currentInstance.transform.localPosition = Vector3.forward * (distance * i);

            // Garder le centre de la ligne crée à l'original
            if (i%2 == 0)
            {
                this.transform.position = Vector3.back * (distance * i/2);
            }
          
            lineOfObjects.Add(currentInstance);
        }
    }

    // Permet de récupérer les nouvelles valeurs de spectre de AudioSync par AudioSyncMain
    public void SetScales(float[] scales)
    {
        scaleValues = scales;
    }

}
