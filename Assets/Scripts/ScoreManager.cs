using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("Level Reference")]
    [SerializeField] private LevelParser level;

    [SerializeField] private int score;

    public void InitScore ()
    {
        score = 0;
    }

    public void AddScore (TileType type)
    {
        if (type == TileType.Ball)
        {
            score += 10;
        }
    }

    public int ShowScore ()
    {
        return score;
    }
    
   
}
