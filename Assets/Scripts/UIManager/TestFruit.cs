using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFruit : MonoBehaviour
{
    [SerializeField] private GameObject PowerUp;
    [SerializeField] private UIManager UI;

    [SerializeField] private bool _touchPowerUp = false;
    [SerializeField] private bool _UsePowerUp = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_touchPowerUp == true)
        {
            UI.AddMultiPoint(PowerUp);
            _touchPowerUp = false;
        }

        if (_UsePowerUp == true)
        {
            UI.PlusDePower();
            _UsePowerUp = false;
        }
    }
}
