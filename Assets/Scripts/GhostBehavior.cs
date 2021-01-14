using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GhostType
{
    Blinky, //Rouge
    Pinky, //Rose
    Inky, //Cyan
    Clyde //Jaune
}

public enum BehaviorMode
{
    Chase,
    Scatter,
    Frightened,
}

public class GhostBehavior : MonoBehaviour
{
    [Header("Level Reference")]
    [SerializeField] public LevelParser level;

    [Header("Internal States")]
    public PacmanBehavior target;
    private MoveDir lastDir;
    [SerializeField] private MoveDir currentDir;
    [SerializeField] private Vector2 position;
    [SerializeField] public bool Spawned = false;

    [Header("Settings")]
    public GhostType type;


    public void Spawn()
    {
        position = new Vector2((int)LevelParser.mapWidth/2-1, (int)LevelParser.mapHeight/2);
        UpdatePosition();
    }


    public void ComputeNextMove()
    {
        if (Spawned)
        {
            Move();

            Vector2 target = ComputeTarget(type);
            //Vector2 target = ScatterTarget(type);

            currentDir = (MoveDir)GetTurnClosestToTarget(position, target, GetOpenTiles(position));

            //Debug
            Debug.DrawLine(transform.localPosition, Misc.ConvertToMatrixCoordinates(target), GetComponent<Renderer>().material.color, FindObjectOfType<GameManager>().timeTillUpdate, false);
        }
    }

    #region Movement handling
    private void Move()
    {
        switch (currentDir)
        {
            case MoveDir.Up:
                if (level.mapMatrix[(int)position.x, (int)position.y].neighbourUp.type != TileType.Wall) lastDir = currentDir;
                break;

            case MoveDir.Down:
                if (level.mapMatrix[(int)position.x, (int)position.y].neighbourDown.type != TileType.Wall) lastDir = currentDir;
                break;

            case MoveDir.Left:
                if (level.mapMatrix[(int)position.x, (int)position.y].neighbourLeft.type != TileType.Wall) lastDir = currentDir;
                break;

            case MoveDir.Right:
                if (level.mapMatrix[(int)position.x, (int)position.y].neighbourRight.type != TileType.Wall) lastDir = currentDir;
                break;
        }

        switch (lastDir)
        {
            case MoveDir.Up:
                if (level.mapMatrix[(int)position.x, (int)position.y].neighbourUp.type != TileType.Wall) position.y -= 1;
                break;

            case MoveDir.Down:
                if (level.mapMatrix[(int)position.x, (int)position.y].neighbourDown.type != TileType.Wall) position.y += 1;
                break;

            case MoveDir.Left:
                if (level.mapMatrix[(int)position.x, (int)position.y].neighbourLeft.type != TileType.Wall) position.x -= 1;
                break;

            case MoveDir.Right:
                if (level.mapMatrix[(int)position.x, (int)position.y].neighbourRight.type != TileType.Wall) position.x += 1;
                break;
        }

        UpdatePosition();
    }

    private void UpdatePosition() => transform.localPosition = Misc.ConvertToMatrixCoordinates(position);
    #endregion

    #region AI Logic
    private Vector2 ComputeTarget(GhostType ghost)
    {
        switch (ghost)
        {
            case GhostType.Blinky:
                return target.position;

            case GhostType.Pinky:
                switch (target.lastDir)
                {
                    case MoveDir.Up:
                        return new Vector2(Misc.ConstraintValueBetween((int)target.position.x - 4, 0, LevelParser.mapWidth), Misc.ConstraintValueBetween((int)target.position.y - 4, 0, LevelParser.mapHeight));
                    case MoveDir.Down:
                        return new Vector2(target.position.x, Misc.ConstraintValueBetween((int)target.position.y + 4, 0, LevelParser.mapHeight));
                    case MoveDir.Left:
                        return new Vector2(Misc.ConstraintValueBetween((int)target.position.x - 4, 0, LevelParser.mapWidth), target.position.y);
                    case MoveDir.Right:
                        return new Vector2(Misc.ConstraintValueBetween((int)target.position.x + 4, 0, LevelParser.mapWidth), target.position.y);
                    default:
                        return target.position;
                }


            default:
                return target.position;
        }
    }

    //Returns scatter targets for a given ghost type
    private Vector2 ScatterTarget(GhostType ghost)
    {
        switch (ghost)
        {
            case GhostType.Blinky:
                return new Vector2(LevelParser.mapWidth, 0);

            case GhostType.Pinky:
                return new Vector2(0, 0);

            case GhostType.Inky:
                return new Vector2(LevelParser.mapWidth, LevelParser.mapHeight);

            case GhostType.Clyde:
                return new Vector2(0, LevelParser.mapHeight);

            default:
                return new Vector2(LevelParser.mapWidth/2, LevelParser.mapHeight/2);
        }
    }

    //Converts enum values to vector2 offsets
    private Vector2 EnumToVectorDisplacement(MoveDir dir)
    {
        switch (dir)
        {
            case MoveDir.Up:
                return new Vector2(0, 1);
            case MoveDir.Down:
                return new Vector2(0, -1);
            case MoveDir.Left:
                return new Vector2(1, 0);
            case MoveDir.Right:
                return new Vector2(-1, 0);
            default: return new Vector2();
        }
    }

    //Get the tile the ghost just came from
    private int RotateAboutFace(int dirEnum) => (dirEnum + 2) % 4;

    //Get all open tiles around the ghost
    private bool[] GetOpenTiles(Vector2 pos)
    {

        bool[] openTiles = new bool[4];
        openTiles[(int)MoveDir.Up] = (level.mapMatrix[(int)pos.x, (int)pos.y].neighbourUp.type != TileType.Wall && level.mapMatrix[(int)pos.x, (int)pos.y].neighbourUp.type != TileType.Outside) ? true : false;
        openTiles[(int)MoveDir.Down] = (level.mapMatrix[(int)pos.x, (int)pos.y].neighbourDown.type != TileType.Wall && level.mapMatrix[(int)pos.x, (int)pos.y].neighbourDown.type != TileType.Outside) ? true : false;
        openTiles[(int)MoveDir.Left] = (level.mapMatrix[(int)pos.x, (int)pos.y].neighbourLeft.type != TileType.Wall && level.mapMatrix[(int)pos.x, (int)pos.y].neighbourLeft.type != TileType.Outside) ? true : false;
        openTiles[(int)MoveDir.Right] = (level.mapMatrix[(int)pos.x, (int)pos.y].neighbourRight.type != TileType.Wall && level.mapMatrix[(int)pos.x, (int)pos.y].neighbourRight.type != TileType.Outside) ? true : false;

        int oppDirEnum = RotateAboutFace((int)lastDir); // current opposite direction enum
        openTiles[oppDirEnum] = false;

        return openTiles;
    }

    //This figures out what the ghost's next move will be
    private int GetTurnClosestToTarget(Vector2 pos, Vector2 target, bool[] openTiles)
    {
        int dirEnum = 0;
        float minDist = Mathf.Infinity;
        float dist = 0;

        for (int i = 0; i < 4; i++)
        {
            if (openTiles[i])
            {
                dist = Vector2.Distance(pos, target + EnumToVectorDisplacement((MoveDir)i));
                //Debug.Log("Chemin testé : " + (MoveDir)i + ", Distance : " + dist);
                if (dist < minDist)
                {
                    minDist = dist;
                    dirEnum = i;
                }
            }
        }
        //Debug.Log("Chemin le plus court : " + (MoveDir)dirEnum);
        //Debug.Log("============");
        return dirEnum;
    }
    #endregion
}
