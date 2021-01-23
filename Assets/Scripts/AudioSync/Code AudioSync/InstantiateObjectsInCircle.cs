using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateObjectsInCircle : MonoBehaviour
{
    private float[] scaleValues;

    [SerializeField] private GameObject objectToInstanciate;    //Permet de stocker le prefab à Instantier
    [SerializeField] private float distance = 100f;             //Permet de définir le diamètre || le rayon
    [SerializeField] private float maxScale = 1000f;            //Permet de mettre une hauteur limite

    private List<GameObject> circleOfObjects = new List<GameObject>();  //Permet de stocker les prefab Instantier


    // Start is called before the first frame update
    void Start()
    {
        //CreateCircle();
    }

    public void CreateCircle()
    {
        for (int i = 0; i < scaleValues.Length; i++)
        {
            GameObject currentInstance = Instantiate(objectToInstanciate, this.transform);
            currentInstance.transform.position = this.transform.position;
            //currentInstance.transform.parent = this.transform;
            currentInstance.name = "CreatedObject" + i;

            this.transform.eulerAngles = new Vector3(0, 360f / scaleValues.Length * i, 0);
            currentInstance.transform.position = Vector3.forward * distance;

            circleOfObjects.Add(currentInstance);
        }
    }

    public void SetScales(float[] scales)
    {
        scaleValues = scales;
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void Visualizer()
    {
        for (int i = 0; i < scaleValues.Length; i++)
        {
            float baseScale = 10f;
            circleOfObjects[i].transform.localScale = new Vector3(baseScale, scaleValues[i] * maxScale, 10);
        }
    }

}