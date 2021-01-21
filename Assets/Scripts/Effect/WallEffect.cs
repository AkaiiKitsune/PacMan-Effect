using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallEffect : MonoBehaviour
{
    [SerializeField] private List<GameObject> _wallObject = new List<GameObject>();
    
    //private int[] cpt = new int[8];
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SendGameObject(GameObject nouvMur)
    {
        _wallObject.Add(nouvMur);
    }

    public void TriggerShineColor(int _cptBPM)
    {
        int nb;
        
        if (_cptBPM % 8 == 0)
        {
            nb = 90;
        }
        else 
        {
            nb = 10;
        }

        
        for (int i = 0 ; i < nb; i++)
        {
            _wallObject[Random.Range( 0, _wallObject.Count)].GetComponent<ChangeMaterialProperties>().GoWhite();
        }
    }
}
