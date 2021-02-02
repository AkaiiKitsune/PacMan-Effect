using System.Collections.Generic;
using UnityEngine;

public enum TileType 
{ 
    Wall = 0, 
    Ball = 1, 
    Super = 2, 
    Air = 3, 
    Outside = 4, 
    Fruit = 5
}

public class TileObject
{
    public TileType type;
    public int index;
    public Vector2 position;
    public LevelParser parser;

    public TileObject neighbourUp;
    public TileObject neighbourDown;
    public TileObject neighbourLeft;
    public TileObject neighbourRight;

    public List<Transform> listBlocks;
    public Transform currentBlock = null;

    public TileObject(int _index, Vector2 _position, TileType _type, List<Transform> _listBlocks, LevelParser _parser)
    {
        type = _type;
        index = _index;
        position = _position;
        listBlocks = _listBlocks;
        parser = _parser;

        Init();
    }

    public void SetType(TileType _type)
    {
        type = _type;
        if (type == TileType.Air) parser.displayer.displayTiles[(int)position.x, (int)position.y].SetAir();
    }

    private void Init()
    {
        currentBlock = listBlocks[(int)type];
    }
}
