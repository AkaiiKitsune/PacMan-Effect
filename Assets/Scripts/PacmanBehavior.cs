using UnityEngine;


public enum MoveDir
{
    Up,
    Down,
    Left,
    Right
}

public class PacmanBehavior : MonoBehaviour
{
    [Header("Level Reference")]
    [SerializeField] public LevelParser level;

    [Header("Internal Logic")]
    [SerializeField] public Vector2 position;
    [SerializeField] public bool isAlive = true;
    [SerializeField] private MoveDir lastDir;

    //Handle setting position at spawn time
    public void Spawn()
    {
        position = level.pacmanSpawn;
        UpdatePosition();
    }

    //Handle move logic
    public void Move(MoveDir dir)
    {
        switch (dir)
        {
            case MoveDir.Up:
                if (level.mapMatrix[(int)position.x, (int)position.y].neighbourUp.type != TileType.Wall && level.mapMatrix[(int)position.x, (int)position.y - 1].neighbourUp.type != TileType.Outside) lastDir = dir;
                break;
            
            case MoveDir.Down:
                if (level.mapMatrix[(int)position.x, (int)position.y].neighbourDown.type != TileType.Wall && level.mapMatrix[(int)position.x, (int)position.y - 1].neighbourDown.type != TileType.Outside) lastDir = dir;
                break;
            
            case MoveDir.Left:
                if (level.mapMatrix[(int)position.x, (int)position.y].neighbourLeft.type != TileType.Wall && level.mapMatrix[(int)position.x, (int)position.y - 1].neighbourLeft.type != TileType.Outside) lastDir = dir;
                break;
            
            case MoveDir.Right:
                if (level.mapMatrix[(int)position.x, (int)position.y].neighbourRight.type != TileType.Wall && level.mapMatrix[(int)position.x, (int)position.y - 1].neighbourRight.type != TileType.Outside) lastDir = dir;
                break;
        }

        switch (lastDir)
        {
            case MoveDir.Up:
                if (level.mapMatrix[(int)position.x, (int)position.y].neighbourUp.type != TileType.Wall && level.mapMatrix[(int)position.x, (int)position.y - 1].neighbourUp.type != TileType.Outside)
                {
                    level.mapMatrix[(int)position.x, (int)position.y].neighbourUp.type = TileType.Air; //TO CHANGE TO ADD POINTS IF YOU EAT A BALL
                    position.y -= 1;
                }
                UpdatePosition();
                break;

            case MoveDir.Down:
                if (level.mapMatrix[(int)position.x, (int)position.y].neighbourDown.type != TileType.Wall && level.mapMatrix[(int)position.x, (int)position.y].neighbourDown.type != TileType.Outside)
                {
                    level.mapMatrix[(int)position.x, (int)position.y].neighbourDown.type = TileType.Air; //TO CHANGE TO ADD POINTS IF YOU EAT A BALL
                    position.y += 1;
                }
                UpdatePosition();
                break;

            case MoveDir.Left:
                if (level.mapMatrix[(int)position.x, (int)position.y].neighbourLeft.type != TileType.Wall && level.mapMatrix[(int)position.x, (int)position.y].neighbourLeft.type != TileType.Outside)
                {
                    level.mapMatrix[(int)position.x, (int)position.y].neighbourLeft.type = TileType.Air; //TO CHANGE TO ADD POINTS IF YOU EAT A BALL
                    position.x -= 1;
                }
                UpdatePosition();
                break;

            case MoveDir.Right:
                if (level.mapMatrix[(int)position.x, (int)position.y].neighbourRight.type != TileType.Wall && level.mapMatrix[(int)position.x, (int)position.y].neighbourRight.type != TileType.Outside)
                {
                    level.mapMatrix[(int)position.x, (int)position.y].neighbourRight.type = TileType.Air; //TO CHANGE TO ADD POINTS IF YOU EAT A BALL
                    position.x += 1;
                }
                UpdatePosition();
                break;
        }
    }

    public void UpdatePosition() => transform.localPosition = new Vector3(position.x*LevelDisplayer.size - ((LevelParser.mapWidth*LevelDisplayer.size)/2),
                                                                                     - position.y*LevelDisplayer.size + ((LevelParser.mapHeight * LevelDisplayer.size) / 2) - LevelDisplayer.size / 2, 
                                                                                     transform.localPosition.z);


    /// TODO Implement this hell
    public void ChangeTerrain(LevelParser levelToJumpTo) { }
}
