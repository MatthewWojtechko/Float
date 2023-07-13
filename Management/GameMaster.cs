using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    BalloonLevel balloonLevel = BalloonLevel.L1;

    public Transform mainBalloon;
    public Animator canvasAnim;
    public Text levelCompleteText;
    public Material secondLevelSkybox;
    public Material thirdLevelSkybox;
    public Text balloonsRemainingText;
    public GameObject PauseUI;
    public GameObject endScene;
    public GameObject maincam;
    public GameObject secondcam;
    public GameObject cloudWorld;
    public GameObject groundWorld;
    public GameObject level1Balloons;
    public GameObject level2Balloons;
    public GameObject level3Balloons;

    BalloonFly mainBalloonScript;
    Constants constants;
    AudioSource audioSource;

    int numberOfLevel01Balloons = 0;
    int numberOfLevel02Balloons = 0;
    int numberOfLevel03Balloons = 0;

    int numberOfFoundBalloons = 0;
    int currentLevel = 1;
    bool gameIsPaused = false;
    bool gameHasStarted = false;

    void Awake()
    {
        StationaryBalloon.onStart += IncrementNumberOfBalloonsPerLevel;
        StationaryBalloon.onBalloonGet += IncrementNumberOfFoundBalloonsPerLevel;
        Messenger.AddListener(GameEvent.START_LEVEL_ONE, OnGameStart);
        Messenger.AddListener(GameEvent.OPENING, PlayWindSFX);
    }

    void Start()
    {
        AudioListener.pause = false;
        Time.timeScale = 1;
        Application.targetFrameRate = 60;

        audioSource = GetComponent<AudioSource>();
        constants = Constants.instance;
        mainBalloonScript = mainBalloon.GetComponent<BalloonFly>();
        balloonsRemainingText.text = (numberOfLevel01Balloons - numberOfFoundBalloons).ToString();
    }

    private void Update()
    {
        if (gameHasStarted)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePauseGame();
            }
            if (gameIsPaused)
            {
                if (Input.GetKeyDown(KeyCode.Y))
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
                if (Input.GetKeyDown(KeyCode.N))
                {
                    TogglePauseGame();
                }
            }
        }
    }

    void TogglePauseGame()
    {
        if (gameIsPaused)
        {
            //AudioListener.pause = false;
            gameIsPaused = false;
            Time.timeScale = 1;
            PauseUI.SetActive(false);
            balloonsRemainingText.enabled = true;
        }
        else
        {
            //AudioListener.pause = true;
            gameIsPaused = true;
            Time.timeScale = 0;
            PauseUI.SetActive(true);
            balloonsRemainingText.enabled = false;
        }
    }

    private void OnDestroy()
    {
        StationaryBalloon.onStart -= IncrementNumberOfBalloonsPerLevel;
        StationaryBalloon.onBalloonGet -= IncrementNumberOfFoundBalloonsPerLevel;
        Messenger.RemoveListener(GameEvent.START_LEVEL_ONE, OnGameStart);
        Messenger.RemoveListener(GameEvent.OPENING, PlayWindSFX);
    }

    void PlayWindSFX()
    {
        audioSource.Play();
    }

    void OnGameStart()
    {
        gameHasStarted = true;
        balloonsRemainingText.gameObject.SetActive(true);
        level1Balloons.SetActive(true);
    }

    void IncrementNumberOfBalloonsPerLevel(BalloonLevel balloonLevel)
    {
        switch (balloonLevel)
        {
            case BalloonLevel.L1:
                numberOfLevel01Balloons++;
                break;
            case BalloonLevel.L2:
                numberOfLevel02Balloons++;
                break;
            case BalloonLevel.L3:
                numberOfLevel03Balloons++;
                break;
        }
    }

    void IncrementNumberOfFoundBalloonsPerLevel(BalloonLevel _balloonLevel)
    {
        int numberOfRemainingBalloons = 0;
        numberOfFoundBalloons++;

        switch (balloonLevel)
        {
            case BalloonLevel.L1:
                numberOfRemainingBalloons = numberOfLevel01Balloons - numberOfFoundBalloons;
                if (numberOfFoundBalloons >= numberOfLevel01Balloons)
                {
                    numberOfRemainingBalloons = numberOfLevel02Balloons;
                    RenderSettings.skybox = secondLevelSkybox;
                    numberOfFoundBalloons = 0;
                    balloonLevel = BalloonLevel.L2;
                    Messenger.Broadcast(GameEvent.TRANSITION_TO_TWO);
                    ChangeBounds(constants.level02Boundary.lowerBoundary, constants.level02Boundary.upperBoundary);
                    currentLevel = 2;
                    level2Balloons.SetActive(true);
                }
                break;
            case BalloonLevel.L2:
                numberOfRemainingBalloons = numberOfLevel02Balloons - numberOfFoundBalloons;
                if (numberOfFoundBalloons >= numberOfLevel02Balloons)
                {
                    numberOfRemainingBalloons = numberOfLevel03Balloons;
                    balloonsRemainingText.text = (numberOfLevel01Balloons - numberOfFoundBalloons).ToString();
                    RenderSettings.skybox = thirdLevelSkybox;
                    numberOfFoundBalloons = 0;
                    balloonLevel = BalloonLevel.L3;
                    Messenger.Broadcast(GameEvent.TRANSITION_TO_THREE);
                    ChangeBounds(constants.level03Boundary.lowerBoundary, constants.level03Boundary.upperBoundary);
                    currentLevel = 3;
                    level3Balloons.SetActive(true);
                }
                break;
            case BalloonLevel.L3:
                numberOfRemainingBalloons = numberOfLevel03Balloons - numberOfFoundBalloons;
                if (numberOfFoundBalloons >= numberOfLevel03Balloons)
                {
                    numberOfFoundBalloons = 0; // make sure this doesn't repeat
                    StartCoroutine(startEndScene());
                }
                break;
        }
        balloonsRemainingText.text = numberOfRemainingBalloons.ToString();
    }

    // Show end level text, wait, and start the end.
    IEnumerator startEndScene()
    {
        levelCompleteText.text = "Level " + currentLevel + "/3 Complete";
        canvasAnim.enabled = true;
        canvasAnim.Play("LevelComplete", -1, 0);
        yield return new WaitForSeconds(3);
        Messenger.Broadcast(GameEvent.END_SCENE);
        groundWorld.SetActive(true);
        groundWorld.SetActive(false);
        maincam.SetActive(false);
        groundWorld.SetActive(true);
        secondcam.SetActive(true);
        cloudWorld.SetActive(false);
        endScene.SetActive(true);
        balloonsRemainingText.gameObject.SetActive(false);
    }

    void ChangeBounds(float lower, float upper)
    {
        levelCompleteText.text = "Level " + currentLevel + "/3 Complete";
        canvasAnim.enabled = true;
        canvasAnim.Play("LevelComplete", -1, 0);
        mainBalloonScript.SetBounds(lower, upper);
    }
}
