using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;


public class GameManager : MonoBehaviour
{
    [Header("Settings")]
    public float timeTillUpdate = 0.5f;
    public float timeTillSpawn = 3f;
    [SerializeField] public int pacmanLife = 3;

    [Header("Game Objects")]
    [SerializeField] private PacmanBehavior pacman;
    [SerializeField] public List<GhostBehavior> ghostPrefabs;

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
    private bool exitButton;
    public bool triggerUpdate = false;

    [Header("Chase Pattern")]
    private ChaseMode CurrentMode;
    [SerializeField] List<ChaseMode> ChaseModes;
    [SerializeField] List<int> ChaseTime;

    [Header("Score & PowerUp")]
    [SerializeField] private ScoreManager score;
    [SerializeField] public PowerUp power;

    [Header("UI")]
    [SerializeField] private UIManager UIManager;

    #region Initialisation
    private void Start()
    {
        //Level Generation and Level Display logic
        foreach (LevelParser level in levels) level.InitLevel();
        foreach (LevelDisplayer displayer in levelDisplayers) displayer.DisplayLevel();

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
            ghostPrefabs[i].transform.localPosition = new Vector3(ghostPrefabs[i].transform.localPosition.x, ghostPrefabs[i].transform.localPosition.y, ghostPrefabs[i].transform.localPosition.z - 1f);
            ghostPrefabs[i].target = pacman;
            ghostPrefabs[i].gameObject.transform.Rotate(-90.0f, 0.0f, 0.0f);
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
        device.TryGetFeatureValue(CommonUsages.secondaryButton, out exitButton);

        if (inputStick.y > .15) currDirection = MoveDir.Up;
        else if (inputStick.y < -.15) currDirection = MoveDir.Down;

        if (inputStick.x > .15) currDirection = MoveDir.Right;
        else if (inputStick.x < -.15) currDirection = MoveDir.Left;

        if(exitButton) SceneManager.LoadScene("Menu");
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
        foreach (GhostBehavior ghost in ghostPrefabs)
        {
            ghost.Spawned = true;
            yield return new WaitForSecondsRealtime(timeTillSpawn);
        }
    }

    IEnumerator UpdateGameLogic()
    {

        while (isGame)
        {
            //Gameloop and update logic
            // Debug.Log("TICK : " + Time.time);
            if (power.IsPacmanSuper())
            {
                AudioManager.PlaySound("fuite");
                CurrentMode = ChaseMode.Frighten;
            }
            //Check if pacman collide an ennemy
            foreach (GhostBehavior ghost in ghostPrefabs)
            {
                ghost.ComputeNextMove(CurrentMode);
                if( ghost.position == pacman.position)
                {
                    pacman.colliding = true;
                }
            }


            if (pacman.colliding && pacmanLife > 0 && !power.IsPacmanSuper())
            {
                AudioManager.PlaySound("death");
                pacman.colliding = false;
                pacman.lastDir = MoveDir.Up;
                pacmanLife -= 1;
                UIManager.Touch();
                pacman.Spawn();

                GhostRespawn();
            }
            else if (pacman.colliding && power.IsPacmanSuper())
            {
                pacman.colliding = false;
                GhostBehavior closest = ghostPrefabs[0];
                float distance = 100f;
                foreach(GhostBehavior ghost in ghostPrefabs)
                {
                    if(Vector2.Distance(ghost.position, pacman.position) < distance)
                    {
                        closest = ghost;
                        distance = Vector2.Distance(ghost.position, pacman.position);
                        Debug.Log("Closest is : " + closest.name);
                    }
                }
                closest.Spawn();
                AudioManager.PlaySound("chomp");
            }
            else if (pacman.colliding && pacmanLife == 0)
            {
                pacman.colliding = false;
                AudioManager.PlaySound("death");
                score.SaveScore();
                UIManager.GameOver();
            }

            //Update Pacman
            pacman.Move(currDirection);

            pacman.colliding = false;

            //Wait timeTillUpdate seconds till next update cycle
            while (triggerUpdate == false)
            {
                yield return null;
            }

            triggerUpdate = false;
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

    public bool AreGhostsFrightened()
    {
        if (CurrentMode == ChaseMode.Frighten) return true;
        else return false;
    }
    
    IEnumerator UpdateChaseLogic()
    {
        for(int i=0; i<ChaseModes.Count; i++)
        {
            CurrentMode = ChaseModes[i];

            while(power.IsPacmanSuper()) yield return null;

            yield return new WaitForSecondsRealtime(ChaseTime[i]);
        }

        while (true)
        {
            CurrentMode = ChaseMode.Chase; //Whatever the last mode in the array was, defaults to chase till player either dies or level ends

            while (!power.IsPacmanSuper()) yield return null;//Fix : Eating supers sometimes prevented ghosts to switch back to chase mode, this avoids that scenario
            while (power.IsPacmanSuper()) yield return null;

            yield return new WaitForEndOfFrame();
        }
    }
    #endregion

    public void Trigger(bool trigger)
    {
        triggerUpdate = trigger;
    }
}