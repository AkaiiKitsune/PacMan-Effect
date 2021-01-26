using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCercleEffect : MonoBehaviour
{
    
    [SerializeField] ParticleSystem PartSysCercle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerInstantiateCircle(int index)
    {
        ParticleSystem part = Instantiate(PartSysCercle);
        part.transform.localPosition = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
    }
}
