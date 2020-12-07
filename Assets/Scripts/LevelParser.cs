using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelParser : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private TextAsset _maps;
    [SerializeField] private char[] _mapContent = null;
    [SerializeField] private float _scale = 10;

    private const int _mapWidth = 28;
    private const int _mapHeight = 31;

    private int _mapIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        _mapContent = _maps.text.ToCharArray();

        for (int i = 0; i < _mapContent.Length; i++)
        {
            if (_mapContent[i] == '|')
            {
                GameObject temp = Instantiate(_prefab, this.transform);
                Vector3 pos = new Vector3((i % _mapWidth) - ((_prefab.transform.localScale.x*_scale*_mapWidth) / 2 - _prefab.transform.localScale.x*_scale/2), (int)-(i / _mapWidth) + ((_prefab.transform.localScale.y*_scale*_mapHeight)/2), 0);
                temp.transform.localPosition += pos / _scale;
            }
        }
    }
}
