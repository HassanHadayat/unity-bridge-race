using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public delegate void LevelStartedEvent();
    public event LevelStartedEvent OnLevelStartedEvent;

    public delegate void LevelEndedEvent(bool isWon);
    public event LevelEndedEvent OnLevelEndedEvent;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        // Set Game Target Frame Rate
        Application.targetFrameRate = 60;
    }
    private void Start()
    {
        Time.timeScale = 0f;

    }

    public void StartLevel()
    {
        Time.timeScale = 1f;
        OnLevelStartedEvent?.Invoke();
    }
    public void EndLevel(bool isWon)
    {
        OnLevelEndedEvent?.Invoke(isWon);
    }
}
