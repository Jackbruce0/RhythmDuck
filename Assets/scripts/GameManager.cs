/* GameManager.cs
 * Description: Controls game states, sets pages for 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public delegate void GameDelegate();
    public static event GameDelegate OnGameStarted;
    public static event GameDelegate OnGameOverConfirmed;

    public static GameManager Instance;
     
    public GameObject startPage;
    public GameObject gameOverPage;
    public GameObject countdownPage;
    public GameObject beatMaker;
    public Text scoreText;

    GameObject currentBeatMaker;

    enum PageState 
    {
        None,
        Start,
        GameOver,
        Countdown
    }

    int score = 0;
    bool gameOver = true;

    public bool GameOver { get { return gameOver; } } // What is this better than a get method?
    public int Score { get { return score; }}

    void Awake() 
    {
        Instance = this;
    }

    void OnEnable() //+= means subscribe
    {
        CountdownText.OnCountdownFinished += OnCountdownFinished;
        TapController.OnPlayerDied += OnPlayerDied;
        TapController.OnPlayerScored += OnPlayerScored;
    }

    void OnDisable() //-= means unsubscribe
    {
        CountdownText.OnCountdownFinished -= OnCountdownFinished;
        TapController.OnPlayerDied -= OnPlayerDied;
        TapController.OnPlayerScored -= OnPlayerScored;
    }

    void OnCountdownFinished()
    {
        SetPageState(PageState.None);
        OnGameStarted(); //event sent to TapController
        score = 0;
        gameOver = false;
        currentBeatMaker = Instantiate(beatMaker); //start the music
    }

    void OnPlayerDied()
    {
        Destroy(currentBeatMaker); //stop the music
        gameOver = true;
        int savedScore = PlayerPrefs.GetInt("Highscore"); //created High score variable in PlayerPrefs
        if (score > savedScore) {
            PlayerPrefs.SetInt("Highscore", score); //adjusts High score acordingly
        }
        SetPageState(PageState.GameOver);
    }

    void OnPlayerScored()
    {
        score++;
        scoreText.text = score.ToString();
    }


    void SetPageState(PageState state)
    {
        switch (state) 
        {
            case PageState.None:
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(false);
                break;
            case PageState.Start:
                startPage.SetActive(true);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(false);
                break;
            case PageState.GameOver:
                startPage.SetActive(false);
                gameOverPage.SetActive(true);
                countdownPage.SetActive(false);
                break;
            case PageState.Countdown:
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                countdownPage.SetActive(true);
                break;

        }
    }

    public void ConfirmGameOver() 
    {
        //activated when replay button is hit
        OnGameOverConfirmed(); //event sent to TapController
        scoreText.text = "0";
        SetPageState(PageState.Start);
    }

    public void StartGame() 
    {
        //activated when play button is hit
        SetPageState(PageState.Countdown);
    }

	
}
