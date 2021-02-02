using System.Collections.Generic;
using UnityEngine;

public class LevelParser : MonoBehaviour
{
    [Header("Constants")]
    public const int mapWidth = 28;
    public const int mapHeight = 31;

    [Header("Map")]
    public TileObject[,] mapMatrix = new TileObject[mapWidth, mapHeight];
    private char[] _mapContentString = null;
    [SerializeField] private TextAsset _maps;
    [SerializeField] private int mapIndex;
    public LevelDisplayer displayer;

    [Header("Blocks")]
    public List<Transform> listBlocks = new List<Transform>(5);

    [Header("Pacman Spawn Tile")]
    public Vector2 pacmanSpawn;

    //Debug functions
    [ContextMenu("Dump MapMatrix To Text")]
    void DumpMapMatrixToText()
    {
        using (System.IO.TextWriter tw = new System.IO.StreamWriter("mapMatrix.txt"))
        {
            for (int j = 0; j < mapHeight; j++)
            {
                for (int i = 0; i < mapWidth; i++)
                {
                    string UDLR = null;
                    if (j > 0) UDLR += mapMatrix[i, j].neighbourUp.index.ToString().PadRight(3, '-'); else UDLR += "nul";
                    UDLR += ",";
                    if (j < mapHeight - 1) UDLR += mapMatrix[i, j].neighbourDown.index.ToString().PadRight(3, '-'); else UDLR += "nul";
                    UDLR += ",";
                    if (i > 0) UDLR += mapMatrix[i, j].neighbourLeft.index.ToString().PadRight(3, '-'); else UDLR += "nul"; ;
                    UDLR += ",";
                    if (i < mapWidth - 1) UDLR += mapMatrix[i, j].neighbourRight.index.ToString().PadRight(3, '-'); else UDLR += "nul"; ;
                    tw.Write(mapMatrix[i, j].type.ToString().Substring(0, 1) + ":" + mapMatrix[i, j].index.ToString().PadRight(3, '-') + ";" + UDLR + " ");
                }
                tw.WriteLine();
            }
        }
    }

    private void Awake()
    {
        mapIndex = Random.Range(1, 400);
        //Initialising the map character array to be parsed

        int lowerBound = (mapIndex + (mapIndex * 868));

        string map = _maps.text;

        Debug.Log("Map length : " + map.Length + ", lowerBound:" + lowerBound);

        map = map.Substring(lowerBound, 868);

        _mapContentString = map.ToCharArray();
        
    }

    public void InitLevel()
    {
        //For each tile in the matrix...
        for (int y = 0; y < mapHeight; y++)
            for (int x = 0; x < mapWidth; x++)
            {
                //Create a tile
                mapMatrix[x, y] = MakeTileObject(x, y);

                //And set it's neighbours
                SetTileNeighbour(x, y);
            }
        pacmanSpawn = FindPacmanSpawn(14, 16);
    }

    //Create the tiles in the matrix
    private TileObject MakeTileObject(int x, int y)
    {

        int index = x + (y * mapWidth); //Get the current index in a more readable format, to display in the debug matrix
        char currentChar = _mapContentString[index]; //Get the currently parsed character

        TileType _type;

        //Return a proper tiletype based on the current parsed character
        switch (currentChar)
        {
            case '|':
                _type = TileType.Wall;
                break;

            case '.':
                _type = TileType.Ball;
                break;

            case 'o':
                _type = TileType.Super;
                break;

            case '_':
                _type = TileType.Outside;
                break;

            default:
                _type = TileType.Air;
                break;
        }

        if (index == 349 || index == 350) _type = TileType.Outside; //Makes ghost exit outside blocks
        if (index == 401 || index == 410) _type = TileType.Fruit; //Fruit Spawn

        return new TileObject(index, new Vector2(x, y), _type, listBlocks, this);
    }

    //Set each tile's 4 neighbours accordingly
    private void SetTileNeighbour(int x, int y)
    {
        if (x > 0)
        {
            mapMatrix[x, y].neighbourLeft = mapMatrix[x - 1, y];
            mapMatrix[x - 1, y].neighbourRight = mapMatrix[x, y];
        }

        if (y > 0)
        {
            mapMatrix[x, y].neighbourUp = mapMatrix[x, y - 1];
            mapMatrix[x, y - 1].neighbourDown = mapMatrix[x, y];
        }
    }

    private Vector2 FindPacmanSpawn(int x, int y)
    {
        int count = 0;

        while (count < 2 || y > 31)
        {

            if (mapMatrix[x, y].type == TileType.Air)
            {
                y++;
                count++;
            }

            else
            {
                y++;
            }
        }
        Vector2 m_spawn = new Vector2(x, y-1);
        return m_spawn;
    }

}
