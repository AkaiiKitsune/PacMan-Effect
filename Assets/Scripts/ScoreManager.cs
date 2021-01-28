using UnityEngine;


public class ScoreManager : MonoBehaviour
{
    [Header("Level Reference")]
    [SerializeField] private LevelParser level;
    [SerializeField] public PowerUp power;

    [SerializeField] private int score;
    [SerializeField] private UIManager UIManager;

    public void InitScore ()
    {
        score = 0;
        UIManager.TextScore.text = UIManager.AddPoint(score);
        UIManager.SetProgressionMax();
    }

    public void AddScore (TileType type)
    {
        if (type == TileType.Ball)
        {
            score += 10;
            UIManager.TextScore.text = UIManager.AddPoint(score);
            UIManager.GetProgression();
        }
        else if (type == TileType.Super)
        {
            score += 50;
            UIManager.TextScore.text = UIManager.AddPoint(score);
            UIManager.GetProgression();
            power.SuperPacman();
        }


    }

    public int ShowScore ()
    {
        return score;
    }
    
   
}
