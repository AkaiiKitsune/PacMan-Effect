using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallEffect : MonoBehaviour
{
    bool canBeTrigger = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerShineColor(int index)
    {
        if ((index <= 30) && canBeTrigger)
        {
            GetComponent<ChangeMaterialProperties>().GoWhite();
            canBeTrigger = false;
        }
    }

    //Même chose que audessus
    public void TriggerResetColor(int index)
    {
        if (index <= 30)
        {
            canBeTrigger = true;
        }else{ 
            GetComponent<ChangeMaterialProperties>().ResetColor();
        }
    }
}
