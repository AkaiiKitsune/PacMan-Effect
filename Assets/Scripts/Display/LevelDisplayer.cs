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

    [Header("Walls")]
    public List<Transform> listWall = new List<Transform>(4);

    [Header("Settings")]
    public static float size = .7f;

    [Header("UI and Effect")]
    [SerializeField] private TileEffect tileEffect;
    [SerializeField] private UIManager UIManager;

    public void DisplayLevel()
    {
        float tempx = origin.transform.position.x, tempy = this.transform.position.y;
        tempx -= (mapWidth / 2) * size;
        tempy += (mapHeight / 2) * size;



        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                //Instantiate and setup the tile.
                //14;13 / 15;13 : Spawn tiles
                Transform temp = (mapMatrix.mapMatrix[x, y].type == TileType.Wall || (x == 13 && y == 12) || (x == 14 && y == 12)) ? MakeWall(new Vector2(x, y), new Vector3(tempx, tempy, 0)) : Instantiate(mapMatrix.mapMatrix[x, y].currentBlock, new Vector3(tempx, tempy, 0), Quaternion.identity);
                temp.gameObject.AddComponent<TileDebug>().Init(mapMatrix.mapMatrix[x, y]);
                
                if (mapMatrix.mapMatrix[x, y].type == TileType.Wall) temp.gameObject.AddComponent<ChangeMaterialProperties>();
                

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

        tileEffect.Init();
    }

    Transform MakeWall(Vector2 matrix, Vector3 pos)
    {
        Transform temp;
        int neighbours = 0;
        if (CheckIfWall(matrix.x, matrix.y - 1)) neighbours++; //UP
        if (CheckIfWall(matrix.x, matrix.y + 1)) neighbours++; //DOWN
        if (CheckIfWall(matrix.x + 1, matrix.y)) neighbours++; //RIGHT
        if (CheckIfWall(matrix.x - 1, matrix.y)) neighbours++; //LEFT


        if (neighbours == 2)
        {
            temp = Instantiate(listWall[0], pos, Quaternion.identity);
            if (CheckIfWall(matrix.x, matrix.y - 1) && CheckIfWall(matrix.x + 1, matrix.y)) temp.localScale = new Vector3(-temp.localScale.x, temp.localScale.y, temp.localScale.z);
            else if (CheckIfWall(matrix.x, matrix.y + 1) && CheckIfWall(matrix.x - 1, matrix.y)) temp.localScale = new Vector3(temp.localScale.x, -temp.localScale.y, temp.localScale.z);
            else if (CheckIfWall(matrix.x, matrix.y + 1) && CheckIfWall(matrix.x + 1, matrix.y)) temp.localScale = new Vector3(-temp.localScale.x, -temp.localScale.y, temp.localScale.z);
            return temp;
        }

        if (neighbours == 3)
        {
            if (CheckIfWall(matrix.x, matrix.y - 1) && CheckIfWall(matrix.x, matrix.y + 1) && CheckIfWall(matrix.x - 1, matrix.y))
            {
                temp = Instantiate(listWall[2], pos, Quaternion.identity);
                temp.localScale = new Vector3(temp.localScale.x, temp.localScale.y, temp.localScale.z);
                return temp;
            }
            if (CheckIfWall(matrix.x, matrix.y - 1) && CheckIfWall(matrix.x, matrix.y + 1) && CheckIfWall(matrix.x + 1, matrix.y))
            {
                temp = Instantiate(listWall[2], pos, Quaternion.identity);
                temp.localScale = new Vector3(-temp.localScale.x, temp.localScale.y, temp.localScale.z);
                return temp;
            }
            if (CheckIfWall(matrix.x, matrix.y - 1) && CheckIfWall(matrix.x - 1, matrix.y) && CheckIfWall(matrix.x + 1, matrix.y))
            {
                temp = Instantiate(listWall[3], pos, Quaternion.Euler(0, 90, 90));
                temp.localScale = new Vector3(-temp.localScale.x, temp.localScale.y, temp.localScale.z);
                return temp;
            }

            temp = Instantiate(listWall[3], pos, Quaternion.Euler(0, 90, 90));
            temp.localScale = new Vector3(temp.localScale.x, temp.localScale.y, temp.localScale.z);
            return temp;
        }

        if (!CheckIfWall(matrix.x - 1, matrix.y - 1))
        {
            temp = Instantiate(listWall[1], pos, Quaternion.Euler(0, 0, 90));
            temp.localScale = new Vector3(temp.localScale.x, - temp.localScale.y, temp.localScale.z);
            return temp;
        }

        if (!CheckIfWall(matrix.x - 1, matrix.y + 1))
        {
            temp = Instantiate(listWall[1], pos, Quaternion.Euler(0, 0, 90));
            temp.localScale = new Vector3(- temp.localScale.x, -temp.localScale.y, temp.localScale.z);
            return temp;
        }

        if (!CheckIfWall(matrix.x + 1, matrix.y + 1))
        {
            temp = Instantiate(listWall[1], pos, Quaternion.Euler(0, 0, 90));
            temp.localScale = new Vector3(-temp.localScale.x, temp.localScale.y, temp.localScale.z);
            return temp;
        }

        temp = Instantiate(listWall[1], pos, Quaternion.Euler(0, 0, 90));
        temp.localScale = new Vector3(temp.localScale.x, temp.localScale.y, temp.localScale.z);
        return temp;
    }

    bool CheckIfWall(float _x, float _y)
    {
        int x = (int)_x;
        int y = (int)_y;

        if (x < 0) x = 0;
        if (x >= mapWidth) x = mapWidth - 1;

        if (y < 0) y = 0;
        if (y >= mapHeight) y = mapHeight - 1;

        if (mapMatrix.mapMatrix[x, y].type == TileType.Wall) return true;
        if (mapMatrix.mapMatrix[x, y].type == TileType.Outside) return true;
        return false;
    }
}
