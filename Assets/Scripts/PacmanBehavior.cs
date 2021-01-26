using UnityEngine;


public enum MoveDir
{
    Up = 0,
    Right = 1,
    Down = 2,
    Left = 3
}

public class PacmanBehavior : MonoBehaviour
{
    [Header("Level Reference")]
    [SerializeField] public LevelParser level;

    [Header("Internal Logic")]
    [SerializeField] public Vector2 position;
    [SerializeField] public bool isAlive = true;
    [SerializeField] public MoveDir lastDir;
    [SerializeField] public bool colliding;

    [Header("Settings")]
    [SerializeField] public float Speed = 0.7f;
    [SerializeField] public float SpeedMultiplicator = 0.2f;

    [Header("Score Manager")]
    [SerializeField] public ScoreManager score;

    //Handle setting position at spawn time
    public void Spawn()
    {
        position = level.pacmanSpawn;
        UpdatePosition();
    }

    //Handle move logic
    public void Move(MoveDir dir)
    {
        if (position.x <= 0 && dir == MoveDir.Left)
        {
            position.x = LevelParser.mapWidth - 1;
            UpdatePosition();
        }
        else if (position.x >= LevelParser.mapWidth - 1 && dir == MoveDir.Right)
        {
            position.x = 0;
            UpdatePosition();
        }
        else
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
                    if (level.mapMatrix[(int)position.x, (int)position.y].neighbourUp.type != TileType.Wall && level.mapMatrix[(int)position.x, (int)position.y].neighbourUp.type != TileType.Outside)
                        position.y -= 1;
                    break;
                case MoveDir.Down:
                    if (level.mapMatrix[(int)position.x, (int)position.y].neighbourDown.type != TileType.Wall && level.mapMatrix[(int)position.x, (int)position.y].neighbourDown.type != TileType.Outside)
                        position.y += 1;
                    break;
                case MoveDir.Left:
                    if (level.mapMatrix[(int)position.x, (int)position.y].neighbourLeft.type != TileType.Wall && level.mapMatrix[(int)position.x, (int)position.y].neighbourLeft.type != TileType.Outside)
                        position.x -= 1;
                    break;
                case MoveDir.Right:
                    if (level.mapMatrix[(int)position.x, (int)position.y].neighbourRight.type != TileType.Wall && level.mapMatrix[(int)position.x, (int)position.y].neighbourRight.type != TileType.Outside)
                        position.x += 1;
                    break;
            }
            UpdatePosition();            
        }
        UpdateTile(); 
    }

    private void UpdatePosition() => transform.localPosition = Misc.ConvertToMatrixCoordinates(position);

    private void UpdateTile()
    {
        score.AddScore(level.mapMatrix[(int)position.x, (int)position.y].type);
        level.mapMatrix[(int)position.x, (int)position.y].SetType(TileType.Air);
    }

    public bool EnnemyCollide(Vector2 ennemyPos)
    {
        if (new Vector2((int)ennemyPos.x, (int)ennemyPos.y) == new Vector2((int)position.x, (int)position.y))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        colliding = true;
    }
    void OnCollisionExit(Collision collision)
    {
        colliding = false;
    }


    /// TODO Implement this hell
    private void ChangeTerrain(LevelParser levelToJumpTo) { }
}
