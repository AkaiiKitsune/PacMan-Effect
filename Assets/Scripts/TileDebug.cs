using System.Collections.Generic;
using UnityEngine;
public class TileDebug : MonoBehaviour
{
    [Header("Type & Settings")]
    [SerializeField] private TileType type;
    [SerializeField] private int index;

    [Header("Voisins")]
    [SerializeField] private TileObject neighbourUp;
    [SerializeField] private TileObject neighbourDown;
    [SerializeField] private TileObject neighbourLeft;
    [SerializeField] private TileObject neighbourRight;

    [Header("Blocks")]
    [SerializeField] private Transform currentBlock = null;

    public void Init(TileObject tile)
    {
        type = tile.type;
        index = tile.index;
        neighbourUp = tile.neighbourUp;
        neighbourDown = tile.neighbourDown;
        neighbourLeft = tile.neighbourLeft;
        neighbourRight = tile.neighbourDown;
        currentBlock = tile.currentBlock;
    }
}