using System.Collections.Generic;
using UnityEngine;

public class PlayerInstantiator : MonoBehaviour
{
    public static PlayerInstantiator Instance;

    public CameraFollow cameraFollow;

    public List<GameObject> players;

    [Header("AI Prefabs")]
    public GameObject PlayerBluePrefab;
    public GameObject AIRedPrefab;
    public GameObject AIGreenPrefab;
    public GameObject AIPinkPrefab;

    public Transform parentTrans;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    public void InstantiatePlayers(string[] playerColors, Platform platform)
    {
        foreach (string color in playerColors)
        {
            //GameObject player = null;
            if (color == "Blue")
            {
                GameObject player = Instantiate(PlayerBluePrefab, parentTrans);
                player.GetComponent<PlayerController>().currPlatform = platform;
                cameraFollow.SetTarget(player.transform);
                players.Add(player);
            }
            else if (color == "Red")
            {
                GameObject player = Instantiate(AIRedPrefab, parentTrans);
                player.GetComponent<AIController>().currPlatform = platform;
                players.Add(player);
            }
            else if (color == "Green")
            {
                //players.Add(Instantiate(AIGreenPrefab, parentTrans));
                GameObject player = Instantiate(AIGreenPrefab, parentTrans);
                player.GetComponent<AIController>().currPlatform = platform;
                players.Add(player);
            }
            else if (color == "Pink")
            {
                //players.Add(Instantiate(AIPinkPrefab, parentTrans));
                GameObject player = Instantiate(AIPinkPrefab, parentTrans);
                player.GetComponent<AIController>().currPlatform = platform;
                players.Add(player);
            }
            //players.Add(player);
        }
    }


}
