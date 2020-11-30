using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelParser : MonoBehaviour
{
    [SerializeField] private GameObject Prefab;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(Prefab, this.gameObject.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
