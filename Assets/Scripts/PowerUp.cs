using UnityEngine;
using System.Collections;


public enum Power
{
    PacGomme = 0,
    Toofast = 1,
    Shield = 2,
}
public class PowerUp : MonoBehaviour
{
    [Header("Internal logic")]
    private int nbPowerUp;
    private bool superPac;

    private void Start()
    {
        nbPowerUp = (System.Enum.GetNames(typeof(Power)).Length);
    }

    public void SuperPacman()
    {
        StartCoroutine(NotSuperPacman());
    }

    IEnumerator NotSuperPacman()
    {
        superPac = true;
        yield return new WaitForSecondsRealtime(10f);
        superPac = false;

    }
        
    public bool IsPacmanSuper ()
    {
        return superPac;
    }
    
}
