using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Gestion du score
    [SerializeField] private Text TextScore;
    [SerializeField] private Text TextHighScore;

    [SerializeField] private int _score = 0;
    [SerializeField] private int _highScore = 10000;


    //gestion des pop up de point
    [SerializeField] private GameObject TexteVolant;


    //Gestion de l'affichage de la Vie
    [SerializeField] private GameObject ViePacMan;
    [SerializeField] private List<GameObject> Pv = new List<GameObject>();


    //Gestion de l'affichage de multiplicateur
    [SerializeField] private List<GameObject> MultiPoint = new List<GameObject>();


    //Gestion de l'affichage du power up
    [SerializeField] private GameObject PowerUp;


    //Gestion de l'affichage de la progression du niveau
    [SerializeField] private Image CercleProgres;

    [SerializeField] private int _progressionMax;   //total de pac gomme
    [SerializeField] private int _progression;      //pac gomme manger

    //Affichage du gameover
    [SerializeField] private Canvas UIGameOver;
    private bool _PacManNot = false;

    //Debug Log
    public bool _test = false;


    //========================= Start is called before the first frame update
    void Start()
    {
        /*TextScore.text = AddPoint(_score, 0);
        TextHighScore.text = AddPoint(_highScore, 0);

        //on fait aparaitre les pac-mans vie selon un ordre précis et on les stockes dans un tableau. 
        for (int i = 0; i < 3; i++)
        {
            GameObject newPv = Instantiate(ViePacMan, this.transform);

            //y = position du score - l'écartement entre les vies - la distance d'écartement entre les vies * le combientième de vie (deuxième, troisième, ...)
            newPv.transform.position += new Vector3(TextScore.transform.position.x, TextScore.transform.position.y - 1.5f - (0.25f * i), 0f);
            Pv.Add(newPv);
        }*/
    }

    //=========================Il sert pour le Debug 
    private void Update()
    {
        UpdateProgression();

        if (_test == true)
        {
            Debug.Log("Toucher :" + Touch());
            _test = false;
        }
    }

    //=========================Système d'adition de point    
    private string AddPoint(int score, int point)
    {
        score += point;

        int limit = 100000;
        string zero = "";
        // tant que le score sera inférieur a limit, zero aura un 0 de plus et limit sera divisée par 10 à chaque fois jusqu'à que la condition soit fausse
        while (score < limit)
        {
            zero += "0";
            limit /= 10;
        }
        return zero + score.ToString();
    }

    //=========================Gestion des textes
    public void FloatingText(GameObject instiator, string content, Color couleur)
    {
        var text = Instantiate(TexteVolant, instiator.transform.position, Quaternion.identity, transform);
        text.GetComponent<TextMesh>().text = content;
        text.GetComponent<TextMesh>().color = couleur;
    }

    //=========================Gestion de la vie et de son affichage
    public bool Touch()
    {
        if (Pv.Count > 0)
        {
            Destroy(Pv[Pv.Count - 1]);
            Pv.RemoveAt(Pv.Count - 1);
            DestroyFruits();
            PlusDePower();
            return true;
        }
        else { return false; }
    }

    //=========================Gestion de l'affichage des multiplicateurs.
    //Addition d'un fruit multiplicateur
    public void AddMultiPoint(GameObject fruit)
    {
        if (MultiPoint.Count < 5)
        {
            var newFruit = Instantiate(fruit, this.transform);

            float currentPos = 0.5f;
            float largeur = 0.5f;
            float longueur = 1f;
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


    //=========================Récupération des parmètres de progression du niveau
    public void GetProgression(int totaleGomme, int actuelGomme)
    {
        _progressionMax = totaleGomme;
        _progression = actuelGomme;
    }

    //=========================Gestion de l'affichage de la progression
    public void UpdateProgression()
    {
        float progresse = (float)_progression / (float)_progressionMax;
        //CercleProgres.fillAmount = progresse;
    }

    //=========================Affichage du game over
    public void GameOver()
    {
        if (_PacManNot == false)
        {
            Debug.Log("MORTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTTT");
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
