using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithRhythm : MonoBehaviour
{
    Vector3 initialPos;
    [SerializeField] private float xStep = 50f;
    [SerializeField] private int nbStep = 5;
    private int currentStepIndex = 0;
    bool canBeTrigger = true;

    // Start is called before the first frame update
    void Start()
    {
        transform.localPosition = transform.localPosition + Vector3.back * xStep * nbStep / 2f;
        initialPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Il est appeler directement depuis le système d'évènements de AudioSync
    public void TriggerMove(int index)
    {
        if ((index == 0) && canBeTrigger)
        {
            transform.localPosition = initialPos + Vector3.forward * xStep * (currentStepIndex % nbStep);
            currentStepIndex++;

            canBeTrigger = false;
        }
    
    }

    //Pareil que au dessus
    public void ResetTrigger(int index)
    {
        if (index == 0)
        {
            canBeTrigger = true;
        }
      
    }
}
