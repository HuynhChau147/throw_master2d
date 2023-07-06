using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    private bool isPaused;
    private bool isGameOver;
    private int bombCounterF;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject WinScreen;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioSource audioSrc;
    [SerializeField] private TextMeshProUGUI bombCounter;
    [SerializeField] private Enemy zombie;
    [SerializeField] private GameplayManager gm;

    // Active GameOver screen
    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        isGameOver = true;
    }

    private void Update() {
        bombCounterF = gm.getBombLeft();
        bombCounter.text = $"x{bombCounterF.ToString()}";

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Paused();
        }

        if((zombie.getWinStatus()))
        {
            audioSrc.Stop();
            Debug.Log("You win");
            Invoke("Win",2f);
        }
    }

    // GameOver screen func
    public void Restart()
    {
        if(isPaused == true)
        {
            pauseButton.GetComponent<PauseGame>().PausedGame();
        }
        isGameOver = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        if(isPaused == true)
        {
            pauseButton.GetComponent<PauseGame>().PausedGame();
        }
        isGameOver = false;
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        isGameOver = false;
        Application.Quit();
    }

    // Pause screen func
    public void Paused()
    {

        if(isGameOver == true)
        {
            return;
        }

        if(isPaused == false)
        {
            pauseButton.SetActive(false);
            pauseButton.GetComponent<PauseGame>().PausedGame();
            pauseScreen.SetActive(true);
            isPaused = true;
        }
        else if(isPaused == true)
        {
            pauseButton.SetActive(true);
            pauseButton.GetComponent<PauseGame>().PausedGame();
            pauseScreen.SetActive(false);
            isPaused = false;
        }
    }

    // Win screen func
    public void Win()
    {
        WinScreen.SetActive(true);
    }

    public void RestartFromTheBeginning()
    {
        if(isPaused == true)
        {
            pauseButton.GetComponent<PauseGame>().PausedGame();
        }
        isGameOver = false;
        LevelManager.Instance.LoadScence(1);
    }
}
