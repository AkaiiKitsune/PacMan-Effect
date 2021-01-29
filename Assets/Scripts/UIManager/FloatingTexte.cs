using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;

public class FloatingTexte : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 3F;
    [SerializeField] private Vector3 Offset = new Vector3(0, 2, 0);
    [SerializeField] private Vector3 Randomize = new Vector3(2.5F, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, _lifeTime);

        gameObject.transform.localPosition += Offset + new Vector3(Random.Range(-Randomize.x, Randomize.x),
            Random.Range(-Randomize.y, Randomize.y),
            Random.Range(-Randomize.y, Randomize.y));
    }
}
