using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool IsGameStarted { get; private set; }

    [SerializeField] private GameObject tapToStartText;  

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !IsGameStarted)
        {
            StartGame();
        }
        CheckStartText();
    }

    private void CheckStartText()
    {
        if(tapToStartText == null)
        {
            tapToStartText = GameObject.Find("TapToStartText");
        }
    }

    public void StartGame()
    {
        IsGameStarted = true;
        if (tapToStartText != null)
        {
            tapToStartText.SetActive(false);  
        }
    }

    public void GameIsOver()
    {
        IsGameStarted = false;
    }

    public void RestartLevel(bool isInvokeRestart = false, float timeToRestart = 2f)
    {
        if (!isInvokeRestart)
        {
            Restart();
        }
        else
        {
            Invoke("Restart", timeToRestart);
        }
    }

    private void Restart()
    {
        GameIsOver();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        if (tapToStartText != null)
        {
            tapToStartText.SetActive(true);
        }
    }
}
