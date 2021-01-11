using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float timeTillUpdate = 0.5f;
    [SerializeField] private bool isGame = false;

    [Header("Game Objects")]
    [SerializeField] private PacmanBehavior pacman;

    [Header("Levels")]
    [SerializeField] private int defaultSpawn = 0;
    [SerializeField] private List<LevelParser> levels;

    [Header("Display Scripts")]
    [SerializeField] private List<LevelDisplayer> levelDisplayers;

    [Header("Internal States")]
    [SerializeField] private MoveDir currDirection = MoveDir.Up;



    // Static singleton instance
    private static GameManager instance;

    // Static singleton property
    // GameManager.Instance.Whatever();
    public static GameManager Instance
    {
        // Here we use the ?? operator, to return 'instance' if 'instance' does not equal null
        // otherwise we assign instance to a new component and return that
        get { return instance ?? (instance = new GameObject("GameManager").AddComponent<GameManager>()); }
    }

    #region Initialisation
        private void Awake()
        {
            //Level Generation and Level Display logic
            foreach (LevelParser level in levels) level.InitLevel();
            foreach (LevelDisplayer displayer in levelDisplayers) displayer.DisplayLevel();
        }

        private void Start()
        {
            InitPacman();
            StartGame();
        }

        void InitPacman()
        {
            //Instantiating pacman's object
            pacman = Instantiate(pacman, levels[defaultSpawn].transform);
            pacman.name = "Pacman";
            pacman.transform.localPosition = new Vector3(pacman.transform.localPosition.x, pacman.transform.localPosition.y, pacman.transform.localPosition.z - 1);

            //Init pacman spawn coordinates
            pacman.level = levels[defaultSpawn];
            pacman.Spawn();
        }
    #endregion


    private void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        if (moveVertical > .1) currDirection = MoveDir.Up;
        else if (moveVertical < -.1) currDirection = MoveDir.Down;

        if (moveHorizontal > .1) currDirection = MoveDir.Right;
        else if (moveHorizontal < -.1) currDirection = MoveDir.Left;
    }

    #region Game Logic
    void StartGame()
        {
            isGame = true;
            StartCoroutine(UpdateGameLogic());
        }

        IEnumerator UpdateGameLogic()
        {
            while (isGame)
            {
                //Gameloop and update logic
                Debug.Log("TICK : " + Time.time);

            pacman.Move(currDirection);
                //Wait timeTillUpdate seconds till next update cycle
                yield return new WaitForSecondsRealtime(timeTillUpdate);
            }
        }
    #endregion
}