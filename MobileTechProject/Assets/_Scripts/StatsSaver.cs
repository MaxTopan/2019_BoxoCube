using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsSaver : MonoBehaviour
{
    public string HSString { get; private set; }
    public int currentScore { get; private set; }
    public int highScore { get; private set; }
    public GameObject player;
    private int xPos;
    public bool gameOver { get; private set; }
    UIController ui;

    // Use this for initialization
    void Awake()
    {
        gameOver = false;
        HSString = "HighScore";
        AudioListener.volume = PlayerPrefs.GetInt("GameVolume");

        ui = GetComponent<UIController>();
        highScore = PlayerPrefs.GetInt(HSString);
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null && !gameOver)
        {
            StartCoroutine(GameOver());
        }
        else if (player != null)
        {
            xPos = Mathf.RoundToInt(player.transform.position.x);
            currentScore = currentScore > xPos ? currentScore : xPos;
        }
    }

    public void SaveHighScore()
    {
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt(HSString, currentScore);
            ui.hiScore.text = "Hi Score: " + currentScore;
        }
    }

    public void SaveAudioChoice(int vol)
    {
        PlayerPrefs.SetInt("GameVolume", vol);
    }

    private IEnumerator GameOver()
    {
        gameOver = true;
        SaveHighScore();

        yield return null; // new WaitForSeconds(0.6f);
        ui.DeathScreen();
    }
}
