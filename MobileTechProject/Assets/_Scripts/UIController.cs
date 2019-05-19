using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
#if UNITY_EDITOR
    DataCollector dc; // used for tracking fuel usage
#endif

    public Text scoreText, currentScore, hiScore, deathScore, deathHiScore; // UI text
    public GameObject pausePanel, playPanel, deathPanel; // panels with the rest of the UI on them
    private Animator scoreAnim; // animator for the popping animation on score text
    public Sprite[] speakerSprites = new Sprite[2]; // sprites to toggle the speaker icon between on and off
    public Image speakerImg; // active speaker icon

    private StatsSaver stats; // reference to script managing score, game over etc
    private bool scoreAnimCalled; // prevent score animation playing repeatedly

    // Use this for initialization
    void Awake()
    {
#if UNITY_EDITOR
        dc = GameObject.FindGameObjectWithTag("DataCollector").GetComponent<DataCollector>();
#endif

        speakerImg.sprite = speakerSprites[(int)AudioListener.volume]; // keep speaker icon consistent when resetting
        scoreAnim = scoreText.gameObject.GetComponent<Animator>(); // assign animator
        stats = GetComponent<StatsSaver>(); // assign stats script
        hiScore.text = "Hi Score: " + stats.highScore; // initialise high score text
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + stats.currentScore; // display score text play screen
        currentScore.text = "Current Score: " + stats.currentScore; // display score pause screen

        if (stats.currentScore > 0 && stats.currentScore % 100 == 0 && !scoreAnimCalled) // on increments of 100 other than 0
        {
            scoreAnim.Play("ScoreTextUpdate"); // play score milestone animation
            scoreAnimCalled = true; // set bool to prevent animation from being called multiple times
        }
        else if (scoreAnimCalled && stats.currentScore % 100 != 0) // once the player moves on from the milstone score
        {
            scoreAnimCalled = false; // reset the bool so the animatio can be called at the next milestone
        }

#if UNITY_STANDALONE
        // if player is not on title screen, and time is 0 (so game is paused or player is dead)
        if ((pausePanel.activeSelf || deathPanel.activeSelf) && 
            (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)))
        {
            Retry();
        }
        // if player is on pause screen or playing
        if ((playPanel.activeSelf ^ Time.timeScale == 0) && !deathPanel.activeSelf &&
            Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
#endif
    }

    public void TogglePause()
    {
        Time.timeScale = Time.timeScale == 0 ? 1 : 0; // toggle timescale between 1 and 0

        playPanel.SetActive(!playPanel.activeSelf);
        pausePanel.SetActive(!pausePanel.activeSelf); // toggle pause panel active on inactive
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ToggleAudio()
    {
        if (AudioListener.volume == 0)
        {
            AudioListener.volume = 1;
        }
        else
        {
            AudioListener.volume = 0;
        }

        speakerImg.sprite = speakerSprites[(int)AudioListener.volume];
        stats.SaveAudioChoice((int)AudioListener.volume);
    }

    public void ResetHighScore()
    {
        PlayerPrefs.SetInt(stats.HSString, 0);
    }

    public void DeathScreen()
    {
        Time.timeScale = 0;
        playPanel.SetActive(false);
        deathPanel.SetActive(true);

#if UNITY_EDITOR
        dc.ResetRunData(stats.currentScore);
#endif

        if (stats.currentScore == stats.highScore)
        {
            deathScore.enabled = false;
            deathHiScore.color = new Color(0.898f, 1f, 0.055f);
            deathHiScore.text = "NEW HI SCORE: " + stats.highScore;
        }
        else
        {
            deathScore.text = "Score: " + stats.currentScore;
            deathHiScore.text = "Hi Score: " + stats.highScore;
        }
    }
}
