using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEffect : MonoBehaviour
{
    [SerializeField] private LevelDisplayer level;
    [SerializeField] private ChangeMaterialProperties background;
    [SerializeField] private List<ChangeMaterialProperties> tiles = new List<ChangeMaterialProperties>();
    [SerializeField] private int nb;

    public void Init()
    {
        for (int x = 0; x < LevelParser.mapWidth; x++)
            for (int y = 0; y < LevelParser.mapHeight; y++)
                if (level.displayTiles[x, y].type == TileType.Wall)
                    tiles.Add(level.displayTiles[x, y].GetComponent<ChangeMaterialProperties>());
    }

    public void TriggerShineColor(int _cptBPM)
    {
        if (_cptBPM % 8 == 0)
             nb = 90;
        else nb = 10;
        for (int i = 0; i < nb; i++)tiles[Random.Range(0, tiles.Count)].GoWhite();

        if (_cptBPM % 16 == 0) background.GoWhite();
        else if (_cptBPM % 2 == 0) background.GoDark();



    }
}