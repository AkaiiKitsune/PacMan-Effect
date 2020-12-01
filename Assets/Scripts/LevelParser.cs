using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelParser : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private TextAsset _maps;
    [SerializeField] private char[] _mapContent = null;

    private int _mapWidth = 29;


    // Start is called before the first frame update
    void Start()
    {
        _mapContent = _maps.text.ToCharArray();
        for (int i = 0; i < _mapContent.Length; i++)
        {
            if (_mapContent[i] == '|')
            {
                GameObject temp = Instantiate(_prefab, this.transform);
                Vector3 pos = new Vector3(i % _mapWidth, (int)-(i / _mapWidth), 0);
                temp.transform.localPosition += pos / 5;
            }
        }
    }
}
