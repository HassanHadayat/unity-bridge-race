using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Panels")]
    [SerializeField] private GameObject gameStartPanel;
    [SerializeField] private GameObject gamePlayPanel;
    [SerializeField] private GameObject gameEndPanel;


    [SerializeField] private GameObject levelCompleted;
    [SerializeField] private GameObject levelFailed;

    public Animator UIAnimController;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        GameManager.Instance.OnLevelStartedEvent += OnClick_StartLevel;
        GameManager.Instance.OnLevelEndedEvent += OnLevelEnded;


        gameStartPanel.SetActive(true);

        gamePlayPanel.SetActive(false);
        gameEndPanel.SetActive(false);
        UIAnimController.Play("OpenGameStartPanel");
    }
    public void OnClick_RestartBtn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            OnLevelCompleted();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            OnLevelFailed();
        }
    }

    public void OnClick_StartLevel()
    {
        UIAnimController.Play("CloseGameStartPanel");
    }

    public void OnLevelEnded(bool isWon)
    {
        if (isWon)
            OnLevelCompleted();
        else
            OnLevelFailed();
    }
    public void OnLevelCompleted()
    {
        UIAnimController.Play("CloseGamePlayPanel");

        levelFailed.SetActive(false);
        levelCompleted.SetActive(true);
    }

    public void OnLevelFailed()
    {
        UIAnimController.Play("CloseGamePlayPanel");

        levelCompleted.SetActive(false);
        levelFailed.SetActive(true);
    }
}
