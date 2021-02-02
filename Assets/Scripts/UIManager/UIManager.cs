using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Gestion du score")]
    public TextMeshProUGUI TextScore;
    public TextMeshProUGUI TextHighScore;
    [SerializeField] private ScoreManager ScoreManager;


    [Header("gestion des pop up")]
    [SerializeField] private GameObject TexteVolant;


    [Header("affichage de la Vie")]
    [SerializeField] private ParticleManager ParticleManager;

    [SerializeField] private GameObject ViePacMan;
    private List<GameObject> Pv = new List<GameObject>();


    [Header("affichage de multiplicateur")]
    private List<GameObject> MultiPoint = new List<GameObject>();


    [Header("affichage du power up")]
    private GameObject PowerUp;


    [Header("affichage de la progression du niveau")]
    [SerializeField] private LevelDisplayer level;
    [SerializeField] private Image CercleProgres;

    [SerializeField] private int _progressionMax;   //total de pac gomme
    [SerializeField] private int _progression;      //pac gomme manger

    [Header("gameover")]
    [SerializeField] private Canvas UIGameOver;
    private bool _PacManNot = false;

    //Debug Log
    public bool _test = false;



    //=========================Il sert pour le Debug 
    /*private void Update()
    {
    }*/

    //=========================Système d'adition de point    
    public string AddPoint(int score)
    {
        Debug.Log(score.ToString());
        int limit = 100000;
        string zero = "";
        // tant que le score sera inférieur a limit, zero aura un 0 de plus et limit sera divisée par 10 à chaque fois jusqu'à que la condition soit fausse
        while (score < limit)
        {
            zero += "0";
            limit /= 10;
        }
        return (zero + score).ToString();
    }

    //=========================Gestion des textes
    public void FloatingText(Transform instiator, string content, Color couleur)
    {
        var text = Instantiate(TexteVolant, instiator.position, Quaternion.identity, transform);
        text.GetComponent<TextMesh>().text = content;
        text.GetComponent<TextMesh>().color = couleur;
    }

    #region Vie PacMan
    //=========================Initialisation de l'affichage de la vie
    public void InitShowPV(int vie)
    {
        //on fait aparaitre les pac-mans vie selon un ordre précis et on les stockes dans un tableau. 
        for (int i = 0; i < vie; i++)
        {
            GameObject newPv = Instantiate(ViePacMan, this.transform);
            //y = position du score - l'écartement entre les vies - la distance d'écartement entre les vies * le combientième de vie (deuxième, troisième, ...)
            newPv.transform.position += new Vector3(TextScore.transform.position.x, TextScore.transform.position.y - 12f - (1.5f * i), 0f);
            Pv.Add(newPv);
        }
    }

    //=========================Gestion de la vie et de son affichage
    public bool Touch()
    {
        if (Pv.Count > 0)
        {
            ParticleManager.ParticulePacMan(Pv[Pv.Count - 1].transform);
            Destroy(Pv[Pv.Count - 1]);
            Pv.RemoveAt(Pv.Count - 1);
            DestroyFruits();
            PlusDePower();
            return true;
        }
        else { return false; }
    }
    #endregion


    #region Multiplicateurs
    //=========================Gestion de l'affichage des multiplicateurs.
    //Addition d'un fruit multiplicateur
    public void AddMultiPoint(GameObject fruit)
    {
        if (MultiPoint.Count < 5)
        {
            var newFruit = Instantiate(fruit, this.transform);

            float currentPos = 6f;
            float largeur = 2f;
            float longueur = 4f;
            switch (MultiPoint.Count)
            {
                case 0:
                    newFruit.transform.position += new Vector3(TextScore.transform.position.x - longueur / 2, TextScore.transform.position.y - currentPos - 0, 0f);
                    break;
                case 1:
                    newFruit.transform.position += new Vector3(TextScore.transform.position.x + longueur / 2, TextScore.transform.position.y - currentPos - 0, 0f);
                    break;
                case 2:
                    newFruit.transform.position += new Vector3(TextScore.transform.position.x - longueur / 2, TextScore.transform.position.y - currentPos - largeur, 0f);
                    break;
                case 3:
                    newFruit.transform.position += new Vector3(TextScore.transform.position.x + longueur / 2, TextScore.transform.position.y - currentPos - largeur, 0f);
                    break;
                case 4:
                    newFruit.transform.position += new Vector3(TextScore.transform.position.x, TextScore.transform.position.y - currentPos - largeur / 2, 0f);
                    break;
            }

            MultiPoint.Add(newFruit);
        }
        else
        {
            Debug.Log("Les fruit sont au max !!!");
        }
    }

    //=========================Supression des multiplicateur
    public void DestroyFruits()
    {
        while (MultiPoint.Count > 0)
        {
            Destroy(MultiPoint[MultiPoint.Count - 1]);
            MultiPoint.RemoveAt(MultiPoint.Count - 1);
        }
    }
    #endregion

    #region Power Up
    //=========================Gestion de l'affichage du power up
    //Ajout de du power up et affichage
    public void TouchPower(GameObject newPowerUp)
    {
        if (PowerUp == null)
        {
            PowerUp = Instantiate(newPowerUp, this.transform);
            PowerUp.transform.position += new Vector3(TextHighScore.transform.position.x, TextHighScore.transform.position.y - 2f, 0f);
        }
    }


    //=========================Na plus le power up
    public void PlusDePower()
    {
        Destroy(PowerUp);
    }
    #endregion

    #region Progresion
    //=========================Initialisation de la progression
    public void SetProgressionMax()
    {
        _progressionMax = 0;
        _progression = 0;
            for (int x = 0; x < LevelParser.mapWidth; x++)
                for (int y = 0; y < LevelParser.mapHeight; y++)
                    if (level.displayTiles[x, y].type == TileType.Ball || level.displayTiles[x, y].type == TileType.Super)
                        _progressionMax++;
                
            
        
    }
    //=========================Récupération des parmètres de progression du niveau
    public void GetProgression()
    {
        _progression++;
        UpdateProgression();
    }

    //=========================Gestion de l'affichage de la progression
    private void UpdateProgression()
    {
        float progresse = (float)_progression / (float)_progressionMax;
        CercleProgres.fillAmount = progresse;

        if(_progression >= _progressionMax) SceneManager.LoadScene("Menu");
    }
    #endregion

    //=========================Affichage du game over
    public void GameOver()
    {
        SceneManager.LoadScene("Menu");
        if (_PacManNot == false)
        {
            UIGameOver.gameObject.SetActive(true); 
            _PacManNot = true;
        }
        else
        {
            UIGameOver.gameObject.SetActive(false);
            _PacManNot = false;
        }
    }
}
