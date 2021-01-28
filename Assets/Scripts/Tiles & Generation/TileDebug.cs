using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum cameFrom { Up, Down, Left, Right}
public class TileDebug : MonoBehaviour
{
    public const int mapWidth = 28;
    public const int mapHeight = 31;

    [Header("Type & Settings")]
    private TileObject tile;
    public TileType type;
    [SerializeField] private int index;

    [Header("Neighbours")]
    [SerializeField] private TileDebug neighbourUp;
    [SerializeField] private TileDebug neighbourDown;
    [SerializeField] private TileDebug neighbourLeft;
    [SerializeField] private TileDebug neighbourRight;

    public void Init(TileObject _tile)
    {
        tile = _tile;
        type = _tile.type;
        index = _tile.index;

        SetTileNeighbour(_tile.index % mapWidth, (int)(_tile.index / mapWidth));

        switch (type) {
            case TileType.Wall:
                this.transform.localPosition -= new Vector3(0, 0, LevelDisplayer.size);
                break;

            case TileType.Outside:
                this.transform.localPosition -= new Vector3(0, 0, LevelDisplayer.size);
                break;
        }
    }

    public void SetAir()
    {
        this.gameObject.GetComponentInChildren<Renderer>().enabled = false;
    }

    private void SetTileNeighbour(int x, int y)
    {
        if (x > 0)
        {
            neighbourLeft = GameObject.Find(tile.neighbourLeft.index.ToString()).GetComponent<TileDebug>();
            neighbourLeft.neighbourRight = this;
        }

        if (y > 0)
        {
            neighbourUp = GameObject.Find(tile.neighbourUp.index.ToString()).GetComponent<TileDebug>();
            neighbourUp.neighbourDown = this;
        }
    }
}