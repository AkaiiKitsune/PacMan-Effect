using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [Header("Cercles de Particule")]
    [SerializeField] ParticleSystem PartSysCercle;

    [Header("Désintégration PacMan")]
    [SerializeField] ParticleSystem PartSysPacMan;

    public void ParticulePacMan(Transform pPacMan)
    {
        ParticleSystem part = Instantiate(PartSysPacMan);
        part.transform.localPosition = new Vector3(pPacMan.position.x, pPacMan.position.y, pPacMan.position.z);
        part.transform.parent = this.transform;
    }

    public void TriggerInstantiateCircle(int index)
    {
        ParticleSystem part = Instantiate(PartSysCercle);
        part.transform.localPosition = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        part.transform.parent = this.transform;
    }
}

