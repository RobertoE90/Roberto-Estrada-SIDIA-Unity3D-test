using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameStates
{
    Started= 0,
    Running = 1,
    Dead = 2,
}
public class GameController : MonoBehaviour
{
    public GameStates gameState = GameStates.Running;

    //list all avaliable prefabs
    public List<GameObject> groundPrefabs = new List<GameObject>();
    //list all avaliable obstacles
    public List<GameObject> obstaclePrefabs = new List<GameObject>();
    //used for triggering the ground creation in relation to the runner z displacement
    private int nextGroundCreateTrigger = -150;
    //used for triggering the ground creation in relation to the runner z displacement
    private int nextObstacleCreateTrigger = 0;

    //current game speed
    public float gameCurrentSpeed = 0.0f;
    //this variable is used for linear interpolation between the current speed and the level actual speed
    public float gameMaxLevelSpeed = 0.0f;
    //limits the game speed
    private float gameMaxSpeed = 60.0f;
    //game difficulty level
    private int difficultyLevel = 1;
    //amount of time passed since level changed
    private float timeLaps = 0.0f;
    //change level duration
    private float levelDuration = 3.0f;

    //runner object reference
    public GameObject runner;
    //runner object controller reference
    private RunnerController runnerController;


    //initialices all variables
    //creates the begin ground
    void Start()
    {
        runnerController = runner.GetComponent<RunnerController>();

        //creates the 4 initial GroundObjects
        for (int i = 0; i < 4; i++) 
        {
            GenerateGround();
        }

        //game level 1 initial speed
        gameMaxLevelSpeed = (difficultyLevel + 1) * 3 + 15.0f;
    }

    void Update()
    {
        if (gameState == GameStates.Started) 
        {
            return;
        }
        else if(gameState == GameStates.Dead)
        {
            return;
        }
        timeLaps += Time.deltaTime;
        if (timeLaps > levelDuration)
        {
            timeLaps = 0;
            difficultyLevel += 1;
            //game speed depends on the game Level
            gameMaxLevelSpeed = (difficultyLevel + 1) * 3;
            if (gameMaxLevelSpeed >= gameMaxSpeed) 
            {
                gameMaxLevelSpeed = gameMaxSpeed;
            }
            levelDuration *= 1.5f;
        }

        //smoots the speed changes
        if (gameCurrentSpeed < gameMaxLevelSpeed)
        {
            gameCurrentSpeed += 0.01f;
        }
        else {
            gameCurrentSpeed = gameMaxLevelSpeed;
        }
        
        //creates a ground prefab object related to the runner displacement
        if (runner.transform.position.z >= nextGroundCreateTrigger) 
        {
            GenerateGround();
        }

        //creates a ground prefab object related to the runner displacement
        if (runner.transform.position.z + 100 >= nextObstacleCreateTrigger)
        {
            GenerateObstacle();
        }
    }


    //creates the new ground object ahead
    public void GenerateGround()
    {
       GameObject.Instantiate<GameObject>(groundPrefabs[0], 
                                          Vector3.forward * (nextGroundCreateTrigger + 100), 
                                          Quaternion.identity);
       nextGroundCreateTrigger += 50;
    }

    //creates new (random) obstacles objects ahead given a random value for the 3 rails
    public void GenerateObstacle()
    {
        for (int i = -1; i < 2; i++) {
            int randomObstacleIndex = Random.Range(0, obstaclePrefabs.Count);
            if (Random.value > 0.5f) 
            {
                //starts creating the obstacles 100 units ahead of the player point
                GameObject.Instantiate<GameObject>(obstaclePrefabs[randomObstacleIndex],
                                                   Vector3.forward * (nextObstacleCreateTrigger + 100) + Vector3.right * i * 2,
                                                   Quaternion.identity);
            }
        }
        int randomGeneratorDelta = Random.Range(10, 40);
        nextObstacleCreateTrigger += randomGeneratorDelta;
    }

    public void StartRunning() {
        gameState = GameStates.Running;
        runnerController.animator.SetTrigger("startGameTrigger");
    }

    //used for debuging propouses
    /*
    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(160, 10, 150, 400));
        GUILayout.BeginVertical();
        GUILayout.Label("level" + difficultyLevel);
        GUILayout.Label("speed " + gameCurrentSpeed);
        GUILayout.Label("nextLevel " + levelDuration);

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
     */
}
