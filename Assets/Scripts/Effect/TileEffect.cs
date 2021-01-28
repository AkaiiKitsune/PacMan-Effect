using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEffect : MonoBehaviour
{
    [SerializeField] private LevelDisplayer level;

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


        for (int i = 0; i < nb; i++)
        {
            int x;
            int y;
            while (true) { 
                x = Random.Range(0, LevelParser.mapWidth);
                y = Random.Range(0, LevelParser.mapHeight);
                if (level.displayTiles[x, y].type == TileType.Wall)
                {
                    break;
                }
            }
            level.displayTiles[x,y].GetComponent<ChangeMaterialProperties>().GoWhite();
            
        }
    }
}