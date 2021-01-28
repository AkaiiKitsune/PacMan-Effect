using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;


public class GameManager : MonoBehaviour
{
    [Header("Settings")]
    public float startOffset = 1f;
    public float timeTillUpdate = 0.5f;
    public float timeTillSpawn = 3f;
    [SerializeField] public int pacmanLife = 3;

    [Header("Game Objects")]
    [SerializeField] private PacmanBehavior pacman;
    [SerializeField] private List<GhostBehavior> ghostPrefabs;

    [Header("Levels")]
    [SerializeField] private int defaultSpawn = 0;
    [SerializeField] private List<LevelParser> levels;

    [Header("Display Scripts")]
    [SerializeField] private List<LevelDisplayer> levelDisplayers;

    [Header("Internal States")]
    [SerializeField] private MoveDir currDirection = MoveDir.Up;
    [SerializeField] private bool isGame = false;
    private InputDevice device;
    private Vector2 inputStick;

    [Header("Chase Pattern")]
    private ChaseMode CurrentMode;
    [SerializeField] List<ChaseMode> ChaseModes;
    [SerializeField] List<int> ChaseTime;

    [Header("Score & PowerUp")]
    [SerializeField] private ScoreManager score;
    [SerializeField] private PowerUp power;

    [Header("UI")]
    [SerializeField] private UIManager UIManager;

    #region Initialisation
    private void Awake()
    {
        //Level Generation and Level Display logic
        foreach (LevelParser level in levels) level.InitLevel();
        foreach (LevelDisplayer displayer in levelDisplayers) displayer.DisplayLevel();
    }

    private void Start()
    {
        device = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);

        InitPacman();
        InitGhosts();
        score.InitScore();
        StartGame();
        UIManager.InitShowPV(pacmanLife);
    }

    void InitPacman()
    {
        //Instantiating pacman's object
        pacman = Instantiate(pacman, levels[defaultSpawn].transform);
        pacman.name = "Pacman";
        pacman.transform.localPosition = new Vector3(pacman.transform.localPosition.x, pacman.transform.localPosition.y, pacman.transform.localPosition.z - 1);

        //Init pacman spawn coordinates
        pacman.level = levels[defaultSpawn];
        pacman.score = score;
        pacman.Spawn();
    }


    void InitGhosts()
    {
        for (int i = 0; i < ghostPrefabs.Count; i++)
        {
            ghostPrefabs[i] = Instantiate(ghostPrefabs[i], levels[defaultSpawn].transform);
            ghostPrefabs[i].name = ghostPrefabs[i].type.ToString();
            ghostPrefabs[i].transform.localPosition = new Vector3(ghostPrefabs[i].transform.localPosition.x, ghostPrefabs[i].transform.localPosition.y, ghostPrefabs[i].transform.localPosition.z - 1);
            ghostPrefabs[i].target = pacman;

            //Init pacman spawn coordinates
            ghostPrefabs[i].level = levels[defaultSpawn];
            ghostPrefabs[i].Spawn();
        }
    }
    #endregion


    private void Update()
    {
        //float moveHorizontal = Input.GetAxis("Horizontal");
        //float moveVertical = Input.GetAxis("Vertical");        
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputStick);

        if (inputStick.y > .1) currDirection = MoveDir.Up;
        else if (inputStick.y < -.1) currDirection = MoveDir.Down;

        if (inputStick.x > .1) currDirection = MoveDir.Right;
        else if (inputStick.x < -.1) currDirection = MoveDir.Left;

        if (power.IsPacmanSuper()) CurrentMode = ChaseMode.Scatter;
    }

    #region Game Logic
    void StartGame()
    {
        isGame = true;
        StartCoroutine(UpdateGameLogic());
        StartCoroutine(UpdateChaseLogic());
        StartCoroutine(SpawnGhostsInOrder());
    }

    IEnumerator SpawnGhostsInOrder()
    {
        foreach(GhostBehavior ghost in ghostPrefabs)
        {
            ghost.Spawned = true;
            yield return new WaitForSecondsRealtime(timeTillSpawn);
        }
    }

    IEnumerator UpdateGameLogic()
    {
        yield return new WaitForSecondsRealtime(startOffset);

        while (isGame)
        {
            //Gameloop and update logic
           // Debug.Log("TICK : " + Time.time);

            //Update Pacman
            pacman.Move(currDirection);

            //Check if pacman collide an ennemy
                foreach (GhostBehavior ghost in ghostPrefabs) ghost.ComputeNextMove(CurrentMode);
            
                if (pacman.colliding && pacmanLife > 0 && !power.IsPacmanSuper())
                {
                    pacman.colliding = false;
                    pacman.lastDir = MoveDir.Up;
                    pacmanLife -= 1;
                    UIManager.Touch();
                    pacman.Spawn();

                    GhostRespawn();
                }
                else if (pacman.colliding && power.IsPacmanSuper()) { 
                    Debug.Log(pacman.collideName);
                    GameObject.Find(pacman.collideName).GetComponent<GhostBehavior>().Spawn();
                }
                else if (pacman.colliding && pacmanLife == 0) UIManager.GameOver();
                
                   
                


            //Wait timeTillUpdate seconds till next update cycle
            yield return new WaitForSecondsRealtime(timeTillUpdate);
        }
    }

    //Need help to fix ai respawn
    void GhostRespawn()
    {
        foreach (GhostBehavior ghost in ghostPrefabs)
        {
            ghost.Spawned = false;
            ghost.Spawn();
        }
        StartCoroutine(SpawnGhostsInOrder());
    }
    
    IEnumerator UpdateChaseLogic()
    {
        for(int i=0; i<ChaseModes.Count; i++)
        {
            CurrentMode = ChaseModes[i];
            yield return new WaitForSecondsRealtime(ChaseTime[i]);
        }
        CurrentMode = ChaseMode.Chase; //Whatever the last mode in the array was, defaults to chase till player either dies or level ends
    }
    #endregion
}