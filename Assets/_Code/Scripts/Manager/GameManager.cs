using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        // Set Game Target Frame Rate
        Application.targetFrameRate = 60;
    }
}
