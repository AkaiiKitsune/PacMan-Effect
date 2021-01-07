using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LevelParser))]
public class LevelDisplayer : MonoBehaviour
{
    [Header("Constants")]
    private const int mapWidth = 28;
    private const int mapHeight = 31;

    [Header("Map & Refs")]
    [SerializeField] private LevelParser mapMatrix;
    [SerializeField] private Transform origin;
    public TileDebug[,] displayTiles = new TileDebug[mapWidth, mapHeight];

    [Header("Settings")]
    public static float size = .7f;

    private void Start()
    {
        float tempx = origin.transform.position.x, tempy = this.transform.position.y;
        tempx -= (mapWidth / 2) * size;
        tempy += (mapHeight / 2) * size;

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                //Instantiate and setup the tile.
                Transform temp = Instantiate(mapMatrix.mapMatrix[x, y].currentBlock, new Vector3(tempx, tempy, 0), Quaternion.identity);
                temp.gameObject.AddComponent<TileDebug>().Init(mapMatrix.mapMatrix[x, y]);
                temp.name = mapMatrix.mapMatrix[x, y].index.ToString();
                temp.transform.parent = transform;
                temp.localScale *= size;
                displayTiles[x, y] = temp.GetComponent<TileDebug>();

                tempx += size;
            }
            //Reset the temporary position
            tempx -= mapWidth * size;
            tempy -= size;
        }

        this.transform.localRotation = origin.rotation;
    }
}
