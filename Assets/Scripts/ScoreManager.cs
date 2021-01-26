using UnityEngine;


public class ScoreManager : MonoBehaviour
{
    [Header("Level Reference")]
    [SerializeField] private LevelParser level;
    [SerializeField] public PowerUp power;

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
        else if (type == TileType.Super)
        {
            score += 50;
            power.SuperPacman();
        }


    }

    public int ShowScore ()
    {
        return score;
    }
    
   
}
